using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class PurchaseUnlock : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Wallet wallet;          // tu objeto Wallet en escena
    [SerializeField] private GameObject model3D;     // hijo con el modelo a mostrar
    [SerializeField] private GameObject uiCanvas;    // hijo con el canvas (nombre, precio, icono)
    [SerializeField] private string playerTag = "Player";

    [Header("Datos del ítem")]
    [SerializeField] private string itemId = "unlock_item_1"; // ID único para keysData
    [SerializeField] private string itemName = "Arado";
    [SerializeField] private int price = 20;                    // se refleja en el TMP

    [Header("UI del Canvas (TMP)")]
    [SerializeField] private TextMeshProUGUI txtName;   // nombre del objeto
    [SerializeField] private TextMeshProUGUI txtPrice;  // precio mostrado

    private bool unlocked;

    private void Awake()
    {
        // Cargar estado (keysData)
        unlocked = KeysRepository.IsUnlocked(itemId);

        // Sincronizar UI del canvas con los datos
        if (txtName) txtName.text = itemName;
        if (txtPrice) txtPrice.text = price.ToString();

        // Asegurar que el collider sea trigger
        var col = GetComponent<Collider>();
        if (col && !col.isTrigger)
        {
            Debug.LogWarning($"[PurchaseUnlock:{name}] El Collider no estaba como Trigger. Se marcará automáticamente.");
            col.isTrigger = true;
        }

        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        if (model3D) model3D.SetActive(unlocked);
        if (uiCanvas) uiCanvas.SetActive(!unlocked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unlocked) return;                    // ya está comprado/desbloqueado
        if (!other.CompareTag(playerTag)) return;

        if (wallet == null)
        {
            Debug.LogWarning("[PurchaseUnlock] Wallet no asignado.");
            return;
        }

        // Al pasar el jugador: si alcanza el dinero, descuenta y desbloquea
        if (wallet.Money >= price)
        {
            if (!wallet.Spend(price)) return; // descuenta y dispara OnMoneyChanged

            unlocked = true;
            KeysRepository.SetUnlocked(itemId, true); // guarda en keysData.txt

            ApplyVisibility(); // oculta canvas, muestra modelo
        }
        // Si no alcanza, no hace nada; el canvas permanece visible
    }
}
