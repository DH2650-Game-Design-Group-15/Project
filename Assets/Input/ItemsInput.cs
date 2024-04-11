using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemsInput : MonoBehaviour {
    private CacheCloseObjects closeObjectsScript;
    private PlayerInventory playerInventoryScript;

    void Start(){
        closeObjectsScript = GetComponentInChildren<CacheCloseObjects>();
        playerInventoryScript = GetComponent<PlayerInventory>();
    }

    public void OnTakeItem(InputAction.CallbackContext context){
        if (context.started){
            HashSet<GameObject> items = closeObjectsScript.GetNearItems();

            if (items.Count > 0){
                foreach (GameObject item in items) {
                    playerInventoryScript.Add(item);
                    closeObjectsScript.OnTriggerExit(item.GetComponent<Collider>());
                    Destroy(item);
                    break;
                }
            }
        }
    }
}
