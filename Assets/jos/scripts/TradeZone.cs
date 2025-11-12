using UnityEngine;

public class TradeZone : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Wallet wallet;
    [SerializeField] private bool autoSellOnEnter = true;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!autoSellOnEnter || !other.CompareTag(playerTag)) return;

        var inventory = inventoryManager.GetInventory();
        var items = inventory.GetInventoryItems();

        int totalMoney = 0;
        for (int i = 0; i < items.Length; i++)
        {
            int price = DataManager.instance.GetCropPriceFromCropType(items[i].cropType); // añade esto en tu DataManager
            totalMoney += items[i].amount * price;
        }

        if (totalMoney > 0)
        {
            wallet.Add(totalMoney);
            inventory.Clear();
            // refrescar UI del inventario
            GetComponent<InventoryDisplay>()?.UpdateDisplay(inventory); // si el display vive aquí
            // si el display no está en este objeto, llama al que ya usas:
            // inventoryManager.GetComponent<InventoryDisplay>().UpdateDisplay(inventory);

            // guarda: inventario + dinero (ver nota abajo)
            inventoryManager.SendMessage("SaveInventory", SendMessageOptions.DontRequireReceiver);
            PlayerPrefs.SetInt("money", wallet.Money); // opción simple; ver “Guardado” abajo
            PlayerPrefs.Save();
        }
    }
}
