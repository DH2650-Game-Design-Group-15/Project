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

    void Update(){
        if (printStateInventory){
            string message = SaveInventory();
            Debug.Log(message);
            printStateInventory = false;
        }
    }

    public string ToJson(){
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
        return MakeReadable(json);
    }

    public string MakeReadable(string json){
        int tab = 0;
        char[] signs = {'{', '}', '[', ']', ','};
        int[] next = new int[signs.Length];
        for (int i = 0; i < signs.Length; i++){
            next[i] = json.IndexOf(signs[i]);
        }
        while (string.Join("", next) != "-1-1-1-1"){
            (int min, int idx) = Min(next);
            switch (idx) {
                case 0: break;
                case 1: tab++; break;
                case 2: tab--; break;
                case 3: tab++; break;
                case 4: tab--; break;
                default: break;
            }
            min++;
            json = json.Insert(min, InsertJson(tab));
            for (int i = 0; i < signs.Length; i++){
                next[i] = json.IndexOf(signs[i]);
            }
        }
        return json;
    }

    private (int, int) Min(int[] ints){
        int max = ((int)Math.Pow(2.0, 32.0)) - 1;
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
        return ToJson();
    }

    public int Rows { get => rows; set => rows = value; }
    public int Columns { get => columns; set => columns = value; }
    public List<List<bool>> FreeSlot {get => freeSlot; }
    public List<InventoryItemHelper> InventoryItems { get => inventoryItems; }
    public bool InventoryChanged { get => inventoryChanged; set => inventoryChanged = value; }
    public CanvasInventory Canvas { get => canvas; }
}
