using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private readonly int maxHealth = 100;
    public int startingHealth;
    public HealthBar healthBar;
    private int health;
    private InventoryInput inventoryInput;

    public int MaxHealth => maxHealth;

    private void Start () {
        inventoryInput = FindObjectOfType<InventoryInput>();
        healthBar.SetMaxHealth(MaxHealth);
        SetHealth(startingHealth);
    }

    private void Update () {
        if (health <= 0) {
            inventoryInput.SetCursor(true);
            SceneManager.LoadScene("GameOver");
        }
    }

    public void PlayerHit (int damage) {
        SetHealth(GetHealth() - damage);
        if (GetHealth() > maxHealth){
            SetHealth(MaxHealth);
        }
    }

    private void SetHealth (int hp) {
        health = hp;
        healthBar.SetHealth(GetHealth());
    }

    private int GetHealth () {
        return health;
    }

}
