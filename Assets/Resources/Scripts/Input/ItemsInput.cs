using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemsInput : MonoBehaviour {
    public CacheCloseObjects closeObjectsScript;
    public PlayerInventory playerInventoryScript;

    void Start(){
        Transform parent = transform.parent ?? transform;
        closeObjectsScript = parent.GetComponentInChildren<CacheCloseObjects>();
        playerInventoryScript = parent.GetComponent<PlayerInventory>();
    }

    /// <summary> Called when the player tries to take an object. 
    /// If there're objects nearby a random item gets picked. If no object is nearby nothing happens.
    /// </summary>
    public void OnTakeItem(InputAction.CallbackContext context){
        if (context.started){
            HashSet<GameObject> items = closeObjectsScript.GetNearItems();
            if (items.Count > 0){
                foreach (GameObject item in items) {
                    int left = playerInventoryScript.Add(item.GetComponent<Item>());
                    if (left == 0){
                        closeObjectsScript.OnTriggerExit(item.GetComponent<Collider>());
                        Debug.Log(item.GetComponent<Item>().Amount);
                        Destroy(item);
                    } else {
                        item.GetComponent<Item>().Amount = left;
                    }
                    break;
                }
            }
        }
    }

    /// <summary> Returns a UI GameObject, that's below the mouse. The GameObjects name must equal pattern </summary>
    /// <param name="pattern"> RegEx Expression, that has to fit the GameObjects name </param>
    /// <returns> The GameObject below the mouse. Returns null, if no GameObject fits with the name </returns>
    public GameObject ItemOnMouse(string pattern){
        List<RaycastResult> raycasts = new();
        PointerEventData pointer = new(EventSystem.current) {
            position = Mouse.current.position.ReadValue()
        };
        EventSystem.current.RaycastAll(pointer, raycasts);
        
        foreach (RaycastResult raycast in raycasts) {
            if (Regex.IsMatch(raycast.gameObject.name, pattern)){
                return raycast.gameObject;
            }
        }
        return null;
    }

    /// <summary> Returns a position stored in name. name must contain 4 digits, else it can't identify the values. 
    /// The first to digits are the column, the two other digits are the row. </summary>
    /// <param name="name"> String that must contain 4 digits. </param>
    /// <returns> The position as Tuple (column, row). If the string had more or less than 4 digits it returns (-1, -1), 
    /// else it returns the position. </returns>
    public (int, int) GetPositionFromName(string name){
        List<int> digits = new();
        foreach (char digit in name) {
            bool isDigit = int.TryParse(digit.ToString(), out int number);
            if (isDigit){
                digits.Add(number);
            }
        }
        if (digits.Count != 4){
            Debug.LogWarning("Position isn't an integer.");
            return (-1, -1);
        }
        int column = 10 * digits[0] + digits[1];
        int row = 10 * digits[2] * digits[3];
        return (column, row);
    }

    /// <summary> Called when a player press the button to remove an item from the inventory.
    /// Removes the texture from this item slot and deletes the entry in the players inventory
    /// </summary>
    public void OnRemoveStack(InputAction.CallbackContext context){
        if (context.started){
            string itemPattern = @"^Item\d{4}$";
            GameObject itemUI = ItemOnMouse(itemPattern);
            if (itemUI != null){
                ItemReference reference = itemUI.GetComponentInChildren<ItemReference>();
                Item item = reference.Item;
                if (item != null){
                    (int column, int row) = GetPositionFromName(itemUI.name);
                    if (column < 0 || row < 0){
                        return;
                    }
                    bool removed = playerInventoryScript.RemoveStack(item.name, column, row);
                    if (removed){
                        reference.GetComponent<RawImage>().enabled = false;
                        reference.GetComponent<RawImage>().texture = null;
                    }
                }
            }
        }
    }
}
