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
    /// <param name="column"> The column to store the item in the inventory </param>
    /// <param name="row"> The row to store the item in the inventory </param>
    public void EnableSlot(Item item, int column, int row){
        string slotName = "Item" + column.ToString("00") + row.ToString("00");
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
        Debug.Log(reference.ItemName);
    }

    /// <summary> Removes a existing item from this slot in the inventories UI. </summary>
    /// <param name="column"> The column to remove the item in the inventory </param>
    /// <param name="row"> The row to remove the item in the inventory </param>
    public void DisableSlot(int column, int row){
        string slotName = "Item" + column.ToString("00") + row.ToString("00");
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
        for (int i = 0; i < player.Rows; i++){
            for (int j = 0; j < player.Columns; j++) {
                string name = "Item" + i.ToString("D2") + j.ToString("D2");
                if (transform.Find(name) == null){
                    CreateEmptyInventorySlot(i, j);
                }
            }
        }
        Vector2 cellSize = GetComponent<GridLayoutGroup>().cellSize;
        Vector2 spacing = GetComponent<GridLayoutGroup>().spacing;
        GetComponent<RectTransform>().sizeDelta = new Vector2(player.Rows * (cellSize.x + spacing.x), player.Columns * (cellSize.y + spacing.y));
    }

    private void CreateEmptyInventorySlot(int row, int column){
        int index = row * player.Columns + column;
        GameObject slot = Instantiate(itemPrefab, transform);
        slot.name = "Item" + row.ToString("D2") + column.ToString("D2");
        slot.transform.SetSiblingIndex(index);
        RawImage image = slot.GetComponentInChildren<ItemReference>().GetComponent<RawImage>();
        image.enabled = false;
    }

    void Start() {
        CreateEmptyInventory();
        transform.parent.gameObject.SetActive(false);
    }
}
