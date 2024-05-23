using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth;
    public HealthBar healthBar;
    private int health;

    private void Start () {
        SetHealth(startingHealth);
        healthBar.SetMaxHealth(startingHealth);
    }

    public void PlayerHit (int damage) {
        SetHealth(GetHealth() - damage);
        healthBar.SetHealth(GetHealth());
    }

    private void SetHealth (int hp) {
        health = hp;
    }

    private int GetHealth () {
        return health;
    }

}
