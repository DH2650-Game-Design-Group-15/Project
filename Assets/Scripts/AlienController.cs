using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    public Transform gunFront;
    public GameObject bullet;
    public float speed;

    Vector3 moveDirection;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void Fire()
    {
        GameObject newBullet = Instantiate(bullet, gunFront.position, gunFront.rotation);

    }
}