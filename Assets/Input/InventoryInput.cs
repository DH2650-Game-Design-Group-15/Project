using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour {
    private Inputs inputs;
    public GameObject inventoryCanvas;
    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<Inputs>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openInventory(InputAction.CallbackContext context){
        inputs.ChangeActionMap("Inventory");
        inventoryCanvas.SetActive(true);
    }

    public void closeInventory(InputAction.CallbackContext context){
        inputs.ReturnToActionMap();
        inventoryCanvas.SetActive(false);
    }
}
