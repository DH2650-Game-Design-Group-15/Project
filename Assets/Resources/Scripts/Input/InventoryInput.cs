using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour {
    private Inputs inputs;
    public GameObject inventoryCanvas;
    
    void Start() {
        inventoryCanvas = transform.parent.GetComponent<Inventory>().InventoryCanvas.transform.parent.gameObject;
        inputs = GetComponent<Inputs>();
    }

    public void OpenInventory(InputAction.CallbackContext context){
        if (context.started){
            inputs.ChangeActionMap("Inventory");
            inventoryCanvas.SetActive(true);
        }
    }

    public void CloseInventory(InputAction.CallbackContext context){
        if (context.started){
            inputs.ReturnToActionMap();
            inventoryCanvas.SetActive(false);
            if (inventoryCanvas.transform.GetSiblingIndex() > 0) {
                InventoryCanvas[] inventories = inventoryCanvas.transform.parent.GetComponentsInChildren<InventoryCanvas>(true);
                for (int i = 0; i < inventories.Length; i++){
                    if (inventories[i].transform.parent != inventoryCanvas.transform){
                        Destroy(inventories[i].transform.parent.gameObject);
                    }
                }
            }
        }
    }

    public void OpenStorage(InputAction.CallbackContext context){
        if (context.started){
            inputs.ChangeActionMap("Inventory");
            inventoryCanvas.SetActive(true);
            GameObject storage = GameObject.FindWithTag("Storage");
            storage.GetComponent<Inventory>().ReloadInventoryCanvas();
            storage.GetComponent<Inventory>().InventoryCanvas.transform.parent.SetAsFirstSibling();
            transform.parent.GetComponent<Inventory>().InventoryCanvas.transform.parent.parent.GetComponent<HorizontalLayoutGroup>().spacing = 150;
        }
    }
}
