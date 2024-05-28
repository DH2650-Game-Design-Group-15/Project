using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour {

    public float laserActiveTime;
    public float laserRange;
    public float laserMaxCharge;
    public float damagePerSecond;
    public LayerMask targetMask;
    public HealthBar chargeSlider;
    public new Camera camera;
    private Transform origin;
    private float laserCharge;
    private LineRenderer laser;

    private void Awake () {
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
        chargeSlider.SetMaxHealth((int)(laserMaxCharge*100));
        SetLaserCharge(laserMaxCharge);
    }

    private void Start () {
        origin = transform.parent.transform;
    }

    private void Update () {
        if (laser.enabled && GetLaserCharge() > 0) {
            float time = Time.deltaTime;
            SetLaserCharge(GetLaserCharge() - time);
            RaycastHit hit = laserHit();
            if (hit.transform != null) {
                if (hit.transform.CompareTag("NPC")) {
                    float damage = damagePerSecond * time;
                    if (hit.collider.GetComponent<AIScoutHealth>() != null) {
                        hit.collider.GetComponent<AIScoutHealth>().TakeDamage(damage, transform.parent.parent.position, time);
                    }
                }
            }
        } else {
            laser.enabled = false;
            if (GetLaserCharge() < laserMaxCharge) {
                SetLaserCharge(GetLaserCharge() + Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Fires a bullet towards the player with a random spread
    /// </summary>
    public void Fire (InputAction.CallbackContext context) {
        if (context.started && GetLaserCharge() > 0) {
            laserHit();
            laser.enabled = true;
        }
        if (context.canceled) {
            laser.enabled = false;
        }
    }

    private RaycastHit laserHit () {
        laser.SetPosition(0, origin.position);
        Vector3 ray = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, camera.transform.forward, out hit, laserRange, targetMask)) {
            laser.SetPosition(1, hit.point);

        } else {
            laser.SetPosition(1, ray + (camera.transform.forward * laserRange));
        }
        return hit;
    }

    private void SetLaserCharge (float charge) {
        laserCharge = charge;
        int laserUI = (int)(charge*100);
        chargeSlider.SetHealth(laserUI);
    }

    private float GetLaserCharge () {
        return laserCharge;
    }

}
