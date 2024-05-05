using UnityEngine;

public class DestroyCanvas : MonoBehaviour
{
    // Start is called before the first frame update
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
