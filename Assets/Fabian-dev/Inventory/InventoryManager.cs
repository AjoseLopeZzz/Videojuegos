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
        //
        inventory.CropHarvestedCallback(cropType);
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }
    [NaughtyAttributes.Button]
    private void ClearInventory()
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
        inventory= JsonUtility.FromJson<Inventory>(data);
            if (inventory == null)
                inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText( dataPath, data);
    }


}
