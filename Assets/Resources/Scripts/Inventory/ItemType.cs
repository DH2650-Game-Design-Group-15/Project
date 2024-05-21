using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Stores all Items of one type for a given inventory. 
/// </summary>
public class ItemType{
    private int amount;
    private readonly int maxStackSize;
    private readonly string itemName;
    private readonly Texture texture;
    private readonly GameObject prefab;
    private readonly List<ItemSlot> slots;
    private readonly Inventory inventory;

    /// <summary> Constructor </summary>
    /// <param name="itemName"> The name of this item </param>
    /// <param name="stackSize"> The maximum amount of items stored in this slot. </param>
    /// <param name="texture"> The image of the Item in the inventory </param>
    /// <param name="prefab"> The prefab of this GameObject when it gets removed from the inventory. </param>
    /// <param name="inventory"> The inventory which stores the items in this object. </param>
    public ItemType(string itemName, int stackSize, Texture texture, GameObject prefab, Inventory inventory){
        this.itemName = itemName;
        this.inventory = inventory;
        this.texture = texture;
        this.prefab = prefab;
        maxStackSize = stackSize;
        slots = new();
        amount = 0;
    }

    /// <summary> Constructor. It takes name, stack size, texture and the prefab from the item </summary>
    /// <param name="item"> Object containing name, stack size, texture and prefab. </param>
    /// <param name="inventory"> The inventory which stores the items in this object. </param>
    public ItemType(Item item, Inventory inventory):this(item.GetType().ToString(), item.MaxStackSize, item.ImageInventory, item.Prefab, inventory){
    }

    /// <summary>
    /// Adds the item first to already existing slots of the same type. If all existing slots are full it creates new slots. 
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
    /// Adds this item to the given position. The position must be empty or already containing this type of item. 
    /// </summary>
    /// <param name="amount"> The amount of new added items. </param>
    /// <param name="position"> The position to add the items. Use (-1, -1) if the position doesn't matter </param>
    /// <returns> Returns 0, if all items are stored in this slot. If it can't store all items it returns the remaining amount. </returns>
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

    /// <summary> Adds this item to the given slot. The slot must be in slots. </summary>
    /// <param name="amount"> The amount of new added items. </param>
    /// <param name="slot"> The slot which gets the item. </param>
    /// <returns> The amount that can't be stored in this slot. 0 if all items are stored in this position. </returns>
    private int AddExistingSlot(int amount, ItemSlot slot){
        int newAmount = slot.Add(amount);
        if (amount != newAmount){
            inventory.InventoryCanvas?.Amount(slot.Position, slot.Amount);
        }
        return newAmount;
    }

    /// <summary> Creates a new slot at the given position and adds the item to this slot. </summary>
    /// <param name="amount"> The amount of new added items. </param>
    /// <param name="position"> The position of the new slot. </param>
    private int AddNewSlot(int amount, Vector2Int position){
        if (!inventory.FreeSlot[position.x][position.y]){
            Debug.LogWarning("Slot is already used");
            return amount;
        }
        slots.Add(new ItemSlot(position, this));
        amount = slots[^1].Add(amount);
        inventory.InventoryCanvas?.AddSlot(slots[^1].Position, itemName, slots[^1].Amount, texture);
        inventory.FreeSlot[slots[^1].Position.x][slots[^1].Position.y] = false;
        return amount;
    }

    /// <summary>
    /// Removes if possible the given amount of this inventory. If it doesn't contain the amount nothing happens.
    /// </summary>
    /// <param name="amount"> The amount to remove from this item </param>
    /// <returns> True, if the inventory had more than or equal to the amount given in item.
    /// False, if someone tried to remove more items than existing
    /// </returns>
    public bool Remove(int amount){
        if (this.amount < amount){
            return false;
        }
        this.amount -= amount;
        for (int i = slots.Count - 1; i >= 0; i--){
            amount = slots[i].Remove(amount);
            if (amount == 0){
                inventory.InventoryCanvas?.RemoveSlot(slots[i].Position);
                inventory.FreeSlot[slots[i].Position.x][slots[i].Position.y] = true;
                slots.RemoveAt(i);
                break;
            } else if (amount == -1){
                inventory.InventoryCanvas?.Amount(slots[i].Position, slots[i].Amount);
                break;
            } else {
                inventory.InventoryCanvas?.RemoveSlot(slots[i].Position);
                inventory.FreeSlot[slots[i].Position.x][slots[i].Position.y] = true;
                slots.RemoveAt(i);
            }
        }
        return true;
    }

    /// <summary> Removes if possible the given amount at this position. If it doesn't contain the amount nothing happens. </summary>
    /// <param name="amount"> The amount to remove from this item </param>
    /// <param name="position"> The position where to remove the item </param>
    /// <returns> True, if the slot had more than or equal to the amount given in item.
    /// False, if someone tried to remove more items than existing
    /// </returns>
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
        } else {
            inventory.InventoryCanvas?.Amount(slot.Position, slot.Amount);
        }
        return true;
    }

    /// <summary>
    /// Removes the stack on this position from the players inventory.
    /// </summary>
    /// <param name"position"> The position of the stack before removing. </param>
    public void RemoveStack(Vector2Int position){
        Vector3 throwPosition = new(0, 1, 0.5f);
        ItemSlot stack = GetItemSlot(position);
        prefab.GetComponent<Item>().Amount = stack.Amount;
        GameObject obj = GameObject.Instantiate(prefab, inventory.transform);
        obj.transform.localPosition = throwPosition;
        obj.transform.parent = null;
        Remove(stack.Amount, position);
    }

    /// <summary>
    /// Moves this item to a new position. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack. 
    /// If this stack isn't big enough the remaining part stays in the old slot.
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
            newType = new(itemName, maxStackSize, texture, prefab, inventory);
            inventory.Type.Add(newType);
        }
        slot.Move(newPosition, newType);
        slots.Remove(slot);
        newType.slots.Add(slot);
        amount -= slot.Amount;
        newType.amount += slot.Amount;
    }

    /// <summary> Returns true, if an item of this type is stored at the position </summary>
    /// <param name="position"> The position to check </param>
    /// <returns> True, if the position exists here, otherwise false. </returns>
    public bool Contains(Vector2Int position){
        if (GetItemSlot(position) == null) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary> Returns the ItemSlot at the given position </summary>
    /// <param name="position"> The position to find </param>
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