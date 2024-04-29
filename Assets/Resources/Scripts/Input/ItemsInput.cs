using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
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

    /// <summary>
    /// Called when the player tries to take an object. 
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

    /// <summary>
    /// Returns a UI GameObject, that's below the mouse. The GameObjects name must equal pattern
    /// The pattern has to be a RegEx expression
    /// Returns null, if no GameObject with this name is found
    /// </summary>
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

    public (int, int) GetPositionFromName(string name){
        string columnStr = name.Substring(5, 2);
        string rowStr = name.Substring(7, 2);
        bool columnIsNum = int.TryParse(columnStr, out int column);
        bool rowIsNum = int.TryParse(rowStr, out int row);
        if (!columnIsNum || !rowIsNum){
            Debug.LogWarning(string.Format("Position isn't an integer. Column is: {0,2} and Row is: {1,2}", column, row));
            return (-1, -1);
        } else if (column < 0 || row < 0){
            Debug.LogWarning(string.Format("Number was negativ: {0:3}", math.min(column, row)));
            return (-1, -1);
        }
        return (column, row);
    }

    /// <summary>
    /// Called when a player press the button to remove an item from the inventory.
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
