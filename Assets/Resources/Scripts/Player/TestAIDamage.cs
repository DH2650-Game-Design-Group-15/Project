using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIDamage : MonoBehaviour {

    public float shotSpeed;
    public float shotDelay;
    public float spread;
    public float bulletLife;
    public GameObject shellPrefab;
    private bool aiming;
    private float timeSinceShot;

    private void Start () {
        SetTimeSinceShot(0f);
    }

    private void Update () {
        SetTimeSinceShot(GetTimeSinceShot() + Time.deltaTime);
        if (GetTimeSinceShot() >= shotDelay) {
            Fire();
        }
    }

    /// <summary>
    /// Fires a bullet towards the player with a random spread
    /// </summary>
    private void Fire () {
        Vector3 direction = new Vector3(1, 0, 0);
        GameObject bullet = Instantiate(shellPrefab, transform.position, Quaternion.Euler(90,0,0));
        bullet.GetComponent<Rigidbody>().velocity = direction * shotSpeed;
        StartCoroutine(OnMiss(bulletLife, bullet));
        SetTimeSinceShot(0f);
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
}