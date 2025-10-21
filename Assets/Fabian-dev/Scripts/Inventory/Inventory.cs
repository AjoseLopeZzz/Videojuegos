using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

        if (!cropFound)
        {
            items.Add(new InventoryItem(cropType, 1));
        }

        DebugInventory();
    }

    /// <summary>
    /// Devuelve todos los ítems actuales del inventario.
    /// </summary>
    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    /// <summary>
    /// Limpia completamente el inventario.
    /// </summary>
    public void Clear()
    {
        items.Clear();
    }

    /// <summary>
    /// Cambia manualmente la cantidad de un tipo de cultivo.
    /// Si no existe y el valor es mayor a 0, lo agrega.
    /// Si el valor llega a 0, lo elimina de la lista.
    /// </summary>
    public void SetAmount(CropType type, int newAmount)
    {
        bool found = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].cropType == type)
            {
                found = true;

                if (newAmount <= 0)
                {
                    // elimina si llega a 0
                    items.RemoveAt(i);
                }
                else
                {
                    InventoryItem updated = items[i];
                    updated.amount = newAmount;
                    items[i] = updated;
                }

                break;
            }
        }

        // si no existe y la cantidad es mayor a 0, lo agrega
        if (!found && newAmount > 0)
        {
            items.Add(new InventoryItem(type, newAmount));
        }

        DebugInventory();
    }

    /// <summary>
    /// Muestra en consola las cantidades actuales (solo para debug).
    /// </summary>
    public void DebugInventory()
    {
        foreach (InventoryItem item in items)
        {
            // Descomenta para ver los datos en consola:
            // Debug.Log("Tenemos " + item.amount + " de " + item.cropType);
        }
    }
}
