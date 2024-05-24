using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public GameObject doorObject;

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player")) {
            doorObject.GetComponent<DoorAnimation>().OpenDoor();
        }
    }
    
    private void OnTriggerExit (Collider other) {
        if (other.CompareTag("Player")) {
            doorObject.GetComponent<DoorAnimation>().CloseDoor();
        }
    }

}
