using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour {
    private Inputs inputs;
    public GameObject inventoryUI;
    private ObjectDetection objectDetection;
    private float maxStorageDistance = 10f;
    
    void Start() {
        Inventory inventory = Parent.FindParent(gameObject, typeof(Inventory)).GetComponent<Inventory>();
        inventoryUI = Parent.FindParent(inventory.InventoryCanvas, "Inventory")?.gameObject;
        inputs = GetComponent<Inputs>();
        objectDetection = Parent.FindChild(inventory, typeof(ObjectDetection))?.GetComponent<ObjectDetection>();
        if (inventoryUI == null || objectDetection == null){
            Debug.LogError("Can't find all components");
        }
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
            GameObject[] objects = objectDetection.DetectObjects();
            objects = ObjectDetection.ObjectsWithComponent(objects, typeof(Inventory));
            (GameObject storage, float distance) = objectDetection.ClosestObject(objects);
            if (distance < maxStorageDistance){
                inputs.ChangeActionMap("Inventory");
                inventoryUI.SetActive(true);
                storage.GetComponent<Inventory>().ReloadInventoryCanvas();
                storage.GetComponent<Inventory>().InventoryCanvas.transform.parent.SetAsFirstSibling();
                inventoryUI.GetComponentInChildren<HorizontalLayoutGroup>().spacing = 150;
            }
        }
    }
}
