using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    /// <summary>
    /// Se llama cuando se cosecha un cultivo.
    /// Si ya existe en la lista, incrementa su cantidad.
    /// Si no existe, lo agrega.
    /// </summary>
    public void CropHarvestedCallback(CropType cropType)
    {
        bool cropFound = false;

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];
            if (item.cropType == cropType)
            {
                item.amount++;
                items[i] = item; // aseguramos que el cambio quede guardado (struct-safe)
                cropFound = true;
                break;
            }
        }

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

    //  NUEVOS MeTODOS (no reemplazan ninguno)
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

    // dentro de la clase Inventory
    public void SetCropAmount(CropType type, int amount)
    {
        // Si amount es 0 o menos, eliminamos el ítem si existe.
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].cropType == type)
            {
                if (amount <= 0)
                {
                    items.RemoveAt(i);
                }
                else
                {
                    items[i].amount = amount;
                }
                return;
            }
        }

        // Si no existe y amount > 0, lo anadimos
        if (amount > 0)
        {
            items.Add(new InventoryItem(type, amount));
        }
    }
}
