using UnityEngine;


/// <summary>
/// Stores the amount of items in this item slot and on which field in the inventory it's displayed.
/// </summary>
public class ItemSlot{
    private readonly Item item;
    private int column;
    private int row;
    private bool full;
    private readonly CanvasInventory canvas;

    /// <summary>
    /// Calls the constructor with the next free slot in the players inventory
    /// </summary>
    /// <param name="item"> Item stored in this slot </param>
    /// <param name="inventory"> Whos inventory this item is. Searchs there for the next free slot </param>
    public ItemSlot(CanvasInventory canvas, Item item, PlayerInventory inventory): this(canvas, item, inventory.NextFreeSlot()){}
    private ItemSlot(CanvasInventory canvas, Item item, (int, int) slot):this(canvas, item, slot.Item1, slot.Item2){}
    /// <summary>
    /// Calls the constructor with a chosen slot
    /// Doesn't check if the slot is already used somewhere else
    /// </summary>
    /// <param name="item"> Item stored in this slot </param>
    /// <param name="column"> Column to store the item. Must be between 0 and player inventories column </param>
    /// <param name="row"> Row to store the item. Msut be between 0 and player inventories row </param>
    public ItemSlot(CanvasInventory canvas, Item item, int column, int row){
        this.item = item;
        this.column = column;
        this.row = row;
        this.full = item.SpaceAvailable() == 0;
        this.canvas = canvas;
        canvas.EnableSlot(item, column, row);
    }

    /// <summary>
    /// Adds the given amount to the item. If the amount is greater than the available space it adds only as much as possible.
    /// </summary>
    /// <param name="amount"> How many entities more it should store in this slot. </param>
    /// <returns> How many entites can't be stored in this slot because it reached the maximum amount.\n
    /// Returns 0, if it stored all entities it should.
    /// </returns>
    public int Add(int amount) {
        // TODO Canvas update
        if (Item.SpaceAvailable() <= amount){
            Item.Amount = Item.MaxStackSize;
            full = true;
            return amount - Item.SpaceAvailable();
        } else {
            Item.Amount += amount;
            return 0;
        }
    }

    /// <summary>
    /// Removes amount from the stored amount. 
    /// </summary>
    /// <param name="amount"> How many entities should be removed from this slot. </param>
    /// <returns> If it didn't remove all entities from this slot it returns how many entities aren't removed.\n
    /// If the given amount is removed and there're still entities left it returns -1.
    /// </returns>
    public int Remove(int amount){
        // TODO Canvas update
        full = false;
        if (Item.Amount < amount){
            canvas.DisableSlot(column, row);
            return amount - Item.Amount;
        } else {
            Item.Amount -= amount;
            return -1;
        }
    }

    /// <summary>
    /// Removes the whole stack from the players inventory
    /// </summary>
    public void RemoveStack(){
        canvas.DisableSlot(column, row);
    }

    /// <summary>
    /// Changes the slot
    /// </summary>
    public void Move(int column, int row){
        canvas.DisableSlot(this.column, this.row);
        canvas.EnableSlot(item, column, row);
        this.column = column;
        this.row = row;
    }

    public string ToJson(){
        string json = "{";
        json += string.Format("\"Amount\":{0},\"column\":{1},\"row\":{2}", item.Amount, column, row);
        json += "}";
        return json;
        
    }

    public Item Item { get => item; }
    public int Column { get => column; }
    public int Row { get => row; }
    public bool Full { get => full; }
}