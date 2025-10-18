using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    //  Nuevo: evento opcional para desbloqueos al vender o recolectar cultivos especiales
    public static event System.Action<CropType, int> onSpecialCropCollected;

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
}
