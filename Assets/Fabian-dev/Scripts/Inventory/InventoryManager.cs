using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    // Instancia global (Singleton)
    public static InventoryManager instance;

    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    // Evento para desbloqueos por cultivos especiales
    public static event System.Action<CropType, int> onSpecialCropCollected;

    private void Awake()
    {
        // Asignar instancia única
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Ruta de guardado (solo usada en escritorio/móvil)
        dataPath = Path.Combine(Application.persistentDataPath, "inventoryData.txt");

        LoadInventory();
        ConfigureInventoryDisplay();

        // Suscribirse al evento de cosecha
        CropTIle.onCropHarvested += CropHarvestedCallback;
    }

    private void OnDestroy()
    {
        CropTIle.onCropHarvested -= CropHarvestedCallback;
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }

    private void CropHarvestedCallback(CropType cropType)
    {
        inventory.CropHarvestedCallback(cropType);
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();

        // Activar evento si el cultivo es especial (Maíz o Tomate)
        if (cropType == CropType.Corn || cropType == CropType.Tomato)
        {
            int totalMaiz = inventory.GetCropAmount(CropType.Corn);
            int totalTomate = inventory.GetCropAmount(CropType.Tomato);

            onSpecialCropCollected?.Invoke(CropType.Corn, totalMaiz);
            onSpecialCropCollected?.Invoke(CropType.Tomato, totalTomate);
        }
    }

    [NaughtyAttributes.Button]
    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    // ==========================
    // MÉTODOS DE GUARDADO / CARGA
    // ==========================

    private void LoadInventory()
    {
#if UNITY_WEBGL
        // En WebGL usamos PlayerPrefs (no hay acceso al sistema de archivos)
        if (PlayerPrefs.HasKey("inventoryData"))
        {
            string data = PlayerPrefs.GetString("inventoryData");
            inventory = JsonUtility.FromJson<Inventory>(data);
            if (inventory == null)
                inventory = new Inventory();
        }
        else
        {
            inventory = new Inventory();
        }
#else
        // En PC/Móvil usamos archivo persistente
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);
            if (inventory == null)
                inventory = new Inventory();
        }
        else
        {
            inventory = new Inventory();
            File.WriteAllText(dataPath, ""); // crea archivo vacío
        }
#endif
    }

    public void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);

#if UNITY_WEBGL
        PlayerPrefs.SetString("inventoryData", data);
        PlayerPrefs.Save();
#else
        File.WriteAllText(dataPath, data);
#endif
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    // ==========================
    // MÉTODOS PÚBLICOS NUEVOS
    // ==========================

    /// <summary>
    /// Retorna la cantidad actual de un cultivo específico.
    /// </summary>
    public int GetCropAmount(CropType type)
    {
        return inventory != null ? inventory.GetCropAmount(type) : 0;
    }

    /// <summary>
    /// Resta una cantidad de un cultivo del inventario.
    /// Si la cantidad es mayor que la disponible, la deja en cero.
    /// </summary>
    public void RemoveCrop(CropType type, int cantidad)
    {
        if (inventory == null) return;

        int actual = inventory.GetCropAmount(type);
        int nuevaCantidad = Mathf.Max(0, actual - cantidad);
        inventory.SetCropAmount(type, nuevaCantidad);

        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();

        Debug.Log($"Se descontaron {cantidad} de {type}. Nuevo total: {nuevaCantidad}");
    }
}
