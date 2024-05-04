using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves how many items can be stored in an inventory, if a slot is used and which items are stored there. 
/// </summary>
[Serializable]
public class Inventory : MonoBehaviour{ // Later abstract, change Start for each type of inventory because of inventorySize
    private GameObject canvasPrefab;
    [SerializeField] private List<List<bool>> freeSlot;
    [SerializeField] private List<ItemType> type;
    [SerializeField] private Vector2Int inventorySize;
    private InventoryCanvas inventoryCanvas;
    // Debug
    public bool printInventory;

    void Awake(){
        GameObject canvasPrefab = Resources.Load<GameObject>("Prefabs/UI/Inventory/Inventory");
        GameObject canvas = GameObject.FindWithTag("Canvas");
        if (canvas == null){
            Debug.LogError("No canvas to build the UI");
        }
        Transform transform = canvas.transform.Find(gameObject.name + "Inventory");
        if (inventoryCanvas == null){
            inventoryCanvas = Instantiate(canvasPrefab, canvas.transform).GetComponentInChildren<InventoryCanvas>();
            inventoryCanvas.transform.parent.name = gameObject.name + "Inventory";
            inventoryCanvas.Inventory = this;
        } else {
            inventoryCanvas = transform.GetComponentInChildren<InventoryCanvas>();
        }
        inventorySize = new Vector2Int(3, 5);
        type = new();
        freeSlot = new();
        for (int x = 0; x < inventorySize.x; x++){
            freeSlot.Add(new List<bool>());
            for (int y = 0; y < inventorySize.y; y++){
                freeSlot[^1].Add(true);
            }
        }
    }


    // Called to perform an action
    /// <summary> Adds the given item to the inventory. </summary>
    /// <param name="itemName"> The item to store in the inventory. </param>
    /// <param name="item"> Contains the allowed stack size and the texture if a new slot is needed. </param>
    /// <param name="amount"> The amount to be added to the inventory </param>
    /// <returns> Returns the amount of this item, that can't be stored in this inventory. </returns>
    public int Add(string itemName, Item item, int amount){
        ItemType itemType = GetItemType(itemName);
        if (itemType != null){
            return itemType.Add(amount);
        } else {
            type.Add(new ItemType(itemName, item.MaxStackSize, item.ImageInventory, this));
            return type[^1].Add(amount);
        }
    }

    /// <summary> Adds the given item to the inventory. Must be stored on given position </summary>
    /// <param name="itemName"> The item to store in the inventory. </param>
    /// <param name="item"> Contains the allowed stack size and the texture if a new slot is needed. </param>
    /// <param name="amount"> The amount to be added to the inventory </param>
    /// <param name="position"> The position, where the item is stored. The position must be empty or already containing some of this type. </param>
    /// <returns> Returns the amount of this item, that can't be stored on this position. </returns>
    public int Add(string itemName, int amount, Item item, Vector2Int position){
        ItemType itemType = GetItemType(itemName);
        if (itemType != null){
            return itemType.Add(amount, position);
        } else {
            type.Add(new ItemType(itemName, item.MaxStackSize, item.ImageInventory, this));
            return type[^1].Add(amount, position);
        }
    }

    /// <summary> Removes the given item from the inventory. If there aren't enough items stored before nothing gets removed. </summary>
    /// <param name="itemName"> The item to be removed from the inventory. </param>
    /// <param name="amount"> The amount to be removed from the inventory </param>
    /// <returns> Returns true, if the amount has been removed. Returns false, if the inventory hadn't enough items stored. </returns>
    public bool Remove(string itemName, int amount){
        ItemType item = GetItemType(itemName);
        if (item != null){
            return item.Remove(amount);
        } else {
            return false;
        }
    }

    /// <summary> 
    /// Removes the whole stack from this position. 
    /// </summary>
    /// <param name="position"> The position of the item in the inventory </param>
    /// <returns> True, if an item existed on this position. False, if no item existed before. </returns>
    public bool RemoveStack(Vector2Int position){
        ItemType item = GetItemType(position);
        if (item != null){
            item.RemoveStack(position);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Moves this item to a new position in the inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="oldPostion"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    /// <remarks> Right now it doesn't add the same type. Instead both are swapping like different items. </remarks>
    public void Move(Vector2Int oldPosition, Vector2Int newPosition){
        ItemType item = GetItemType(oldPosition);
        ItemType otherItem = GetItemType(newPosition);
        if (otherItem != null){
            otherItem.Move(newPosition, oldPosition);
        } else {
            FreeSlot[oldPosition.x][oldPosition.y] = true;
            FreeSlot[newPosition.x][newPosition.y] = false;
        }
        if (item != null){
            item.Move(oldPosition, newPosition);
        } else {
            FreeSlot[oldPosition.x][oldPosition.y] = false;
            FreeSlot[newPosition.x][newPosition.y] = true;
        }
        inventoryCanvas.MoveSlot(oldPosition, newPosition);
    }

    /// <summary>
    /// Moves this item to a new position in the same or another inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="oldPostion"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    /// <param name="inventory"> The inventory of the new position. </param>
    /// <remarks> Right now it doesn't add the same type. Instead both are swapping like different items. </remarks>
    public void Move(Vector2Int oldPosition, Vector2Int newPosition, Inventory inventory){

    }

    public void Split(Vector2Int oldPosition, Vector2Int newPosition){

    }

    public void Split(Vector2Int oldPosition, Vector2Int newPosition, Inventory inventory){

    }
    
    public void ChangeSize(Vector2Int size){

    }

    // Getters
    /// <summary> Returns the ItemType for this object. </summary>
    /// <param name="itemName"> The name of this item to identify it </param>
    public ItemType GetItemType(string itemName){
        foreach (ItemType itemType in type){
            if (itemType.ItemName == itemName){
                return itemType;
            }
        }
        return null;
    }

    /// <summary> Returns the ItemType for this object. </summary>
    /// <param name="position"> The position of this item to identify it. </param>
    /// <returns> Returns the ItemType of the item, stored on this position. </returns>
    public ItemType GetItemType(Vector2Int position){
        foreach (ItemType itemType in type){
            if (itemType.Contains(position)){
                return itemType;
            }
        }
        return null;
    }

    /// <summary> Returns the next free slot in the inventory. It goes first down and then to the right. </summary>
    /// <returns> Returns the position as Vector2Int </returns>
    public Vector2Int NextFreeSlot() {
        for (int x = 0; x < inventorySize.x; x++){
            for (int y = 0; y < inventorySize.y; y++){
                if (freeSlot[x][y]){
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    // Debug + save game state
    void Update(){
        if (printInventory){
            printInventory = false;
            Debug.Log(PrintJsonInventory());
        }
    }

    /// <summary> Creates a json of the whole inventory </summary>
    /// <returns> The json inventory as a string </returns>
    public string PrintJsonInventory(){
        //return JsonUtility.ToJson(this);
        List<string> names = new(){"type", "inventorySize", "amount", "maxStackSize", "itemName", "slots", "position"};
        return JsonRecursive.ToJson(this, names, 5, true);
    }

    public List<List<bool>> FreeSlot { get => freeSlot; set => freeSlot = value; }
    public List<ItemType> Type { get => type; set => type = value; }
    public Vector2Int InventorySize { get => inventorySize; set => inventorySize = value; }
    public InventoryCanvas InventoryCanvas { get => inventoryCanvas; set => inventoryCanvas = value; }

}
