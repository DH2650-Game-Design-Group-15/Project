using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour {

    public float shotDelay;
    public float laserActiveTime;
    public float laserRange;
    public LayerMask playerMask;
    public new Camera camera;
    private Transform origin;
    private float timeSinceShot;
    private LineRenderer laser;

    private void Awake () {
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
        SetTimeSinceShot(0);
    }

    private void Start () {
        origin = transform.parent.transform;
    }

    private void Update () {
        SetTimeSinceShot(GetTimeSinceShot() + Time.deltaTime);
        if (laser.enabled) {
            laserHit();
        }
    }

    /// <summary>
    /// Fires a bullet towards the player with a random spread
    /// </summary>
    public void Fire (InputAction.CallbackContext context) {
        if (context.started && GetTimeSinceShot() > shotDelay) {
            laserHit();
            laser.enabled = true;
            StartCoroutine(ActivateLaser(laserActiveTime));
            /* Ray camRay = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit, bulletDistanceCheck, playerMask)) {
                Vector3 direction = hit.point - transform.position;
                Quaternion rotation = Quaternion.LookRotation(transform.forward);
                GameObject bullet = Instantiate(shellPrefab, transform.position, Quaternion.Euler(90,90,0));
                bullet.transform.rotation = rotation * Quaternion.Euler(90,90,0);
                bullet.transform.SetParent(transform);
                bullet.GetComponent<Rigidbody>().velocity = direction * shotSpeed;
                StartCoroutine(OnMiss(bulletLife, bullet));
            } */
            SetTimeSinceShot(0f);
        }
        if (context.canceled) {
            laser.enabled = false;
        }
    }

    private RaycastHit laserHit () {
        laser.SetPosition(0, origin.position);
        Vector3 ray = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, camera.transform.forward, out hit, laserRange, playerMask)) {
            laser.SetPosition(1, hit.point);

        } else {
            laser.SetPosition(1, ray + (camera.transform.forward * laserRange));
        }
        return hit;
    }

    private IEnumerator ActivateLaser (float time) {
        laser.enabled = true;
        yield return new WaitForSeconds(time);
        laser.enabled = false;
    }

    private void SetTimeSinceShot (float time) {
        timeSinceShot = time;
    }

    private float GetTimeSinceShot () {
        return timeSinceShot;
    }
}
