using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    // Instancia global
    public static InventoryManager instance;

    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    // Evento para desbloqueos por cultivos especiales
    public static event System.Action<CropType, int> onSpecialCropCollected;

    private void Awake()
    {
        // Asignar instancia singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        dataPath = Application.dataPath + "/inventoryData.txt";
        LoadInventory();
        ConfigureInventoryDisplay();
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

    private void LoadInventory()
    {
        string data = "";
        if (File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);
            if (inventory == null)
                inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }
    }

    public void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
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
