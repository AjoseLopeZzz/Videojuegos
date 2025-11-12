using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DepositZoneWithCrates : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Producto que recibe")]
    [SerializeField] private CropType targetCrop = CropType.Corn;

    [Header("Prefabs")]
    [SerializeField] private GameObject cratePrefab;    // Caja
    [SerializeField] private GameObject itemPrefab;     // Mazorca (con o sin Rigidbody)

    [Header("Columna (spawn de CAJAS)")]
    [SerializeField] private Transform rowParent;       // Tiene el BoxCollider columna
    [SerializeField] private string columnBoxOnParent = "";
    [SerializeField] private Vector3 columnStartOffsetLocal = Vector3.zero;
    [SerializeField] private float extraVerticalGap = 0.02f;

    [Header("Items dentro de la caja")]
    [SerializeField] private string innerSpawnName = "SpawnArea"; // opcional
    [SerializeField] private float itemScale = 0.1f;
    [SerializeField] private float spawnHeightOffset = 0.05f;
    [SerializeField] private float jitter = 0.01f;
    [SerializeField] private int itemsPerCrate = 6;

    private readonly List<CrateState> crates = new();
    private BoxCollider columnBox;
    private float verticalStep = 0.25f;

    private void Awake()
    {
        // localizar el BoxCollider de la columna
        columnBox = string.IsNullOrEmpty(columnBoxOnParent)
            ? rowParent.GetComponent<BoxCollider>()
            : rowParent.Find(columnBoxOnParent)?.GetComponent<BoxCollider>();

        if (columnBox == null)
            Debug.LogError("[DepositZone] Falta BoxCollider en la columna (rowParent).");
    }

    /// <summary>
    /// Llama esto cuando el jugador esté sobre la zona (esfera/trigger).
    /// </summary>
    public void ExecuteDeposit()
    {
        if (columnBox == null || inventoryManager == null) return;

        var inv = inventoryManager.GetInventory();
        if (inv == null) return;

        int amount = InventoryUtils.GetAmount(inv, targetCrop);
        if (amount <= 0) return;

        // restar del inventario y refrescar UI/guardado
        InventoryUtils.RemoveAmount(inv, targetCrop, amount);
        inventoryManager.GetComponent<InventoryDisplay>()?.UpdateDisplay(inv);
        inventoryManager.SendMessage("SaveInventory", SendMessageOptions.DontRequireReceiver);

        DepositItems(amount);
    }

    private void DepositItems(int amount)
    {
        int remaining = amount;

        if (crates.Count == 0)
            crates.Add(SpawnCrateAtIndex(0)); // calcula verticalStep

        // llenar cajas existentes
        for (int i = 0; i < crates.Count && remaining > 0; i++)
        {
            int canPut = Mathf.Min(remaining, itemsPerCrate - crates[i].count);
            SpawnItemsInCrate(crates[i], canPut);
            crates[i].count += canPut;
            remaining -= canPut;
        }

        // crear nuevas si falta
        while (remaining > 0)
        {
            var c = SpawnCrateAtIndex(crates.Count);
            int canPut = Mathf.Min(remaining, itemsPerCrate);
            SpawnItemsInCrate(c, canPut);
            c.count += canPut;
            remaining -= canPut;
            crates.Add(c);
        }
    }

    private CrateState SpawnCrateAtIndex(int index)
    {
        var go = Instantiate(cratePrefab, rowParent, false);
        go.transform.localRotation = Quaternion.identity;

        // medir altura caja la primera vez
        if (index == 0)
        {
            verticalStep = Mathf.Max(0.05f, GetLocalBoundsHeight(go.transform)) + extraVerticalGap;
        }

        // posicionar en la columna (usar BoxCollider de rowParent)
        float bottomY = columnBox.center.y - columnBox.size.y * 0.5f;
        var baseLocal = new Vector3(columnBox.center.x, bottomY, columnBox.center.z) + columnStartOffsetLocal;
        go.transform.localPosition = baseLocal + Vector3.up * (index * verticalStep);

        // spawn interno opcional
        Transform inner = go.transform.Find(innerSpawnName);
        BoxCollider innerBox = inner ? inner.GetComponent<BoxCollider>() : null;

        return new CrateState { root = go.transform, innerSpawn = innerBox, count = 0 };
    }

    private void SpawnItemsInCrate(CrateState crate, int n)
    {
        if (crate.innerSpawn != null)
        {
            // Spawnea arriba del Box interno y deja caer
            Transform parent = crate.innerSpawn.transform;
            Vector3 center = crate.innerSpawn.center;
            Vector3 size = crate.innerSpawn.size;

            float topY = center.y + size.y * 0.5f + spawnHeightOffset;
            float bottomY = center.y - size.y * 0.5f + 0.01f;

            for (int i = 0; i < n; i++)
            {
                float x = center.x + Random.Range(-size.x * 0.5f, size.x * 0.5f);
                float z = center.z + Random.Range(-size.z * 0.5f, size.z * 0.5f);

                var item = Instantiate(itemPrefab, parent, false);
                item.transform.localScale = Vector3.one * itemScale;
                item.transform.localPosition = new Vector3(x, topY, z);
                item.transform.localRotation = Random.rotationUniform;

                if (item.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                else
                {
                    item.transform.localPosition = new Vector3(x, bottomY, z);
                }
            }
        }
        else
        {
            // Sin SpawnArea: coloca centrado dentro
            Vector3 size = new Vector3(0.5f, 0.2f, 0.35f);
            Vector3 center = new Vector3(0, 0.1f, 0);

            for (int i = 0; i < n; i++)
            {
                float x = center.x + Random.Range(-size.x * 0.5f, size.x * 0.5f);
                float z = center.z + Random.Range(-size.z * 0.5f, size.z * 0.5f);

                var item = Instantiate(itemPrefab, crate.root, false);
                item.transform.localScale = Vector3.one * itemScale;
                item.transform.localPosition = new Vector3(x, center.y, z);
                item.transform.localRotation = Random.rotationUniform;
            }
        }
    }

    private float GetLocalBoundsHeight(Transform t)
    {
        var renderers = t.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 0.25f;

        Bounds world = renderers[0].bounds;
        foreach (var r in renderers) world.Encapsulate(r.bounds);

        Vector3 localMin = t.InverseTransformPoint(world.min);
        Vector3 localMax = t.InverseTransformPoint(world.max);
        return Mathf.Abs(localMax.y - localMin.y);
    }

    private class CrateState
    {
        public Transform root;
        public BoxCollider innerSpawn; // opcional
        public int count;
    }
}
