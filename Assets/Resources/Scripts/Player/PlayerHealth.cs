using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth;
    public HealthBar healthBar;
    private int health;
    private InventoryInput inventoryInput;

    private void Start () {
        inventoryInput = FindObjectOfType<InventoryInput>();
        SetHealth(startingHealth);
        healthBar.SetMaxHealth(startingHealth);
    }

    private void Update () {
        if (health <= 0) {
            inventoryInput.SetCursor(true);
            SceneManager.LoadScene("GameOver");
        }
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
