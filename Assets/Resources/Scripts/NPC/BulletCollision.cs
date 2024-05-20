using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

    public int bulletDamage;

    private void OnCollisionEnter (Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.GetComponent<PlayerHealth>().PlayerHit(bulletDamage);
        }
        Debug.Log("TEST");
        Destroy(gameObject);  
    }

}