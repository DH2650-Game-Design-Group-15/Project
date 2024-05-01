using UnityEngine;


/// <summary>
/// Stores the amount of items in this item slot and on which field in the inventory it's displayed.
/// </summary>
public class ItemSlot{
    private readonly Item item;
    private Vector2Int position;
    private bool full;
    private readonly CanvasInventory canvas;
    private readonly PlayerInventory inventory;

    /// <summary>
    /// Calls the constructor with the next free slot in the players inventory
    /// </summary>
    /// <param name="item"> Item stored in this slot </param>
    /// <param name="inventory"> Whos inventory this item is. Searchs there for the next free slot </param>
    public ItemSlot(CanvasInventory canvas, Item item, PlayerInventory inventory): this(canvas, item, inventory, inventory.NextFreeSlot()){}
    /// <summary>
    /// Calls the constructor with a chosen slot
    /// Doesn't check if the slot is already used somewhere else
    /// </summary>
    /// <param name="item"> Item stored in this slot </param>
    /// <param name="position"> Position to store the item. Both values must be between 0 and position in inventory </param>
    public ItemSlot(CanvasInventory canvas, Item item, PlayerInventory inventory, Vector2Int position){
        this.item = item;
        this.full = item.SpaceAvailable() == 0;
        this.canvas = canvas;
        canvas.EnableSlot(this.item, position);
        this.inventory = inventory;
        inventory.FreeSlot[position.x][position.y] = false;
    }

    /// <summary>
    /// Adds the given amount to the item. If the amount is greater than the available space it adds only as much as possible.
    /// </summary>
    /// <param name="amount"> How many entities more it should store in this slot. </param>
    /// <returns> How many entites can't be stored in this slot because it reached the maximum amount.\n
    /// Returns 0, if it stored all entities it should.
    /// </returns>
    public int Add(int amount) {
        int space = Item.SpaceAvailable();
        if (space <= amount){
            Item.Amount = Item.MaxStackSize;
            full = true;
            return amount - space;
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
        full = false;
        if (Item.Amount < amount){
            canvas.DisableSlot(position);
            inventory.FreeSlot[position.x][position.y] = true;
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
        canvas.DisableSlot(position);
        inventory.FreeSlot[position.x][position.y] = true;
    }

    /// <summary>
    /// Changes the slot
    /// </summary>
    public void Move(Vector2Int newPosition){
        inventory.FreeSlot[position.x][position.y] = true;
        canvas.DisableSlot(position);
        canvas.EnableSlot(item, newPosition);
        inventory.FreeSlot[newPosition.x][newPosition.y] = false;
    }

    /// <summary> Returns this slot as an json string. </summary>
    /// <returns> The item as a json string. </summary>
    public string ToJson(){
        string json = "{";
        json += string.Format("\"Amount\":{0},\"column\":{1},\"row\":{2}", item.Amount, position.x, position.y);
        json += "}";
        return json;
        
    }

    public Item Item { get => item; }
    public Vector2Int Position { get => position; }
    public bool Full { get => full; }
}