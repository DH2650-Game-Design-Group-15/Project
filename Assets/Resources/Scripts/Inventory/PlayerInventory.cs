using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves, how many items a player can store in his inventory. It saves also a list of all items a user stored by item.
/// </summary>
public class PlayerInventory : MonoBehaviour {
    private int rows;
    private int columns;
    private List<List<bool>> freeSlot;
    private List<InventoryItemHelper> inventoryItems;
    private bool inventoryChanged;
    public CanvasInventory canvas;

    void Start(){
        rows = 5;
        columns = 5;
        inventoryItems = new();
        freeSlot = new();
        for (int i = 0; i < columns; i++){
            freeSlot.Add(new List<bool>());
            freeSlot[i] = new();
            for (int j = 0; j < rows; j++){
                freeSlot[i].Add(true);
            }
        }
        inventoryChanged = true;
    }

    public int Add(Item item){
        foreach (InventoryItemHelper inventoryItem in inventoryItems){
            if (inventoryItem.ItemName == item.GetType().Name){
                inventoryChanged = true;
                return inventoryItem.Add(item);
            }
        }
        inventoryItems.Add(new InventoryItemHelper(item, this));
        return inventoryItems[inventoryItems.Count - 1].Add(item);
    }

    public bool Remove(Item item){
        return Remove(item.GetType().Name, item.Amount);
    }

    public bool Remove(string itemName, int amount){
        foreach (InventoryItemHelper inventoryItem in inventoryItems){
            if (inventoryItem.ItemName == itemName){
                inventoryChanged = true;
                return inventoryItem.Remove(amount);
            }
        }
        return false;
    }

    public bool RemoveStack(string itemName, int column, int row){
        foreach (InventoryItemHelper inventoryItem in inventoryItems){
            if (inventoryItem.ItemName == itemName){
                inventoryChanged = true;
                return inventoryItem.RemoveStack(column, row);
            }
        }
        return false;
    }

    public void Move(string itemName, int oldColumn, int oldRow, int newColumn, int newRow){
        // TODO
    }

    public bool Split(string itemName, int oldColumn, int oldRow, int newColumn, int newRow, int amount){
        // TODO
        return false;
    }

    public bool HasFreeSlots(){
        foreach (List<bool> row in freeSlot){
            foreach(bool slot in row){
                if (slot){
                    return true;
                }
            }
        }
        return false;
    }

    public (int, int) NextFreeSlot(){
        for (int column = 0; column < columns; column++){
            for (int row = 0; row < rows; row++){
                if (freeSlot[column][row]){
                    return (column, row);
                }
            }
        }
        return (-1, -1);
    }

    public int Rows { get => rows; set => rows = value; }
    public int Columns { get => columns; set => columns = value; }
    public List<List<bool>> FreeSlot {get => freeSlot; }
    public List<InventoryItemHelper> InventoryItems { get => inventoryItems; }
    public bool InventoryChanged { get => inventoryChanged; set => inventoryChanged = value; }
    public CanvasInventory Canvas { get => canvas; }
}
