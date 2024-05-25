using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScoutHealth : MonoBehaviour
{
    
    public int repLossOnHit;
    public int startHealth;
    public AIScouting scoutScript;
    private int health;
    
    private void Start () {
        SetHealth(startHealth);
    }

    public void TakeDamage (int damage, Vector3 playerPosition) {
        if (GetHealth() > 0) {
            SetHealth(GetHealth() - damage);
            GetComponentInParent<Fractions>().SetReputationToPlayer(repLossOnHit);
            GetComponent<AIScouting>().Hit(playerPosition);
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
