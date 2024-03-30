using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{

    public float speed = 10f;
    public float lifetime = 2f;

    [HideInInspector]
    public AlienController player;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Hit(100);
        }
        gameObject.SetActive(false); // Disable the bullet
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false); // Disable the bullet
    }
}
