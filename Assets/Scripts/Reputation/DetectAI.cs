using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAI : MonoBehaviour {
    private HashSet<Collider> nearPlayers = new HashSet<Collider>();
    private bool hasChanged = false; // Only for debug required
    
    /// <summary>
    /// Adds AI players to the HashSet, if they enter this trigger
    /// The other players must have the tag "AI"
    /// </summary>
    /// <param name="other"> Collider that enters this trigger </param>
    /// <returns></returns>
    void OnTriggerEnter(Collider other){
        if (other.GetComponent<Fractions>() != null) {
            nearPlayers.Add(other);
            hasChanged = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    void OnTriggerExit(Collider other) {
        if (other.tag == "AI") {
            nearPlayers.Remove(other);
            hasChanged = true;
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
            foreach (Collider collider in nearPlayers) {
                near += collider.gameObject.name + "\n";
            }
            Debug.Log(near);
        }
        
    }

    void Update() {
        DebugNearPlayers();
    }
    
    public HashSet<Collider> getNearPlayers() {
        return nearPlayers;
    }
}
