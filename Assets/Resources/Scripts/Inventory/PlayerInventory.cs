using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves, how many items a player can store in his inventory. It saves also a list of all items a user stored by item.
public class PlayerInventory : MonoBehaviour {
    private List<InventoryItemHelper> items;
    private int rows;
    private int column;
    private List<List<bool>> freeSlot; 
    private int usedSlots;
    private bool inventoryChanged;

    void Start(){
        items = new List<InventoryItemHelper>();
        freeSlot = new List<List<bool>>();
        Rows = 5;
        Column = 5;
        usedSlots = 0;
        InventoryChanged = true;
        for (int i = 0; i < Rows; i++){
            freeSlot.Add(new List<bool>());
            for (int j = 0; j < Column; j++){
                freeSlot[i].Add(true);
            }
        }
    }

    /// <summary>
    /// Adds this item the given amount of times to the players inventory
    /// </summary>
    /// <return>
    /// returns, how many Items can't be added to the players inventory because the inventory is full.
    /// returns 0, if all items are added to the inventory.
    /// </return>
    public int Add(GameObject item) {
        InventoryChanged = true;
        Item itemComponent = item.GetComponent<Item>();
        foreach (InventoryItemHelper helper in items) {
            if (helper.ItemName == itemComponent.GetType().Name) {
                return helper.Add(itemComponent.Amount, itemComponent);
            }
        }
        items.Add(new InventoryItemHelper(itemComponent, this));
        return items[^1].Add(itemComponent.Amount, itemComponent);
    }

    public bool HasFreeSlots() {
        return usedSlots < rows * column;
    }

    public List<InventoryItemHelper> Items { get => items; }
    public int Rows { get => rows; set {
        rows = value;
        InventoryChanged = true;
    }}
    public int Column { get => column; set { 
        column = value; 
        InventoryChanged = true;
    }}
    public List<List<bool>> FreeSlot { get => freeSlot; }
    public int UsedSlots { get => usedSlots; set => usedSlots = value; }
    public bool InventoryChanged { get => inventoryChanged; set => inventoryChanged = value; }
}
