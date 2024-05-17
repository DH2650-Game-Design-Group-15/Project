using UnityEngine;

/// <summary> Helper to delete the UI for all inventories except the players inventory. </summary>
public class InventoryUiDestroyer : MonoBehaviour {
    /// <summary> Deletes the inventory of all storages in the first frame. </summary>
    void Update() {
        Transform inventories = Parent.FindParentSibling(gameObject, "Inventories");
        InventoryCanvas[] canvas = inventories.GetComponentsInChildren<InventoryCanvas>(true);
        for (int i = 0; i < canvas.Length; i++){
            if (canvas[i].Inventory.GetComponentInChildren<InventoryInput>() == null){
                Destroy(canvas[i].gameObject);
            }
        }
        Parent.FindParentSibling(gameObject, "Inventory").gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
