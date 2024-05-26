using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour {

    public int cannisterCompletionAmount;
    public int metalCompletionAmount;
    public RawImage shipUIImage;
    public GameObject shipUI;
    public GameObject itemAmount;
    public ObjectDetection objectDetection;
    public Inventory playerInventory;
    public InventoryInput inventoryInput;
    public Texture cannisterImage;
    public Texture metalImage;
    private int cannisterCurrentAmount;
    private int metalCurrentAmount;
    private bool cannisterUI;
    private bool metalUI;

    public void Start () {
        shipUI.SetActive(false);
    }

    public void Update () {
        if (cannisterUI) {
            itemAmount.GetComponent<TextMeshProUGUI>().text = cannisterCurrentAmount.ToString() + "/" + cannisterCompletionAmount.ToString();
            shipUIImage.texture = cannisterImage;
        } else if (metalUI) {
            itemAmount.GetComponent<TextMeshProUGUI>().text = metalCurrentAmount.ToString() + "/" + metalCompletionAmount.ToString();
            shipUIImage.texture = metalImage;
        }
    }

    public void OpenShipUI (InputAction.CallbackContext context) {
        if (context.started) {
            GameObject[] collisionObjects = objectDetection.DetectObjects();
            foreach (GameObject obj in collisionObjects) {
                if (obj.name == "Cannisters") {
                    cannisterUI = true;
                }
                if (obj.name == "BoxSupplies") {
                    metalUI = true;
                }
            }
            if (cannisterUI || metalUI) {
                if (!shipUI.activeSelf) {
                    inventoryInput.SetCursor(true);
                    shipUI.SetActive(true);
                } else {
                    inventoryInput.SetCursor(false);
                    shipUI.SetActive(false);
                }
            }
        }
    }

    public void Add () {
        if (cannisterUI) {
            increaseCannisterAmount();
        } else if (metalUI) {
            increaseMetalAmount();
        }
    }

    private void increaseCannisterAmount () {
        if (cannisterCurrentAmount < cannisterCompletionAmount) {
            int uraniumAmt = playerInventory.Amount("Uranium");
            Debug.Log(uraniumAmt);
            if (uraniumAmt > 0) {
                playerInventory.Remove("Uranium", 1);
                cannisterCurrentAmount++;
            }
        }
    }

    private void increaseMetalAmount () {
        if (metalCurrentAmount < metalCompletionAmount) {
            int metalAmt = playerInventory.Amount("MetalPlate");
            if (metalAmt > 0) {
                playerInventory.Remove("MetalPlate", 1);
                metalCurrentAmount++;
            }
        }
    }

}