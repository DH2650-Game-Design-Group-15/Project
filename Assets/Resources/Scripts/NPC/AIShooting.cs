using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIShooting : MonoBehaviour {

    public float shotDelay;
    public int damage;
    public int range;
    public LayerMask targetMask;
    private bool aiming;
    private float timeSinceShot;
    private AIScouting scoutScript;

    private void Start () {
        SetScoutScript(GetComponentInParent<AIScouting>());
        SetAiming(false);
        SetTimeSinceShot(0f);
    }

    private void Update () {
        SetTimeSinceShot(GetTimeSinceShot() + Time.deltaTime);
        if (GetScoutScript().GetShooting()) {
            if (GetAiming()) {
                if (GetTimeSinceShot() >= shotDelay) {
                    Fire();
                    SetTimeSinceShot(0f);
                }
            } else {
                GetScoutScript().GetAnimator().SetBool("Aiming", true);
                SetAiming(true);
            }
        } else if (GetAiming()) {
            GetScoutScript().GetAnimator().SetBool("Aiming", false);
            SetAiming(false);
        }
    }

    /// <summary>
    /// Fires at the player
    /// </summary>
    private void Fire () {
        Vector3 playerPos = GetScoutScript().GetFOVScript().GetPlayerPosition() + new Vector3(0,0.6f,0);
        Vector3 direction = (playerPos - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, range, targetMask)) {
            if (hit.collider.CompareTag("Player")) {
                hit.collider.GetComponent<PlayerHealth>().PlayerHit(damage);
            }
        }
        GetScoutScript().GetAnimator().SetTrigger("Shoot");
    }

    /// <summary>
    /// Destroys the gameobject after a certain time if it hasn't 
    /// already been destroyed
    /// </summary>
    /// <param name="time">Time to wait before destroying</param>
    /// <param name="bulletObject">GameObject to destroy</param>
    /// <returns></returns>
    private IEnumerator OnMiss (float time, GameObject bulletObject) {
        yield return new WaitForSeconds(time);
        if (bulletObject != null) {
            Destroy(bulletObject);
        }
    }

    private void SetAiming (bool aim) {
        aiming = aim;
    }

    private bool GetAiming () {
        return aiming;
    }

    private void SetTimeSinceShot (float time) {
        timeSinceShot = time;
    }

    private float GetTimeSinceShot () {
        return timeSinceShot;
    }

    private void SetScoutScript (AIScouting script) {
        scoutScript = script;
    }

    private AIScouting GetScoutScript () {
        return scoutScript;
    }

}