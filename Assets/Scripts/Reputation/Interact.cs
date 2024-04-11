using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A test script to test the fraction behaviour
/// </summary>
public class Interact : MonoBehaviour
{
    Fractions fractionSkript;
    CacheCloseObjects closeObjectsSkript;
    public bool interact = false;

    // Start is called before the first frame update
    void Start()
    {
        fractionSkript = GetComponent<Fractions>();
        closeObjectsSkript = GetComponentInChildren<CacheCloseObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interact) {
            interact = false;
            doSomething();
        }
    }

    void doSomething(){
        HashSet<GameObject> others = closeObjectsSkript.GetNearPlayers();
        foreach (GameObject other in others) {
            Fractions otherFraction = other.GetComponent<Fractions>();
            if (otherFraction != null) {
                int reputation = fractionSkript.getReputation(otherFraction.getFraction());
                if (reputation < -20) {
                    Debug.Log("It's an enemy");
                } else if (reputation < 20) {
                    Debug.Log("He's neutral");
                } else {
                    Debug.Log("He's friendly");
                }
            } else {
                Debug.Log("Character has no fraction");
            }
        }
    }
}
