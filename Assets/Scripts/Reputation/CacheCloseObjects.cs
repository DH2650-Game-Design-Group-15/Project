using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This should be on a children component of a character.
/// It safes all other characters close to this, that makes it easier to find later the character closest to you to interact with
/// </summary>
public class CacheCloseObjects : MonoBehaviour {
    private readonly HashSet<GameObject> nearPlayers = new();
    private readonly HashSet<GameObject> nearItems = new();
    private bool hasChanged = false; // Only for debug required
    
    /// <summary>
    /// Checks if an object close to the player is another character or a collectable item. If so, it's added to the hashset.
    /// Every character must have a component Fractions, every item must have a component Item
    /// </summary>
    /// <param name="other"> Collider that enters this trigger </param>
    /// <returns></returns>
    public void OnTriggerEnter(Collider other){
        if (other.GetComponent<Fractions>() != null) {
            nearPlayers.Add(other.gameObject);
            hasChanged = true;
        } else if (other.GetComponent<Item>() != null){
            nearItems.Add(other.gameObject);
        }
    }

    /// <summary>
    /// Removes this object as a object close to the player, if it was stored as a close character or close item.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public void OnTriggerExit(Collider other) {
        if (other.GetComponent<Fractions>()) {
            nearPlayers.Remove(other.gameObject);
            hasChanged = true;
        } else if (other.GetComponent<Item>() != null){
            nearItems.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Prints a list of all characters which are close to the player
    /// </summary>
    /// <returns></returns>
    void DebugNearPlayers() {
        if (hasChanged) {
            hasChanged = false;
            string near = "";
            foreach (GameObject player in nearPlayers) {
                near += player.name + "\n";
            }
            Debug.Log(near);
        }
        
    }

    void Update() {
        DebugNearPlayers();
    }
    
    public HashSet<GameObject> GetNearPlayers() {
        return nearPlayers;
    }

    public HashSet<GameObject> GetNearItems() {
        return nearItems;
    }
}
