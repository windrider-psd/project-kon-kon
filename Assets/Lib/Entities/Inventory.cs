using Assets.Lib.ValuePairs;
using System;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class Inventory
{
    public int maxCargoSpace;

    public InventoryEntry[] inventory;

    public bool CanAddToInventory(GoodsId goodsId, int quantity)
    {
        if (quantity <= 0)
            return false;

        int currentCargo = 0;

        for (int i = 0; i < inventory.Length; i++)
        {
            currentCargo += inventory[i].value;
        }

        return currentCargo + quantity <= maxCargoSpace;
    }

    public bool AddToInventory(GoodsId goodsId, int quantity)
    {
        if (!CanAddToInventory(goodsId, quantity))
            return false;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].key == goodsId)
            {
                inventory[i].value += quantity;
                return true;
            }
        }

        Array.Resize(ref inventory, inventory.Length + 1);

        inventory[inventory.Length - 1] = new InventoryEntry
        {
            key = goodsId,
            value = quantity
        };

        return true;
    }
    public int GetCargoUsed()
    {
        int total = 0;

        for (int i = 0; i < inventory.Length; i++)
        {
            total += inventory[i].value;
        }

        return total;
    }

    public int GetCargoAvailable()
    {
        return maxCargoSpace - GetCargoUsed();
    }

    public int GetQuantity(GoodsId goodsId)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].key == goodsId)
                return inventory[i].value;
        }

        return 0;
    }

    public float nearFullPercentage = 0.8f;

    public bool IsInventoryFull()
    {
        return GetCargoUsed() >= maxCargoSpace;
    }

    public bool IsInventoryNearFull()
    {
        return GetCargoUsed() >= maxCargoSpace * nearFullPercentage;
    }

    public void ClearInventory()
    {
        inventory = Array.Empty<InventoryEntry>();
    }
}
