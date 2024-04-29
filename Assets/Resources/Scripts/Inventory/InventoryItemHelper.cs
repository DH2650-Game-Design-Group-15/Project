using System.Collections.Generic;
using UnityEngine;

public class InventoryItemHelper {
    private int amount;
    private readonly string itemName;
    private readonly List<ItemSlot> slots;
    private readonly PlayerInventory inventory;

    public InventoryItemHelper(Item item, PlayerInventory inventory){
        slots = new();
        itemName = item.GetType().Name;
        amount = item.Amount;
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
            if (amountLeft == 0){
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
    public bool RemoveStack(int column, int row){
        for (int i = slots.Count - 1; i >= 0; i--){
            if (slots[i].Column == column && slots[i].Row == row){
                slots[i].RemoveStack();
                slots.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary> Moves the slot on position to the new position </summary>
    public void Move(int oldColumn, int oldRow, int newColumn, int newRow) {
        // TODO
    }

    /// <summary> Splits the amount on position to the new position. Everything else stays on the old position. </summary>
    public void Split(int oldColumn, int oldRow, int newColumn, int newRow, int amount){
        // TODO
    }

    /// <summary> Returns true, if a slot of this item contains the position. </summary>
    public bool ContainsPosition(int column, int row){
        for (int i = slots.Count - 1; i >= 0; i--){
            if (slots[i].Column == column && slots[i].Row == row){
                return true;
            }
        }
        return false;
    }


    public int Amount { get => amount; }
    public string ItemName { get => itemName; }
    public List<ItemSlot> Slots { get => slots; }
}
