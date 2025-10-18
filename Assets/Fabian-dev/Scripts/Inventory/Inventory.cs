using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    public void CropHarvestedCallback(CropType cropType)
    {
        bool cropFound = false;
        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];
            if (item.cropType == cropType)
            {
                item.amount++;
                cropFound = true;
                break;
            }
        }
        DebugInventory();
        if (cropFound)
            return;

        items.Add(new InventoryItem(cropType, 1));
    }

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    public void Clear()
    {
        items.Clear();
    }

    public void DebugInventory()
    {
        foreach (InventoryItem item in items)
        {
            // Debug.Log("tenemos " + item.amount + " items " + item.cropType);
        }
    }
    public int GetCropAmount(CropType type)
    {
        foreach (InventoryItem item in items)
        {
            if (item.cropType == type)
                return item.amount;
        }
        return 0;
    }


    //  NUEVOS MÉTODOS (no reemplazan ninguno)
    public int GetAmountOfCrop(CropType type)
    {
        foreach (InventoryItem item in items)
            if (item.cropType == type)
                return item.amount;
        return 0;
    }

    public void RemoveCrop(CropType type, int amount)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].cropType == type)
            {
                items[i].amount = Mathf.Max(0, items[i].amount - amount);
                if (items[i].amount <= 0)
                    items.RemoveAt(i);
                return;
            }
        }
    }
}
