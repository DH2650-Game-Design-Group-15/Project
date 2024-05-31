using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary> 
/// Called functions to pick items and interact with them in the inventory. 
/// </summary>
/// <remarks> Needs Component "Inventory" in a parent (or itself) </remarks>
/// <remarks> "Inventory" or a child must have the component "ObjectDetection" </remarks>
public class ItemsInput : MonoBehaviour {
    private Inventory inventory;
    private ObjectDetection objectDetection;
    // Only for Move and Split 
    private bool move = false;
    private bool split = false;
    private GameObject moveItem;
    private GameObject moveSlot;
    private Vector2 mouseStartPos;
    private Vector3 startingPos;
    private static Vector3 positionInSlot = new(1, 0, 0);
    public Inventory moveInventory;
    private Usage usage;

    /// <summary> Finds the players inventory and the Component ObjectDetection to get the next item in front of the player </summary>
    void Start(){
        inventory = Parent.FindParent(gameObject, typeof(Inventory))?.GetComponent<Inventory>();
        objectDetection = Parent.FindChild(inventory, typeof(ObjectDetection))?.GetComponent<ObjectDetection>();
        usage = GetComponent<Usage>();
    }

    /// <summary> 
    /// Called when the player tries to take an object. If there's an object in front of the player the player picks all (or as many as possible).
    /// If there's no object nothing happens.
    /// </summary>
    public void OnTakeItem(InputAction.CallbackContext context){
        if (context.started){
            GameObject[] objects = objectDetection.DetectObjects();                         // all objects nearby
            objects = ObjectDetection.ObjectsWithComponent(objects, typeof(Item));          // filter by items
            (GameObject obj, _) = objectDetection.ClosestObject(objects);                   // filter closest one
            if (obj != null) {
                Item item = obj.GetComponent<Item>();
                int left = inventory.Add(item.GetType().ToString(), item, item.Amount);
                if (left == 0){
                    Destroy(obj);
                } else {
                    item.Amount = left;
                }
            }
        }
    }

    /// <summary> 
    /// Returns the GameObject to which the cursor is pointing. The name of this GameObject must equal the given pattern.
    /// If there're multiple GameObjects with the right name below the cursor it returns the first hit.
    /// </summary>
    /// <param name="pattern"> RegEx Expression, that has to fit the GameObjects name </param>
    /// <returns> A GameObject below the mouse. It's name equals the pattern </returns>
    /// <example> If pattern is "^[Nn]ame\d{2}$" the name could be something like "name00" or "Name69" but nothing like "name 00" or "Name123" </example>
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

    /// <summary> 
    /// Returns a position which must be between 00 and 99. The given string must contain exactly 4 digits to be valid.
    /// The first to digits are the x value, the two other digits are the y value.
    /// </summary>
    /// <param name="name"> String that must contain 4 digits. </param>
    /// <returns> The position as Vector2Int. If the string had more or less than 4 digits it returns (-1, -1), 
    /// else it returns the position. </returns>
    public Vector2Int GetPositionFromName(string name){
        List<int> digits = new();
        foreach (char digit in name) {
            bool isDigit = int.TryParse(digit.ToString(), out int number);
            if (isDigit){
                digits.Add(number);
            }
        }
        if (digits.Count != 4){
            Debug.LogWarning("Position isn't an integer: " + name);
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(10 * digits[0] + digits[1], 10 * digits[2] + digits[3]);
    }

    /// <summary> 
    /// Removes a stack of this item and drops it in front of the player. The function is called everytime the player presses 
    /// the button to remove an item from the inventory. It removes the item below of the cursor, if this slot is empty nothing happens. </summary>
    /// <param name="context"> CallbackContext, that the function is called only once when the button is pressed first. </param>
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

    /// <summary> 
    /// Removes a stack of the given item and drops it in front of the player. If the name of this object is null nothing happens.
    /// </summary>
    /// <param name="reference"> The ItemReference of the UI slot to the slot in the inventory. </param>
    private void RemoveStack(ItemReference reference){
        string itemName = reference.ItemName;
        if (itemName != null){
            Vector2Int position = GetPositionFromName(reference.transform.parent.gameObject.name);
            if (position.x < 0 || position.y < 0){
                return;
            }
            bool removed = Parent.FindParent(reference.gameObject, typeof(InventoryCanvas)).GetComponent<InventoryCanvas>().Inventory.RemoveStack(position);
            if (removed){
                reference.GetComponent<RawImage>().enabled = false;
                reference.GetComponent<RawImage>().texture = null;
            }
        }
    }

    /// <summary>
    /// Moves an item to another slot when the button is pressed. </summary>
    /// <param name="context"> CallbackContext, that the function is called only once when the button is pressed first. </param>
    public void OnMoveItem(InputAction.CallbackContext context){
        if (context.started){
            MoveStart();
        }
        if (context.canceled && move){
            MoveEnd();
        }
    }

    /// <summary> Stores the moved item in another GameObject and saves its old slot and the cursor position. </summary>
    private void MoveStart(){
        string itemPattern = @"^Item\d{4}";
        moveSlot = ItemOnMouse(itemPattern);
        if (moveSlot != null && moveSlot.GetComponentInChildren<ItemReference>().ItemName != null){ // First check if there's an item slot and then if it stores an item
            move = true;
            mouseStartPos = Mouse.current.position.ReadValue();
            moveItem = moveSlot.GetComponentInChildren<ItemReference>().gameObject;
            startingPos = moveItem.GetComponent<RectTransform>().position;
            moveInventory = Parent.FindParent(moveSlot, typeof(InventoryCanvas))?.GetComponent<InventoryCanvas>().Inventory;
            moveItem.transform.SetParent(Parent.FindParentSibling(inventory.InventoryCanvas, "MoveHelper")); // Lower in the hierarchy means in the foreground. This image has to be in front of all other images
        }
    }

    /// <summary> The moved item follows the cursor. </summary>
    private void MoveProgress(){
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 newPosition = new(mouse.x - mouseStartPos.x + startingPos.x, mouse.y - mouseStartPos.y + startingPos.y, startingPos.z);
        moveItem.GetComponent<RectTransform>().position = newPosition;
    }

    /// <summary> Swaps the position of this item and the item which was before in this slot. If the other item has the same type as the
    /// new item it adds this stack to the other stack until it is full. The remaining amount is stored in the old slot. </summary>
    private void MoveEnd(){
        move = false;
        if (split){
            
        } else {
            string itemPattern = @"^Item\d{4}";
            moveItem.transform.SetParent(moveSlot.transform); // Reset parent to swap position
            GameObject newSlot = ItemOnMouse(itemPattern);
            if (newSlot == null){ // Outside the inventory
                RemoveStack(moveItem.GetComponent<ItemReference>());
            } else if (Parent.FindParent(newSlot, typeof(InventoryCanvas))?.GetComponent<InventoryCanvas>().Inventory == moveInventory){
                if (newSlot != moveSlot) { // Isn't the same slot as before
                    Vector2Int oldPosition = GetPositionFromName(moveSlot.name);
                    Vector2Int newPosition = GetPositionFromName(newSlot.name);
                    moveInventory.Move(oldPosition, newPosition);
                } else { // same slot as before, restore position
                    moveItem.transform.localPosition = positionInSlot;
                }
            } else {
                Vector2Int oldPosition = GetPositionFromName(moveSlot.name);
                Vector2Int newPosition = GetPositionFromName(newSlot.name);
                moveInventory.Move(oldPosition, newPosition, Parent.FindParent(newSlot, typeof(InventoryCanvas))?.GetComponent<InventoryCanvas>().Inventory);
            }
        }
    }

    public void OnUseItem(InputAction.CallbackContext context){
        if (context.started){
            if (move){
                return;
            }
            string itemPattern = @"^Item\d{4}";
            GameObject slot = ItemOnMouse(itemPattern);
            string itemName = slot?.GetComponentInChildren<ItemReference>().ItemName;
            if (slot != null && itemName != null){
                int amount = usage.UseItem(itemName);
                inventory.Remove(itemName, amount);
            }
        }
    }

    void Update(){
        if (move){
            MoveProgress();
        }
    }
    
    /// <summary> Stores if the player tries to move an item or if he wants to split the stack. 
    /// It tries to split while the player holds the button pressed. </summary>
    public void OnSplitItem(InputAction.CallbackContext context){
        if (context.started){
            split = true;
        }
        if (context.canceled){
            split = false;
        }
    }
}
