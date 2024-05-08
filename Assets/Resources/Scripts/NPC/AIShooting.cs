using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIShooting : MonoBehaviour {

    public float shotSpeed;
    public float shotDelay;
    public float spread;
    public float bulletLife;
    public GameObject shellPrefab;
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
    /// Fires a bullet towards the player with a random spread
    /// </summary>
    private void Fire () {
        Vector3 direction = (GetScoutScript().GetFOVScript().GetPlayerPosition() - transform.position).normalized;
        direction += UnityEngine.Random.insideUnitSphere * spread;
        GameObject bullet = Instantiate(shellPrefab, transform.position, Quaternion.Euler(90,0,0));
        bullet.GetComponent<Rigidbody>().velocity = direction * shotSpeed;
        StartCoroutine(OnMiss(bulletLife, bullet));
        SetTimeSinceShot(0f);
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