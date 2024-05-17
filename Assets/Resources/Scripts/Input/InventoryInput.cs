using DestroyIt;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary> 
/// Called functions to open or close inventories.
/// </summary>
/// <remarks> Needs Component "Inventory" in a parent (or itself) </remarks>
/// <remarks> "Inventory" or a child must have the component "ObjectDetection" </remarks>
public class InventoryInput : MonoBehaviour {
    private Inputs inputs;
    public GameObject inventoryUI;
    private ObjectDetection objectDetection;
    private float maxStorageDistance = 10f;
    private FirstPersonController firstPersonController;
    private InputManager inputManager;
    
    /// <summary> Finds the Component Inputs and the parent GameObject of the UI for inventories. </summary>
    void Start() {
        Inventory inventory = Parent.FindParent(gameObject, typeof(Inventory)).GetComponent<Inventory>();
        inventoryUI = Parent.FindParent(inventory.InventoryCanvas, "Inventory")?.gameObject;
        inputs = GetComponent<Inputs>();
        objectDetection = Parent.FindChild(inventory, typeof(ObjectDetection))?.GetComponent<ObjectDetection>();
        if (inventoryUI == null || objectDetection == null){
            Debug.LogError("Can't find all components");
        }
        inputManager = Parent.FindParent(inventory, typeof(InputManager))?.GetComponent<InputManager>();
        firstPersonController = Parent.FindParent(inventory, typeof(FirstPersonController))?.GetComponent<FirstPersonController>();
    }

    /// <summary> Opens the inventory and enables the cursor. It changes also the action map to inventory. </summary>
    /// <param name="context"> CallbackContext, that the function is called only once when the button is pressed first. </param>
    public void OpenInventory(InputAction.CallbackContext context){
        if (context.started){
            SetCursor(true);
            inputs.ChangeActionMap("Inventory");
            inventoryUI.SetActive(true);
        }
    }
    
    /// <summary> Closes the inventory and disables the cursor. It restores all action maps before the inventory was opened. </summary>
    /// <param name="context"> CallbackContext, that the function is called only once when the button is pressed first. </param>
    public void CloseInventory(InputAction.CallbackContext context){
        if (context.started){
            SetCursor(false);
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
    
    /// <summary> Opens the player inventory and the inventory from the storage we are looking at. If no storage is in front of us 
    /// nothing happens. </summary>
    /// <param name="context"> CallbackContext, that the function is called only once when the button is pressed first. </param>
    public void OpenStorage(InputAction.CallbackContext context){
        if (context.started){
            GameObject[] objects = objectDetection.DetectObjects();
            objects = ObjectDetection.ObjectsWithComponent(objects, typeof(Inventory));
            (GameObject storage, float distance) = objectDetection.ClosestObject(objects);
            if (distance < maxStorageDistance){
                SetCursor(true);
                inputs.ChangeActionMap("Inventory");
                inventoryUI.SetActive(true);
                storage.GetComponent<Inventory>().ReloadInventoryCanvas();
                storage.GetComponent<Inventory>().InventoryCanvas.transform.parent.SetAsFirstSibling();
                inventoryUI.GetComponentInChildren<HorizontalLayoutGroup>().spacing = 150;
            }
        }
    }

    /// <summary> Enables or disables the cursor </summary>
    /// <param name="active" true, if the cursor should be enabled, false if the cursor should be disabled. </param>
    private void SetCursor(bool active){
        if (active){
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        firstPersonController.enabled = !active;
        inputManager.enabled = !active;
        Cursor.visible = active;
    }
}
