using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript_2 : MonoBehaviour
{
    public float Gate = 0.2f;
    public float speed = 2;
    public GameObject Target;

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * speed;
        float mouseY = Input.GetAxis("Mouse Y") * speed;
        Target.transform.localRotation = Target.transform.localRotation * Quaternion.Euler(0, mouseX, 0);
        if (transform.rotation.x <= -Gate | transform.rotation.x >= Gate)
        {
            transform.localRotation = transform.localRotation * Quaternion.Euler(mouseY, 0, 0);
        }
        else
        {
            transform.localRotation = transform.localRotation * Quaternion.Euler(-mouseY, 0, 0);
        }//限定旋转的角度，转多了该看的不该看的都看了
    }
}