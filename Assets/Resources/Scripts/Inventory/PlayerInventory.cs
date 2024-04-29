using System;
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
    public bool printStateInventory;

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
        printStateInventory = false;
    }

    /// <summary> Adds the given item to the inventory. </summary>
    /// <param name="item"> The item to store in the inventory. </param>
    /// <returns> Returns the amount of this item, that can't be stored in this inventory. </returns>
    public int Add(Item item){
        foreach (InventoryItemHelper inventoryItem in inventoryItems){
            if (inventoryItem.ItemName == item.GetType().Name){
                inventoryChanged = true;
                return inventoryItem.Add(item);
            }
        }
        inventoryItems.Add(new InventoryItemHelper(item, this));
        return inventoryItems[^1].Add(item);
    }

    /// <summary> 
    /// Removes the item the given amount of times. If the inventory hasn't the necessary amount nothing gets removed.
    /// </summary>
    /// <param name="item"> The name and the amount of the removed item stored in an item. </param>
    /// <returns> True, if it removed the item the given amount of times. False, if the inventory hadn't enough items stored. </returns>
    public bool Remove(Item item){
        return Remove(item.GetType().Name, item.Amount);
    }

    /// <summary> 
    /// Removes the item the given amount of times. If the inventory hasn't the necessary amount nothing gets removed.
    /// </summary>
    /// <param name="itemName"> The name of the removed item. </param>
    /// <param name="amount"> The amount of the removed item. </param>
    /// <returns> True, if it removed the item the given amount of times. False, if the inventory hadn't enough items stored. </returns>
    public bool Remove(string itemName, int amount){
        for (int i = 0; i < inventoryItems.Count; i++) {
            if (inventoryItems[i].ItemName == itemName){
                inventoryChanged = true;
                bool isRemoved = inventoryItems[i].Remove(amount);
                if (inventoryItems[i].Slots.Count == 0){
                    inventoryItems.RemoveAt(i);
                }
                return isRemoved;
            }
        }
        return false;
    }

    /// <summary> 
    /// Removes the whole stack of this item. 
    /// </summary>
    /// <param name="itemName"> The name of the removed item. </param>
    /// <param name="column"> The column of the items position in the inventory </param>
    /// <param name="row"> The row of the items position in the inventory </param>
    /// <returns> True, if the item existed on this position. False, if the item didn't exist there. </returns>
    public bool RemoveStack(string itemName, int column, int row){
        foreach (InventoryItemHelper inventoryItem in inventoryItems){
            if (inventoryItem.ItemName == itemName){
                inventoryChanged = true;
                return inventoryItem.RemoveStack(column, row);
            }
        }
        return false;
    }

    /// <summary>
    /// Moves this item to a new position in the inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="itemName"> The name of the moved item. </param>
    /// <param name="oldColumn"> The column, where the item was stored in the inventory. </param>
    /// <param name="oldRow"> The row, where the item was stored in the inventory. </param>
    /// <param name="newColumn"> The column, where the item is now stored in the inventory. </param>
    /// <param name="newRow"> The row, where the item is now stored in the inventory. </param>
    public void Move(string itemName, int oldColumn, int oldRow, int newColumn, int newRow){
        // TODO
    }

    /// <summary>
    /// Splits a stack. One part stays in this item slot and the other part is moved to another slot. 
    /// The other slot must be empty.
    /// <summary>
    /// <param name="itemName"> The name of the moved item. </param>
    /// <param name="oldColumn"> The column, where the item was stored in the inventory. </param>
    /// <param name="oldRow"> The row, where the item was stored in the inventory. </param>
    /// <param name="newColumn"> The column, where a part of this item is now stored in the inventory. </param>
    /// <param name="newRow"> The row, where a part of this item is now stored in the inventory. </param>
    /// <returns> 
    /// True, if the new slot was empty. False, if the new slot wasn't empty. If false returns nothing happened in the inventory.
    /// </returns>
    public bool Split(string itemName, int oldColumn, int oldRow, int newColumn, int newRow, int amount){
        // TODO
        return false;
    }

    /// <summary> Returns true, if this inventory has at least one free slot. </summary>
    /// <returns> True, if at least one slot is free. 
    /// False, if all slots have an item.
    /// </returns>
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

    /// <summary> Returns the next free slot in the inventory. It goes from the top left frist to the right and then down until it finds the first free spot. </summary>
    /// <returns> Tuple of (column, row). The position of the free slot. </returns>
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

    void Update(){
        if (printStateInventory){
            string message = SaveInventory();
            Debug.Log(message);
            printStateInventory = false;
        }
    }

    /// <summary> Returns the inventory in json format. </summary>
    /// <param name="readable"> If true, it's in multiple lines and with tabs in the beginning. If false, it's one line without spaces </param>
    public string ToJson(bool readable){
        string json = "{";
        json += string.Format("\"columns\":{0},\"rows\":{1},\"freeSlot\":[", columns, rows);
        for (int i = 0; i < freeSlot.Count; i++) {
            json += "[";
            for (int j = 0; j < freeSlot[i].Count; j++) {
                json += freeSlot[i][j].ToString().ToLower();
                if (j < freeSlot[i].Count - 1){
                    json += ",";
                }
            }
            if (i < FreeSlot.Count - 1){
                json += "],";
            }
        }
        json += "]],\"inventoryItems\":[";
        for (int i = 0; i < inventoryItems.Count; i++){
            json += inventoryItems[i].ToJson();
            if (i < inventoryItems.Count - 1){
                json += ",";
            }
        }
        json += "]}";
        if (readable){
            return MakeReadable(json);
        } else {
            return json;
        }
    }

    /// <summary> Formats a json string in an easier readable text </summary>
    /// <param name="json"> String to format </param>
    /// <returns> Formated string </returns>
    public string MakeReadable(string json){
        int tab = 0;
        char[] signs = {'{', '}', '[', ']', ','};
        int[] next = new int[signs.Length];
        for (int i = 0; i < signs.Length; i++){
            next[i] = json.IndexOf(signs[i]);
        }
        int cancel = 500;
        while (true){
            (int min, int idx) = Min(next);
            switch (idx) {
                case 0: tab++; break;
                case 1: tab--; break;
                case 2: tab++; break;
                case 3: tab--; break;
                case 4: break;
                default: break;
            }
            min++;
            if (min >= json.Length){
                break;
            }
            json = json.Insert(min, InsertJson(tab));
            for (int i = 0; i < signs.Length; i++){
                if (min < json.Length - 1){
                    next[i] = json.IndexOf(signs[i], min);
                } else {
                    next[i] = -1;
                }
            }
            cancel--;
            if (cancel == 0){
                Debug.LogWarning("Loop canceled");
                break;
            }
        }
        return json;
    }

    /// <summary> Returns the lowest value greater than or equal to zero and it's first index </summary>
    /// <param name="ints"> Values to compare </param>
    /// <returns> A tuple of (value, index). If all values are less than zero the value is max int and the index -1. </returns>
    private (int, int) Min(int[] ints){
        int max = 2147483647;
        for (int i = 0; i < ints.Length; i++){
            if (ints[i] < 0){
                ints[i] = max;
            }
        }
        int min = ints[0];
        int idx = 0;
        for (int i = 1; i < ints.Length; i++){
            if (ints[i] < min){
                min = ints[i];
                idx = i;
            }
        }
        if (min == max){
            idx = -1;
        }
        return (min, idx);
    }

    private string InsertJson(int tabs){
        string json = "\n";
        for (int i = 0; i < tabs; i++){
            json += "\t";
        }
        return json;
    }

    public string SaveInventory(){
        return ToJson(true);
    }

    public int Rows { get => rows; set => rows = value; }
    public int Columns { get => columns; set => columns = value; }
    public List<List<bool>> FreeSlot {get => freeSlot; }
    public List<InventoryItemHelper> InventoryItems { get => inventoryItems; }
    public bool InventoryChanged { get => inventoryChanged; set => inventoryChanged = value; }
    public CanvasInventory Canvas { get => canvas; }
}
