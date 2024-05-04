using UnityEngine;

public class DestroyCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Update() {
        Transform inventory = transform.parent.Find("Inventories");
        InventoryCanvas[] canvas = inventory.GetComponentsInChildren<InventoryCanvas>(true);
        Debug.Log(canvas.Length + inventory.name);

        for (int i = 0; i < canvas.Length; i++){
            Debug.Log(canvas[i].transform.parent.name);
            if (canvas[i].Inventory.GetComponentInChildren<InventoryInput>() == null){
                Destroy(canvas[i].transform.parent.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
