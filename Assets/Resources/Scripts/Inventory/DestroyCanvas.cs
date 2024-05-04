using UnityEngine;

public class DestroyCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Update() {
        Transform inventory = transform.parent.Find("Inventories");
        InventoryCanvas[] canvas = inventory.GetComponentsInChildren<InventoryCanvas>(true);
        for (int i = 0; i < canvas.Length; i++){
            if (canvas[i].Inventory.GetComponentInChildren<InventoryInput>() == null){
                Destroy(canvas[i].transform.parent.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
