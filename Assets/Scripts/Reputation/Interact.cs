using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    Fractions fractionSkript;
    DetectAI detectAISkript;
    public bool interact = false;

    // Start is called before the first frame update
    void Start()
    {
        fractionSkript = GetComponent<Fractions>();
        detectAISkript = GetComponentInChildren<DetectAI>();
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
        HashSet<Collider> others = detectAISkript.getNearPlayers();
        foreach (Collider other in others) {
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
