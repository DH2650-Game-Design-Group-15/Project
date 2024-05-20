using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates the UI for the inventory and changes the images in each slot.
/// </summary>
public class InventoryCanvas : MonoBehaviour{
    public GameObject itemPrefab;
    private Inventory inventory;
    private bool changeSize;
    private Vector3 positionInSlot = Vector3.right;

    void Awake(){
        LoadPrefab();
    }

    /// <summary> Loads the prefab of an empty slot </summary>
    private void LoadPrefab(){
        itemPrefab = Resources.Load<GameObject>("Prefabs/UI/Inventory/Item");
    }

    /// <summary> Fills the slot in the inventory UI with this item. </summary>
    /// <param name="position"> Place in the UI to add the item. </param>
    /// <param name="itemName"> The name of the item. </param>
    /// <param name="amount"> The amount of the item in this slot </param>
    /// <param name="texture"> The texture of the item in the UI. This is usually an image of how the item looks like. </param>
    public void AddSlot(Vector2Int position, string itemName, int amount, Texture texture){
        ItemReference slotReference = transform.Find(SlotName(position.x, position.y)).GetComponentInChildren<ItemReference>();
        RawImage slotImage = slotReference.GetComponent<RawImage>();
        slotImage.texture = texture;
        slotImage.enabled = true;
        slotReference.ItemName = itemName;
        slotReference.Amount = amount;
    }

    /// <summary> Removes an existing item from this slot in the inventories UI. </summary>
    /// <param name="position"> Where to remove the item in the inventory </param>
    public void RemoveSlot(Vector2Int position){
        ItemReference slotReference = transform.Find(SlotName(position.x, position.y)).GetComponentInChildren<ItemReference>();
        RawImage slotImage = slotReference.GetComponent<RawImage>();
        slotImage.texture = null;
        slotImage.enabled = false;
        slotReference.ItemName = null;
        slotReference.Amount = 0;
    }

    /// <summary> Swaps the position of two items. The item can also be null. </summary>
    /// <param name="oldPosition"> The position of the first item. </param>
    /// <param name="newPosition"> The position of the second item. </param>
    public void MoveSlot(Vector2Int oldPosition, Vector2Int newPosition){
        MoveSlot(oldPosition, newPosition, inventory);
    }

    /// <summary> Swaps the position of two items. The item can also be null. </summary>
    /// <param name="oldPosition"> The position of the first item. </param>
    /// <param name="newPosition"> The position of the second item. </param>
    /// <param name="inventory"> The inventory of the new position. </param>
    public void MoveSlot(Vector2Int oldPosition, Vector2Int newPosition, Inventory inventory){
        Transform oldSlot = transform.Find(SlotName(oldPosition.x, oldPosition.y)).transform;
        Transform newSlot = inventory.inventoryCanvas.transform.Find(SlotName(newPosition.x, newPosition.y)).transform;
        Transform oldItem = oldSlot.GetComponentInChildren<ItemReference>().transform;
        Transform newItem = newSlot.GetComponentInChildren<ItemReference>().transform;
        newItem.SetParent(oldSlot);
        oldItem.SetParent(newSlot);
        newItem.GetComponent<RectTransform>().localPosition = positionInSlot;
        oldItem.GetComponent<RectTransform>().localPosition = positionInSlot;
    }

    /// <summary> Finds the slot at this position and resets the position of the item inside to this slot </summary>
    /// <param name="position"> The position to reset </param>
    public void ResetPosition(Vector2Int position){
        transform.Find(SlotName(position.x, position.y)).GetComponentInChildren<ItemReference>().GetComponent<RectTransform>().localPosition = positionInSlot;
    }

    /// <summary> Changes the amount of items in this slot. 
    /// The amount here should always equal the amount in the inventory DB. </summary>
    /// <param name="position"> The position of the item in the inventory. </param>
    /// <param name="amount"> The new amount of the item in the inventory.
    /// <remarks> If the amount doesn't fit with the amount in the DB, it only shows the player a wrong amount, 
    /// everything is calculated with the amount in the DB. 
    public void Amount(Vector2Int position, int amount){
        transform.Find(SlotName(position.x, position.y)).GetComponentInChildren<ItemReference>().Amount = amount;
    }

    /// <summary> Compares the size of the inventory UI with the inventory DB.
    /// If the DB is bigger it creates slots in the UI until it fits. If the DB is smaller it deletes
    /// the slots most right or below until it fits. </summary>
    /// <remarks> Right now items in deleted slots get lost. </remarks>
    public void CreateInventoryCanvas(){
        int sibling = 0;
        int x = 0;
        int y = 0;
        while (true){
            if (x < inventory.InventorySize.x && y < inventory.InventorySize.y){
                if (transform.Find(SlotName(x, y)) == null){
                    if (itemPrefab == null){
                        LoadPrefab();
                    }
                    GameObject slot = Instantiate(itemPrefab, transform);
                    slot.name = SlotName(x, y);
                    slot.transform.SetSiblingIndex(sibling);
                }
                x++;
                sibling++;
            } else if (x >= inventory.InventorySize.x){
                Transform destroy = transform.Find(SlotName(x, y));
                if (destroy == null){
                    x = 0;
                    y++;
                } else {
                    Destroy(destroy.gameObject);
                    // TODO Move or remove items in removed slots
                }
            } else if (y >= inventory.InventorySize.y){
                Transform destroy = transform.Find(SlotName(x, y));
                if (destroy == null){
                    break;
                } else {
                    Destroy(destroy.gameObject);
                    // TODO Move or remove items in removed slots
                }
            }
        }
        GridLayoutGroup layout = GetComponent<GridLayoutGroup>();
        GetComponent<RectTransform>().sizeDelta = new Vector2(inventory.InventorySize.x * (layout.cellSize.x + layout.spacing.x), 
                                                                inventory.InventorySize.y * (layout.cellSize.y + layout.spacing.y));
    }

    /// <summary> Sets the inventory UI to the same state as the inventory. 
    /// Therefore it creates or deletes first slots, clears all slots and last fills it with the actual item. </summary>
    public void UpdateInventory(){
        CreateInventoryCanvas();
        ClearInventory();
        FillInventory();
    }

    /// <summary> Fills the inventory UI with the items stored in the inventory </summary>
    private void FillInventory(){
        foreach (ItemType type in inventory.Type) {
            foreach (ItemSlot item in type.Slots) {
                AddSlot(item.Position, type.ItemName, item.Amount, type.Texture);
            }
        }
    }

    /// <summary> Removes all items stored in the UI </summary>
    private void ClearInventory(){
        ItemReference[] references = GetComponentsInChildren<ItemReference>();
        foreach (ItemReference reference in references) {
            reference.Amount = 0;
            reference.ItemName = null;
            RawImage image = reference.GetComponent<RawImage>();
            image.enabled = false;
            image.texture = null;
        }
    }

    /// <summary> Returns the default name of a slot in the UI. 
    /// The default name is "Item" + position with 4 digits. </summary>
    /// <param name="x"> The x position for the first 2 digits in the slot name </param>
    /// <param name="y"> The y position for the last 2 digits in the slot name </param>
    public static string SlotName(int x, int y){
        return SlotName("Item", x, y);
    }

    /// <summary> Returns the name of a slot in the UI. </summary>
    /// <param name="prefix"> The string that should be in front of the numbers in the slot name </param>
    /// <param name="x"> The x position for the first 2 digits in the slot name </param>
    /// <param name="y"> The y position for the last 2 digits in the slot name </param>
    public static string SlotName(string prefix, int x, int y){
        return prefix + x.ToString("00") + y.ToString("00");
    }

    void Update(){
        if (changeSize){
            CreateInventoryCanvas();
            changeSize = false;
        }
    }

    public Inventory Inventory { get => inventory; set { inventory = value; changeSize = true; } }
}
