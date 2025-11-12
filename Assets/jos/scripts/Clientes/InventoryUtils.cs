using UnityEngine;
public static class InventoryUtils
{
    // cuántas unidades hay de un tipo
    public static int GetAmount(Inventory inv, CropType type)
    {
        var arr = inv.GetInventoryItems();
        for (int i = 0; i < arr.Length; i++)
            if (arr[i].cropType == type) return arr[i].amount;
        return 0;
    }

    // resta 'amount' unidades de ese tipo (tope a lo que exista)
    public static void RemoveAmount(Inventory inv, CropType type, int amount)
    {
        if (amount <= 0) return;

        // ejemplo directo: si tu Inventory tiene una lista interna editable,
        // añade aquí el código para disminuir. Si no, agrega en tu Inventory
        // un método público que lo haga y llámalo desde aquí.
        var items = inv.GetInventoryItems();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].cropType == type)
            {
                int newAmount = Mathf.Max(0, items[i].amount - amount);
                int delta = items[i].amount - newAmount;

                // *** IMPORTANTE ***
                // reemplaza la siguiente línea por la que corresponda en tu Inventory
                // para escribir el nuevo valor. Si tienes un setter o método,
                // úsalo. Si no, crea uno (ej.: inv.SetAmount(type, newAmount);)

                inv.SetAmount(type, newAmount); // <-- crea este método si no existe

                return;
            }
        }
    }
}
