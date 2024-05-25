using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DoorAnimation : MonoBehaviour {

    public float rotation;
    public float rotationTime;
    private bool isOpen;
    private bool isRotating;
    private Quaternion closed;
    private Quaternion open;

    private void Start() {
        SetIsOpen(false);
        SetClosed(transform.rotation);
        SetOpen(transform.rotation * Quaternion.Euler(0, 0, rotation));
    }

    private void Update() {
        if (!GetIsRotating()) {
            if (GetIsOpen() && GetOpen() != transform.rotation) {
                OpenDoor();
            } else if (!GetIsOpen() && GetClosed() != transform.rotation) {
                CloseDoor();
            }
        }
    }

    public void OpenDoor () {
        if (!GetIsRotating()) {
            StartCoroutine(RotateDoor(GetOpen(), rotationTime));
        }
        SetIsOpen(true);
    }

    public void CloseDoor () {
        if (!GetIsRotating()) {
            StartCoroutine(RotateDoor(GetClosed(), rotationTime));
        }
        SetIsOpen(false);
    }

    private IEnumerator RotateDoor (Quaternion end, float rotationTime) {
        SetIsRotating(true);
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = end;
        float time = 0f;
        while (time < rotationTime) {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            time += Time.deltaTime;
            yield return null;
        }
        SetIsRotating(false);
    }

    private void SetIsOpen (bool status) {
        isOpen = status;
    }

    public bool GetIsOpen () {
        return isOpen;
    }

    private void SetIsRotating (bool rotation) {
        isRotating = rotation;
    }

    private bool GetIsRotating () {
        return isRotating;
    }

    private void SetClosed (Quaternion quaternion) {
        closed = quaternion;
    }

    private Quaternion GetClosed () {
        return closed;
    }

    private void SetOpen (Quaternion quaternion) {
        open = quaternion;
    }

    private Quaternion GetOpen () {
        return open;
    }

}
