using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Saves, how many items a player can store in his inventory. It saves also a list of all items a user stored by item.
/// </summary>
public class PlayerInventory : MonoBehaviour {
    private Vector2Int inventorySize;
    private List<List<bool>> freeSlot;
    private List<InventoryItemHelper> inventoryItems;
    private bool inventoryChanged;
    public CanvasInventory canvas;
    public bool printStateInventory;

    void Start(){
        inventorySize = new Vector2Int(6, 5);
        inventoryItems = new();
        freeSlot = new();
        for (int i = 0; i < inventorySize.x; i++){
            freeSlot.Add(new List<bool>());
            freeSlot[i] = new();
            for (int j = 0; j < inventorySize.y; j++){
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
        InventoryItemHelper inventoryItem = GetInventoryItemHelper(item.GetType().Name);
        if (inventoryItem != null){
            inventoryChanged = true;
            return inventoryItem.Add(item);
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
    /// <param name="position"> The position of the item in the inventory </param>
    /// <returns> True, if the item existed on this position. False, if the item didn't exist there. </returns>
    public bool RemoveStack(string itemName, Vector2Int position){
        InventoryItemHelper inventoryItem = GetInventoryItemHelper(itemName);
        if (inventoryItem == null){
            return false;
        }
        inventoryChanged = true;
        return inventoryItem.RemoveStack(position);        
    }

    /// <summary>
    /// Moves this item to a new position in the inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="itemName"> The name of the moved item. </param>
    /// <param name="oldPostion"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    public void Move(string itemName, Vector2Int oldPosition, Vector2Int newPosition){
        InventoryItemHelper itemHelper = GetInventoryItemHelper(itemName);
        itemHelper?.Move(oldPosition, newPosition);
    }

    /// <summary>
    /// Splits a stack. One part stays in this item slot and the other part is moved to another slot. 
    /// The other slot must be empty.
    /// <summary>
    /// <param name="itemName"> The name of the moved item. </param>
    /// <param name="oldPosition"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where a part of this item is now stored in the inventory. </param>
    /// <returns> 
    /// True, if the new slot was empty. False, if the new slot wasn't empty. If false returns nothing happened in the inventory.
    /// </returns>
    public bool Split(string itemName, Vector2Int oldPosition, Vector2Int newPosition, int amount){
        // TODO
        return false;
    }

    /// <summary> Returns true, if this inventory has at least one free slot. </summary>
    /// <returns> True, if at least one slot is free. 
    /// False, if all slots have an item.
    /// </returns>
    public bool HasFreeSlots(){
        if (NextFreeSlot().Equals(new Vector2Int(-1, -1))){
            return false;
        }
        return true;
    }

    /// <summary> Returns the next free slot in the inventory. It goes from the top left frist to the right and then down until it finds the first free spot. </summary>
    /// <returns> Tuple of (column, row). The position of the free slot. </returns>
    public Vector2Int NextFreeSlot(){
        for (int x = 0; x < inventorySize.x; x++){
            for (int y = 0; y < inventorySize.y; y++){
                if (FreeSlot[x][y]){
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public InventoryItemHelper GetInventoryItemHelper(string itemName){
        foreach (InventoryItemHelper inventoryItem in inventoryItems){
            if (inventoryItem.ItemName == itemName){
                return inventoryItem;
            }
        }
        return null;
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
        string size = "\"size\":{x:" + inventorySize.x + ",y:" + inventorySize.y + "}";
        string free = "\"freeSlot\":[";
        for (int i = 0; i < freeSlot.Count; i++) {
            free += "[";
            for (int j = 0; j < freeSlot[i].Count; j++) {
                free += freeSlot[i][j].ToString().ToLower();
                if (j < freeSlot[i].Count - 1){
                    free += ",";
                }
            }
            if (i < FreeSlot.Count - 1){
                free += "],";
            }
        }
        free += "]]";
        string items = "\"inventoryItems\":[";
        for (int i = 0; i < inventoryItems.Count; i++){
            items += inventoryItems[i].ToJson();
            if (i < inventoryItems.Count - 1){
                items += ",";
            }
        }
        items += "]";
        string json = "{" + size + "," + free + "," + items + "}";
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
        string readable = "";
        int index = -1;
        List<string> lines = new();
        List<char> separator = new();
        int cancel = 100;
        while (true){
            int newIndex = json.IndexOfAny(new char[]{'{', '}', '[', ']', ','}, index + 1);
            if (newIndex < 0 || newIndex >= json.Length){
                break;
            }
            if (separator.Count > 0){
                lines.Add(json.Substring(index + 1, newIndex - index - 1));
            }
            separator.Add(json[newIndex]);
            index = newIndex;
            cancel--;
            if (cancel < 0){
                return readable;
            }
        }
        lines.Add(json.Substring(index));
        int tab = 0;
        for (int i = 0; i < separator.Count; i++){
            int newTab;
            if (separator[i] == '{' || separator[i] == '['){
                newTab = tab + 1;
            } else if (separator[i] == '}' || separator[i] == ']'){
                newTab = tab - 1;
            } else {
                newTab = tab;
            }
            if (separator[i] != ',' && lines[i].Length != 0) {
                readable += "\n" + InsertJson(tab);
            }
            readable += separator[i];
            if (lines[i].Length != 0){
                readable += "\n" + InsertJson(newTab) + lines[i];
            }
            tab = newTab;
        }
        return readable;
    }

    private string InsertJson(int tabs){
        string json = "";
        for (int i = 0; i < tabs; i++){
            json += "\t";
        }
        return json;
    }

    public string SaveInventory(){
        return ToJson(true);
    }

    public Vector2Int InventorySize { get => inventorySize; }
    public List<List<bool>> FreeSlot { get => freeSlot; }
    public List<InventoryItemHelper> InventoryItems { get => inventoryItems; }
    public bool InventoryChanged { get => inventoryChanged; set => inventoryChanged = value; }
    public CanvasInventory Canvas { get => canvas; }
}
