using UnityEngine;
using UnityEngine.UI;

public class CanvasInventory : MonoBehaviour {
    public GameObject itemPrefab;
    public PlayerInventory player;
    
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
        reference.Item = item;
    }

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
        reference.Item = null;
    }

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

    void CreateEmptyInventorySlot(int row, int column){
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
