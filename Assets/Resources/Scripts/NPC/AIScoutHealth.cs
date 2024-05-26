using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScoutHealth : MonoBehaviour
{
    
    public int startHealth;
    public AIScouting scoutScript;
    private int health;
    
    private void Start () {
        SetHealth(startHealth);
    }

    public void TakeDamage (int damage) {
        if (GetHealth() > 0) {
            SetHealth(GetHealth() - damage);
            if (GetHealth() <= 0) {
                StartCoroutine(scoutScript.DeathAnimation(3.6f));
            }
        }
    }

    private void SetHealth (int hp) {
        health = hp;
    }

    private int GetHealth () {
        return health;
    }

}
