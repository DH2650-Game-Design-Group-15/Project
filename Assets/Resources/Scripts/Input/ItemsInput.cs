using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemsInput : MonoBehaviour {
    public Inventory inventory;
    // Only for Move and Split 
    private bool move = false;
    private bool split = false;
    private GameObject moveItem;
    private GameObject moveSlot;
    private Vector2 mouseStartPos;
    private Vector3 startingPos;
    private static Vector3 positionInSlot = new(1, 0, 0);
    public Inventory moveInventory;
    private ObjectDetection objectDetection;
    private float pickUpDistance = 10f;

    void Start(){
        inventory = Parent.FindParent(gameObject, typeof(Inventory))?.GetComponent<Inventory>();
        objectDetection = Parent.FindChild(inventory, typeof(ObjectDetection))?.GetComponent<ObjectDetection>();
    }

    /// <summary> Called when the player tries to take an object. 
    /// If there're objects nearby a random item gets picked. If no object is nearby nothing happens.
    /// </summary>
    public void OnTakeItem(InputAction.CallbackContext context){
        if (context.started){
            GameObject[] objects = objectDetection.DetectObjects();                         // all objects nearby
            objects = ObjectDetection.ObjectsWithComponent(objects, typeof(Item));          // filter by items
            (GameObject obj, float distance) = objectDetection.ClosestObject(objects);      // filter closest one
            if (obj != null && distance < pickUpDistance) {
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
            Vector2Int position = GetPositionFromName(reference.transform.parent.gameObject.name);
            if (position.x < 0 || position.y < 0){
                return;
            }
            bool removed = GetInventory(reference.gameObject).RemoveStack(position);
            if (removed){
                reference.GetComponent<RawImage>().enabled = false;
                reference.GetComponent<RawImage>().texture = null;
            }
            // TODO Spawn Object
        }
    }

    public void OnMoveItem(InputAction.CallbackContext context){
        if (context.started){
            MoveStart();
        }
        if (context.canceled && move){
            MoveEnd();
        }
    }

    private void MoveStart(){
        string itemPattern = @"^Item\d{4}";
        moveSlot = ItemOnMouse(itemPattern);
        if (moveSlot != null && moveSlot.GetComponentInChildren<ItemReference>().ItemName != null){ // First check if there's an item slot and then if it stores an item
            move = true;
            mouseStartPos = Mouse.current.position.ReadValue();
            moveItem = moveSlot.GetComponentInChildren<ItemReference>().gameObject;
            startingPos = moveItem.GetComponent<RectTransform>().position;
            moveInventory = GetInventory(moveSlot);
            moveItem.transform.SetParent(Parent.FindParentSibling(inventory.InventoryCanvas, "MoveHelper")); // Lower in the hierarchy means in the foreground. This image has to be in front of all other images
        }
    }

    private void MoveProgress(){
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 newPosition = new(mouse.x - mouseStartPos.x + startingPos.x, mouse.y - mouseStartPos.y + startingPos.y, startingPos.z);
        moveItem.GetComponent<RectTransform>().position = newPosition;
    }

    private void MoveEnd(){
        move = false;
        if (split){
            
        } else {
            string itemPattern = @"^Item\d{4}";
            moveItem.transform.SetParent(moveSlot.transform); // Reset parent to swap position
            GameObject newSlot = ItemOnMouse(itemPattern);
            if (newSlot == null){ // Outside the inventory
                RemoveStack(moveItem.GetComponent<ItemReference>());
            } else if (GetInventory(newSlot) == moveInventory){
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
                moveInventory.Move(oldPosition, newPosition, GetInventory(newSlot));
            }
        }
    }

    void Update(){
        if (move){
            MoveProgress();
        }
    }

    /// <summary> Returns the Inventory component in a parent. </summary>
    /// <returns> parent Inventory. null if no inventory is found in the 5 next parents
    public Inventory GetInventory(GameObject game){
        Transform inventory = game.transform;
        for (int i = 0; i < 5; i++){
            InventoryCanvas result = inventory.GetComponent<InventoryCanvas>();
            if (result != null){
                return result.Inventory;
            }
            inventory = inventory.parent;
        }
        Debug.LogWarning("No Inventory found");
        return null;
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
