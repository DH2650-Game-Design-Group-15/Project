using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScoutHealth : MonoBehaviour
{
    
    public int repLossPerSecondOnHit;
    public int startHealth;
    public AIScouting scoutScript;
    private float health;
    
    private void Start () {
        SetHealth(startHealth);
    }

    public void TakeDamage (float damage, Vector3 playerPosition, float time) {
        if (GetHealth() > 0) {
            SetHealth(GetHealth() - damage);
            GetComponentInParent<Fractions>().SetReputationToPlayer(-(repLossPerSecondOnHit * time));
            GetComponent<AIScouting>().Hit(playerPosition);
            if (GetHealth() <= 0) {
                StartCoroutine(scoutScript.DeathAnimation(3.6f));
            }
        }
    }

    private void SetHealth (float hp) {
        health = hp;
    }

    private float GetHealth () {
        return health;
    }

}
