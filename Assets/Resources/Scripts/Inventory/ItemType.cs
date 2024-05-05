using System.Collections.Generic;
using UnityEngine;

public class ItemType{
    private int amount;
    private readonly int maxStackSize;
    private readonly string itemName;
    private readonly Texture texture;
    private readonly List<ItemSlot> slots;
    private readonly Inventory inventory;

    public ItemType(string itemName, int stackSize, Texture texture, Inventory inventory){
        this.itemName = itemName;
        this.inventory = inventory;
        this.texture = texture;
        maxStackSize = stackSize;
        slots = new();
        amount = 0;
    }

    /// <summary>
    /// Adds the item first to already existing slots of the same type. 
    /// If all existing slots are full it creates a new one if the inventory hasn't reached the maximum amount of slots.
    /// </summary>
    /// <param name="amount"> The amount of new added items. </param>
    /// <returns> Returns 0, if all items are stored in the inventory. 
    /// If it can't store all items because the inventory has no free slot it returns the amount, that can't be stored in a slot.
    /// </returns>
    public int Add(int amount){
        this.amount += amount;
        for (int i = slots.Count - 1; i >= 0; i--){
            amount = AddExistingSlot(amount, slots[i]);
            if (amount == 0){
                return 0;
            }
        }
        while (amount > 0 && inventory.NextFreeSlot() != new Vector2Int(-1, -1)){
            amount = AddNewSlot(amount, inventory.NextFreeSlot());
        }
        this.amount -= amount;
        return amount;
    }

    /// <summary>
    /// Adds the item first to already existing slots of the same type. 
    /// If all existing slots are full it creates a new one if the inventory hasn't reached the maximum amount of slots.
    /// </summary>
    /// <param name="amount"> The amount of new added items. </param>
    /// <param name="position"> The position to add the items </param>
    /// <returns> Returns 0, if all items are stored in this slot. 
    /// If it can't store all items because this position has not enough space it returns the remaining amount.
    /// </returns>
    public int Add(int amount, Vector2Int position){
        if (position == new Vector2Int(-1, -1)){
            return Add(amount);
        }
        this.amount += amount;
        ItemSlot slot = GetItemSlot(position);
        if (slot == null){
            slot = new ItemSlot(position, this);
            slots.Add(slot);
        }
        amount = AddExistingSlot(amount, slot);
        this.amount -= amount;
        return amount;
    }

    private int AddExistingSlot(int amount, ItemSlot slot){
        int newAmount = slot.Add(amount);
        if (amount != newAmount){
            inventory.InventoryCanvas?.Amount(slot.Position, slot.Amount);
        }
        return newAmount;
    }

    private int AddNewSlot(int amount, Vector2Int position){
        slots.Add(new ItemSlot(position, this));
        amount = slots[^1].Add(amount);
        inventory.InventoryCanvas?.AddSlot(slots[^1].Position, itemName, slots[^1].Amount, texture);
        inventory.FreeSlot[slots[^1].Position.x][slots[^1].Position.y] = false;
        return amount;
    }

    /// <summary>
    /// Removes if possible the given amount of item from the inventory. 
    /// </summary>
    /// <param name="amount"> The amount to remove from this item </param>
    /// <returns> True, if the inventory had more than or equal to the amount given in item.
    /// False, if someone tried to remove more items than existing
    /// </summary>
    public bool Remove(int amount){
        if (this.amount < amount){
            return false;
        }
        this.amount -= amount;
        for (int i = slots.Count - 1; i >= 0; i--){
            amount = slots[i].Remove(amount);
            if (amount == 0){
                break;
            } else if (amount == -1){
                inventory.InventoryCanvas?.RemoveSlot(slots[i].Position);
                inventory.FreeSlot[slots[i].Position.x][slots[i].Position.y] = true;
                slots.RemoveAt(i);
                break;
            } else {
                inventory.InventoryCanvas?.RemoveSlot(slots[i].Position);
                inventory.FreeSlot[slots[i].Position.x][slots[i].Position.y] = true;
                slots.RemoveAt(i);
            }
        }
        return true;
    }

    public bool Remove(int amount, Vector2Int position){
        ItemSlot slot = GetItemSlot(position);
        if (amount > slot.Amount){
            return false;
        } 
        this.amount -= amount;
        slot.Remove(amount);
        if (slot.Amount == 0){
            slots.Remove(slot);
            inventory.FreeSlot[slot.Position.x][slot.Position.y] = true;
        }
        return true;
    }

    /// <summary>
    /// Removes the stack on this position from the players inventory. Removes it only, if it has the same type than this helpers items.
    /// </summary>
    /// <returns> True, if the slot was used by this slot. False if it didn't exist in this slot </returns>
    public void RemoveStack(Vector2Int position){
        Remove(GetItemSlot(position).Amount, position);
    }

    /// <summary>
    /// Moves this item to a new position. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="oldPosition"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    /// <param name="inventory"> The new inventory for this item </param>
    public void Move(Vector2Int oldPosition, Vector2Int newPosition, Inventory inventory){
        ItemSlot slot = GetItemSlot(oldPosition);
        if (inventory == this.inventory){
            slot.Move(newPosition);
            return;
        }
        if (slot == null){
            Debug.LogError("Item wasn't found at " + oldPosition.ToString());
        }
        ItemType newType = inventory.GetItemType(itemName);
        if (newType == null){
            newType = new(itemName, maxStackSize, texture, inventory);
            inventory.Type.Add(newType);
        }
        slot.Move(newPosition, newType);
        slots.Remove(slot);
        newType.slots.Add(slot);
        amount -= slot.Amount;
        newType.amount += slot.Amount;
    }

    /// <summary> Returns true, if an item of this type is stored at position </summary>
    /// <param name="position"> The position to check </param>
    /// <returns> True, if the position exists here, otherwise false. </returns>
    public bool Contains(Vector2Int position){
        if (GetItemSlot(position) == null) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary> Returns the ItemSlot, which stores all information about this item on the given position. </summary>
    /// <param name="position"> The position to check </param>
    /// <returns> The ItemSlot for this position. null if the position wasn't found. </returns>
    public ItemSlot GetItemSlot(Vector2Int position){
        foreach (ItemSlot slot in slots) {
            if (slot.Position == position){
                return slot;
            }
        }
        return null;
    }

    public string ItemName { get => itemName; }
    public int Amount { get => amount; }
    public Inventory Inventory { get => inventory; }
    public List<ItemSlot> Slots { get => slots; }
    public int MaxStackSize { get => maxStackSize; }
    public Texture Texture => texture;
}