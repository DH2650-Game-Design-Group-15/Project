using UnityEngine;
using UnityEngine.UI;

public class CanvasInventory : MonoBehaviour {
    public GameObject itemPrefab;
    public PlayerInventory player;

    public void CreateInventory(){
        if (!player.InventoryChanged) {
            return;
        }
        if (GetComponentsInChildren<RawImage>(true).Length != player.Rows * player.Column * 2) { 
            // Every slot has 2 raw images. If the amount doesn't fit slots have to be deleted or created.
            CreateEmptyInventory();
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(player.Rows * 50f, player.Column * 50f);
        player.InventoryChanged = false;
        foreach (InventoryItemHelper helper in player.Items) {
            foreach (ItemSlot slot in helper.ItemSlots) {
                string slotName = "Item" + slot.Row.ToString("D2") + slot.Column.ToString("D2");
                Transform oldSlot = transform.Find(slotName);
                if (oldSlot != null) {
                    UpdateInventorySlot(oldSlot.gameObject, slot, false);
                } else {
                    CreateInventorySlot(slot);
                }
            }
        }
    }

    public void CreateEmptyInventory(){
        for (int i = 0; i < player.Rows; i++){
            for (int j = 0; j < player.Column; j++) {
                string name = "Item" + i.ToString("D2") + j.ToString("D2");
                if (transform.Find(name) == null){
                    CreateEmptyInventorySlot(i, j);
                }
            }
        }
    }

    void UpdateInventorySlot(GameObject slot, ItemSlot itemSlot, bool isEmpty){
        RawImage image = slot.GetComponentInChildren<Child>().GetComponent<RawImage>();
        if (isEmpty){
            image.enabled = false;
        } else {
            image.texture = itemSlot.Texture;
            image.enabled = true;
        }
    }

    void CreateInventorySlot(ItemSlot itemSlot) {
        int index = itemSlot.Row * player.Column + itemSlot.Column;
        GameObject slot = Instantiate(itemPrefab, transform);
        slot.name = "Item" + itemSlot.Row.ToString("D2") + itemSlot.Column.ToString("D2");
        slot.transform.SetSiblingIndex(index);
        UpdateInventorySlot(slot, itemSlot, false);
    }

    void CreateEmptyInventorySlot(int row, int column){
        int index = row * player.Column + column;
        GameObject slot = Instantiate(itemPrefab, transform);
        slot.name = "Item" + row.ToString("D2") + column.ToString("D2");
        slot.transform.SetSiblingIndex(index);
        UpdateInventorySlot(slot, null, true);
    }

    void Start() {
        CreateEmptyInventory();
    }
}
