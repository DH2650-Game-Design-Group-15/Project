using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Identifier for a storage (a player is also a storge). It stores all items in this inventory.
/// It stores them first grouped by item type and then for each slot the amount.
/// </summary>
public class Inventory : MonoBehaviour{ // Later abstract, change Start for each type of inventory because of inventorySize
    private GameObject uiPrefab; // prefab for the inventories UI
    public Transform inventories; // parent of all inventories
    public InventoryCanvas inventoryCanvas; // reference to the UI
    private List<List<bool>> freeSlot;
    private List<ItemType> type;
    private Vector2Int inventorySize;
    private bool isPlayer;
    // Debug
    public bool printInventory;

    /// <summary> Loads prefabs and initialise all variables. </summary>
    void Awake(){
        uiPrefab = Resources.Load<GameObject>("Prefabs/UI/Inventory/Inventory");
        inventories = Parent.FindChild(GameObject.FindWithTag("Canvas"), "Inventories");
        inventorySize = new Vector2Int(3, 5);
        type = new();
        InitFreeSlots();
        if (inventories == null){
            Debug.LogError("No GameObject inventories to build the UI");
        }
        Transform transform = inventories.Find(gameObject.name + "Inventory");
        if (inventoryCanvas == null){
            CreateInventoryUI();
        } else {
            inventoryCanvas = transform.GetComponentInChildren<InventoryCanvas>();
        }
        inventoryCanvas.CreateInventoryCanvas();
        isPlayer = GetComponentInChildren<InventoryInput>() != null;
    }

    /// <summary> Creates a matrix which shows that all slots in the inventory aren't used. </summary>
    private void InitFreeSlots(){
        freeSlot = new();
        for (int x = 0; x < inventorySize.x; x++){
            freeSlot.Add(new List<bool>());
            for (int y = 0; y < inventorySize.y; y++){
                freeSlot[^1].Add(true);
            }
        }
    }

    /// <summary> Creates an empty UI for this inventory and sets its owner to this inventory. </summary>
    private void CreateInventoryUI(){
        inventoryCanvas = Instantiate(uiPrefab, inventories).GetComponentInChildren<InventoryCanvas>();
        inventoryCanvas.name = gameObject.name + "Inventory";
        inventoryCanvas.Inventory = this;
    }

    /// <summary> Creates and updates the UI of this inventory to the actual state. 
    /// Sets it as last sibling, the last sibling is always on the right. </summary>
    public void ReloadInventoryCanvas(){
        CreateInventoryUI();
        inventoryCanvas.UpdateInventory();
        inventoryCanvas.transform.parent.SetAsLastSibling();
    }

    // Called to perform an action

    /// <summary> Adds the given item to the inventory. Adds it to the first slot that fits. </summary>
    /// <param name="itemName"> The item to store in the inventory. </param>
    /// <param name="item"> Contains the allowed stack size and the texture if it's the first item of this type in the inventory. </param>
    /// <param name="amount"> The amount to be added to the inventory </param>
    /// <returns> Returns the amount of this item, that can't be stored in this inventory. </returns>
    public int Add(string itemName, Item item, int amount){
        return Add(itemName, item, amount, new Vector2Int(-1, -1));
    }

    /// <summary> Adds the given item to the inventory. Must be stored on given position </summary>
    /// <param name="itemName"> The item to store in the inventory. </param>
    /// <param name="item"> Contains the allowed stack size and the texture it's the first item of this type in the inventory. </param>
    /// <param name="amount"> The amount to be added to the inventory </param>
    /// <param name="position"> The position, where the item is stored. 
    /// The position must be empty or already containing some of this type. 
    /// Use (-1, -1) if the next fitting slot should be used. </param>
    /// <returns> Returns the amount of this item, that can't be stored at this position. </returns>
    public int Add(string itemName, Item item, int amount, Vector2Int position){
        ItemType itemType = GetItemType(itemName);
        if (itemType != null){
            return itemType.Add(amount, position);
        } else {
            type.Add(new ItemType(item, this));
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
    /// Removes the whole stack from the given position. Throws the item on the ground in front of the player.
    /// </summary>
    /// <param name="position"> The position of the item in the inventory </param>
    /// <returns> True, if an item existed on this position. False, if no item existed before. </returns>
    public bool RemoveStack(Vector2Int position){
        ItemType item = GetItemType(position);
        item?.RemoveStack(position);
        return item != null;
    }

    /// <summary>
    /// Moves this item to a new position in the same inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="oldPostion"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    public void Move(Vector2Int oldPosition, Vector2Int newPosition){
        Move(oldPosition, newPosition, this);
    }

    /// <summary>
    /// Moves this item to a new position in an inventory. If this slot was already used by another item it swaps the position. 
    /// If this slot was already used by an item of the same type it fills this stack, if this stack isn't big enough the remaining part stays in the old slot.
    /// <summary>
    /// <param name="oldPostion"> The position, where the item was stored in the inventory. </param>
    /// <param name="newPosition"> The position, where the item is now stored in the inventory. </param>
    /// <param name="inventory"> The inventory of the new position. </param>
    public void Move(Vector2Int oldPosition, Vector2Int newPosition, Inventory inventory){
        ItemType thisType = GetItemType(oldPosition);
        ItemType otherType = inventory.GetItemType(newPosition);
        if (otherType == null || thisType.ItemName != otherType.ItemName){
            thisType.Move(oldPosition, newPosition, inventory);
            if (otherType != null){
                otherType.Move(newPosition, oldPosition, this);
                if (otherType.Amount == 0){
                    inventory.type.Remove(otherType);
                }
            }
            if (thisType.Amount == 0){
                type.Remove(thisType);
            }
            inventoryCanvas.MoveSlot(oldPosition, newPosition, inventory);
        } else {
            int amount = thisType.GetItemSlot(oldPosition).Amount;
            int left = otherType.Add(amount, newPosition);
            if (left == 0){
                thisType.Slots.Remove(thisType.GetItemSlot(oldPosition));
                inventoryCanvas.RemoveSlot(oldPosition);
            } else {
                thisType.Remove(amount - left, oldPosition);
                inventoryCanvas.ResetPosition(oldPosition);
            }
        }
    }

    // Getters

    /// <summary> Returns the ItemType for items with this name. </summary>
    /// <param name="itemName"> The name of this item to identify it. </param>
    /// <returns> Returns the ItemType with this name. Null if the name doesn't exist. </returns>
    public ItemType GetItemType(string itemName){
        foreach (ItemType itemType in type){
            if (itemType.ItemName == itemName){
                return itemType;
            }
        }
        return null;
    }

    /// <summary> Returns the ItemType for an item on this position. </summary>
    /// <param name="position"> The position of this item to identify it. </param>
    /// <returns> Returns the ItemType of the item stored on this position. Null if the position was empty. </returns>
    public ItemType GetItemType(Vector2Int position){
        foreach (ItemType itemType in type){
            if (itemType.Contains(position)){
                return itemType;
            }
        }
        return null;
    }

    /// <summary> Returns the next free slot in the inventory. It goes first down and then to the right. </summary>
    /// <returns> Returns the position as Vector2Int. Returns an Vector2Int with (-1, -1) if all slot are full </returns>
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

    /// <summary> Fills the inventory at the start with all items, that are component of the same GameObject as this inventory. </summary>
    private void PrefillInventory(){
        Item[] items = GetComponents<Item>();
        for (int i = 0; i < items.Length; i++){
            Item item = items[i];
            int left = Add(item.GetType().ToString(), item, item.Amount);
            if (left > 0){
                Debug.LogWarning("Can't store all items in this inventory");
            }
            Destroy(item);
        }
    }

    void Start()
    {
        PrefillInventory();
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
        List<string> names = new(){"freeSlot", "type", "inventorySize", "amount", "maxStackSize", "itemName", "slots", "position"};
        return JsonRecursive.ToJson(this, names, 5, true);
    }

    public List<List<bool>> FreeSlot { get => freeSlot; }
    public List<ItemType> Type { get => type; set => type = value; }
    public Vector2Int InventorySize { get => inventorySize; set => inventorySize = value; }
    public InventoryCanvas InventoryCanvas { get => inventoryCanvas; set => inventoryCanvas = value; }
    public bool IsPlayer { get => isPlayer; }
}
