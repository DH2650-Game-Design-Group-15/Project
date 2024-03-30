using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public void Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Perform any death effects or cleanup here
        Destroy(gameObject);
    }
}
