using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemsInput : MonoBehaviour {
    public CacheCloseObjects closeObjectsScript;
    public PlayerInventory playerInventoryScript;
    private bool move = false;
    private bool split = false;
    private GameObject moveObject;
    private GameObject slotMoveObject;
    private Vector2 mouseStartPos;
    private Vector3 startingPos;

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
            Debug.LogWarning("Position isn't an integer: " + name);
            return (-1, -1);
        }
        int column = 10 * digits[0] + digits[1];
        int row = 10 * digits[2] + digits[3];
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
                RemoveStack(reference);
            }
        }
    }

    private void RemoveStack(ItemReference reference){
        string itemName = reference.ItemName;
        if (itemName != null){
            (int column, int row) = GetPositionFromName(reference.transform.parent.gameObject.name);
            if (column < 0 || row < 0){
                return;
            }
            bool removed = playerInventoryScript.RemoveStack(itemName, column, row);
            if (removed){
                reference.GetComponent<RawImage>().enabled = false;
                reference.GetComponent<RawImage>().texture = null;
            }
            // TODO Spawn Object
        }

    }

    public void OnMoveItem(InputAction.CallbackContext context){
        string itemPattern = @"^Item\d{4}";
        if (context.started){
            slotMoveObject = ItemOnMouse(itemPattern);
            if (slotMoveObject != null && slotMoveObject.GetComponentInChildren<ItemReference>().ItemName != null){ // First check if there's an item slot and then if it stores an item
                moveObject = slotMoveObject.GetComponentInChildren<ItemReference>().gameObject;
                move = true;
                mouseStartPos = Mouse.current.position.ReadValue();
                startingPos = moveObject.GetComponent<RectTransform>().position;
                moveObject.transform.SetParent(slotMoveObject.transform.parent.parent.Find("MoveHelper"));
            }
        }
        if (context.canceled && move){
            move = false;
            if (split){
                
            } else {
                moveObject.transform.SetParent(slotMoveObject.transform);
                GameObject newSlot = ItemOnMouse(itemPattern);
                if (newSlot == null){
                    RemoveStack(moveObject.GetComponent<ItemReference>());
                } else if (newSlot != slotMoveObject) {
                    GameObject oldItemInSlot = newSlot.GetComponentInChildren<ItemReference>().gameObject;
                    (int oldColumn, int oldRow) = GetPositionFromName(slotMoveObject.name);
                    (int newColumn, int newRow) = GetPositionFromName(newSlot.name);
                    playerInventoryScript.Move(oldItemInSlot.GetComponent<ItemReference>().ItemName, newColumn, newRow, oldColumn, oldRow);
                    playerInventoryScript.Move(moveObject.GetComponent<ItemReference>().ItemName, oldColumn, oldRow, newColumn, newRow);
                    Vector3 position = new(1, 0, 0);
                    oldItemInSlot.transform.SetParent(slotMoveObject.transform);
                    moveObject.transform.SetParent(newSlot.transform);
                    oldItemInSlot.GetComponent<RectTransform>().localPosition = position;
                    moveObject.GetComponent<RectTransform>().localPosition = position;
                    // TODO Add if same item
                } else {
                    moveObject.transform.localPosition = new Vector3(1, 0, 0);
                }
            }
        }
    }

    void Update(){
        if (move){
            Move();
        }
    }

    private void Move(){
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 newPosition = new(mouse.x - mouseStartPos.x + startingPos.x, mouse.y - mouseStartPos.y + startingPos.y, startingPos.z);
        moveObject.GetComponent<RectTransform>().position = newPosition;
    }

    public void OnSplitItem(InputAction.CallbackContext context){
        if (context.started){
            split = true;
        }
        if (context.canceled){
            split = false;
        }
    }
}
