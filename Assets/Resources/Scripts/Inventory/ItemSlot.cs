using UnityEngine;

/// <summary> Stores all information about an item on a special slot in the inventory </summary>
public class ItemSlot{
    private Vector2Int position;
    private int amount;
    private ItemType itemType;

    /// <summary> Creates an item Slot. The amount is 0 at the beginning but it blocks already a slot in the inventory. </summary>
    /// <param name="position"> Position to store the item in the UI. x is the row, y is the column. </param>
    /// <param name="itemType"> A group for all items of this type in this inventory. </param>
    public ItemSlot(Vector2Int position, ItemType itemType){
        this.position = position;
        this.itemType = itemType;
        amount = 0;
        itemType.Inventory.FreeSlot[position.x][position.y] = false;
    }

    /// <summary> Adds the given amount of items to this slot. If the new amount would be greater than the allowed stack size it 
    /// sets the amount on this value and returns the difference. </summary>
    /// <param name="amount"> The amount of items to be added to this slot </param>
    /// <returns> The amount of items which can't be stored in this slot. Returns 0, if all items are stored here. </returns>
    public int Add(int amount){
        this.amount += amount;
        int overflow = Amount - itemType.MaxStackSize;
        if (overflow > 0){
            this.amount = itemType.MaxStackSize;
            return overflow;
        } else {
            return 0;
        }
    }

    /// <summary> Removes the given amount of items to this slot. Returns the amount of items which aren't removed here. </summary>
    /// <param name="amount"> The amount of items to be removed from this slot </param>
    /// <returns> The amount of items which can't be removed from this slot. Returns -1, if all items are removed and there're still items in this slot left. 
    /// Returns 0, if all items are removed but the new amount of this slot is 0. </returns>
    public int Remove(int amount){
        if (Amount > amount){
            this.amount = Amount - amount;
            return -1;
        } else {
            this.amount = 0;
            return amount - Amount;
        }
    }

    /// <summary> Moves this item to a new position. Doesn't check, if the new position is empty or not. </summary>
    /// <param name="position"> The new position in the inventory. </param>
    public void Move(Vector2Int position){
        this.position = position;
    }

    public void Move(Vector2Int position, ItemType type){
        this.position = position;
        itemType = type;
    }

    public Vector2Int Position { get => position; }
    public int Amount { get => amount; }
    public ItemType Type { get => itemType; }
}