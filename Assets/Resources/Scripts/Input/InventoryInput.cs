using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour {
    private Inputs inputs;
    public GameObject inventoryCanvas;
    
    void Start() {
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
        }
    }
}
