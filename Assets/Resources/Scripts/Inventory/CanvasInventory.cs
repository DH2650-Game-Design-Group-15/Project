using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates the UI for the inventory and changes the images in each slot.
/// </summary>
public class CanvasInventory : MonoBehaviour {
    public GameObject itemPrefab;
    public PlayerInventory player;
    
    /// <summary> Fills the slot in the inventory UI with this item </summary>
    /// <param name="item"> The item, especially it's image, to display in the inventory. </param>
    /// <param name="position"> The position to store the item in the inventory </param>
    public void EnableSlot(Item item, Vector2Int position){
        string slotName = "Item" + position.x.ToString("00") + position.y.ToString("00");
        Transform slot = transform.Find(slotName);
        if (slot == null){
            Debug.LogWarning("Can't find item slot: " + slotName);
            return;
        }
        ItemReference reference = slot.GetComponentInChildren<ItemReference>();
        RawImage image = reference.GetComponent<RawImage>();
        image.texture = item.ImageInventory;
        image.enabled = true;
        reference.ItemName = item.GetType().ToString();
        reference.Amount = item.Amount;
    }

    /// <summary> Removes a existing item from this slot in the inventories UI. </summary>
    /// <param name="position"> Where to remove the item in the inventory </param>
    public void DisableSlot(Vector2Int position){
        string slotName = "Item" + position.x.ToString("00") + position.y.ToString("00");
        Transform slot = transform.Find(slotName);
        if (slot == null) {
            Debug.LogWarning("Can't find item slot: " + slotName);
            return;
        }
        ItemReference reference = slot.GetComponentInChildren<ItemReference>();
        RawImage image = reference.GetComponent<RawImage>();
        image.texture = null;
        image.enabled = false;
        reference.ItemName = null;
        reference.Amount = 0;
    }

    /// <summary> Creates an empty inventory. The size is stored in playerInventory. </summary>
    public void CreateEmptyInventory(){
        for (int x = 0; x < player.InventorySize.x; x++){
            for (int y = 0; y < player.InventorySize.y; y++) {
                string name = "Item" + x.ToString("D2") + y.ToString("D2");
                if (transform.Find(name) == null){
                    CreateEmptyInventorySlot(new Vector2Int(x, y));
                }
            }
        }
        Vector2 cellSize = GetComponent<GridLayoutGroup>().cellSize;
        Vector2 spacing = GetComponent<GridLayoutGroup>().spacing;
        GetComponent<RectTransform>().sizeDelta = new Vector2(player.InventorySize.x * (cellSize.x + spacing.x), player.InventorySize.y * (cellSize.y + spacing.y));
    }

    private void CreateEmptyInventorySlot(Vector2Int position){
        int index = position.x * player.InventorySize.y + position.y;
        GameObject slot = Instantiate(itemPrefab, transform);
        if (index >= transform.GetComponentsInChildren<ItemReference>().Length){
            Debug.LogWarning("index is too big: " + position);
        }
        slot.name = "Item" + position.x.ToString("D2") + position.y.ToString("D2");
        slot.transform.SetSiblingIndex(index);
        RawImage image = slot.GetComponentInChildren<ItemReference>().GetComponent<RawImage>();
        image.enabled = false;
    }

    void Start() {
        CreateEmptyInventory();
        transform.parent.gameObject.SetActive(false);
    }
}
