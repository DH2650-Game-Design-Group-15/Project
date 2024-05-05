using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour {
    private Inputs inputs;
    public GameObject inventoryUI;
    
    void Start() {
        Inventory inventory = Parent.FindParent(gameObject, typeof(Inventory)).GetComponent<Inventory>();
        inventoryUI = Parent.FindParent(inventory.InventoryCanvas, "Inventory").gameObject;
        inputs = GetComponent<Inputs>();
    }

    public void OpenInventory(InputAction.CallbackContext context){
        if (context.started){
            inputs.ChangeActionMap("Inventory");
            inventoryUI.SetActive(true);
        }
    }

    public void CloseInventory(InputAction.CallbackContext context){
        if (context.started){
            inputs.ReturnToActionMap();
            inventoryUI.SetActive(false);
            if (Parent.FindChild(inventoryUI, "Inventories").transform.childCount > 1) {
                InventoryCanvas[] inventories = inventoryUI.GetComponentsInChildren<InventoryCanvas>(true);
                for (int i = 0; i < inventories.Length; i++){
                    if (!inventories[i].Inventory.IsPlayer){
                        Destroy(inventories[i].gameObject);
                    }
                }
            }
        }
    }

    public void OpenStorage(InputAction.CallbackContext context){
        if (context.started){
            inputs.ChangeActionMap("Inventory");
            inventoryUI.SetActive(true);
            GameObject storage = GameObject.FindWithTag("Storage");
            storage.GetComponent<Inventory>().ReloadInventoryCanvas();
            storage.GetComponent<Inventory>().InventoryCanvas.transform.parent.SetAsFirstSibling();
            inventoryUI.GetComponentInChildren<HorizontalLayoutGroup>().spacing = 150;
        }
    }
}
