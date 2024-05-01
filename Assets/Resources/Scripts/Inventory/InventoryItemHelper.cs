using System.Collections.Generic;
using UnityEngine;

public class InventoryItemHelper {
    private int amount;
    private readonly string itemName;
    private readonly List<ItemSlot> slots;
    private readonly PlayerInventory inventory;

    /// <summary> 
    /// Creates the helper to save all slots for this item. It doesn't store the item in this object. 
    /// </summary>
    public InventoryItemHelper(Item item, PlayerInventory inventory){
        slots = new();
        itemName = item.GetType().Name;
        amount = 0;
        this.inventory = inventory;
    }

    /// <summary>
    /// Adds the item first to already existing slots of the same type. 
    /// If all existing slots are full it creates a new one if the inventory hasn't reached the maximum amount of slots.
    /// </summary>
    /// <param name="item"> The item with the amount, how many of it should be stored. </param>
    /// <returns> Returns 0, if all items are stored in the inventory. \n
    /// If it can't store all items because the inventory has no free slot it returns the amount, that can't be stored in a slot.
    /// </returns>
    public int Add(Item item){
        amount += item.Amount;
        int amountLeft = item.Amount;
        foreach (ItemSlot slot in slots) {
            amountLeft = slot.Add(amountLeft);
            if (amountLeft <= 0){
                break;
            }
        }
        if (amountLeft > 0 && inventory.HasFreeSlots()){
            item.Amount = amountLeft;
            slots.Add(new ItemSlot(inventory.Canvas, item, inventory));
            return 0;
        } else {
            amount -= amountLeft;
            return amountLeft;
        }
    }

    /// <summary>
    /// Removes if possible the given amount of item from the inventory. 
    /// </summary>
    /// <param name="amount"> The amount to remove from this item </param>
    /// <returns> True, if the inventory had more than or equal to the amount given in item.
    /// False, if someone tried to remove more items than existing
    /// </summary>
    public bool Remove(int amount){
        if (amount > this.amount){
            return false;
        }
        int amountLeft = amount;
        for (int i = slots.Count - 1; i >= 0; i--) {
            amountLeft = slots[i].Remove(amountLeft);
            if (amountLeft > 0){
                slots.RemoveAt(i);
            } else if (amountLeft == 0){
                slots.RemoveAt(i);
                break;
            } else {
                break;
            }
        }
        if (amountLeft > 0){
            Debug.LogWarning("Player had less items left than were removed");
        }
        return true;
    }

    /// <summary>
    /// Removes the stack on this position from the players inventory. \n
    /// Removes it only, if it has the same type than this helpers items.
    /// </summary>
    /// <returns> True, if the slot was used by this slot. False if it didn't exist in this slot </returns>
    public bool RemoveStack(Vector2Int position){
        (ItemSlot slot, int index) = GetByPosition(position);
        if (slot != null){
            amount -= slot.Item.Amount;
            slot.RemoveStack();
            slots.RemoveAt(index);
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Moves this item to a new position in the inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="oldPosition"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    public void Move(Vector2Int oldPosition, Vector2Int newPosition) {
        (ItemSlot slot, _) = GetByPosition(oldPosition);
        slot?.Move(newPosition);
    }

    /// <summary>
    /// Splits a stack. One part stays in this item slot and the other part is moved to another slot. 
    /// The other slot must be empty.
    /// <summary>
    /// <param name="oldPosition"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where a part of this item is now stored in the inventory. </param>
    /// <param name="amount"> The amount of items stored in the new slot. Must be between 1 and the old amount. </param>
    public void Split(Vector2Int oldPosition, Vector2Int newPosition, int amount){
        // TODO
    }

    public (ItemSlot, int) GetByPosition(Vector2Int position){
        for (int i = slots.Count - 1; i >= 0; i--){
            if (slots[i].Position.Equals(position)){
                return (slots[i], i);
            }
        }
        return (null, -1);
    }

    /// <summary> Returns true, if a slot of this item contains the position. </summary>
    /// <param name="position"> The position in the inventory </param>
    /// <returns> True, if this postion contains at least one element of this item. False otherwise. </returns>
    public bool ContainsPosition(Vector2Int position){
        if (GetByPosition(position).Item1 == null){
            return false;
        } else {
            return true;
        }
    }


    /// <summary> Returns this item with all its slots as an json string. </summary>
    /// <returns> The item as a json string. </summary>
    public string ToJson(){
        string json = "{";
        json += string.Format("\"itemName\":\"{0}\",\"amount\":{1}", itemName, amount);
        json += ",\"slots\":[";
        for (int i = 0; i < slots.Count; i++) {
            json += slots[i].ToJson();
            if (i != slots.Count - 1){
                json += ",";
            }
        }
        json += "]}";
        return json;
    }

    public int Amount { get => amount; }
    public string ItemName { get => itemName; }
    public List<ItemSlot> Slots { get => slots; }
}
