using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ScoutSpawner : MonoBehaviour {
    
    public int scoutLimit;
    public int initReputation;
    public float spawnTime;
    public GameObject scoutObject;
    private bool spawnStatus;
    private bool isPlayerCrouched;

    private void Start () {
        GetComponent<Fractions>().SetReputationToPlayer(initReputation);
        SetSpawnStatus(false);
        for (int i = 0; i < scoutLimit; i++) {
            SpawnScout();
        }
        SetSpawnStatus(true);
    }

    private void Update () {
        if (transform.childCount < scoutLimit && GetSpawnStatus()) {
            SetSpawnStatus(false);
            StartCoroutine(SpawnDelay(spawnTime));
        }
    }

    public void SetIsPlayerCrouched (InputAction.CallbackContext context) {
        if (context.started || context.performed) {
            isPlayerCrouched = true;
        }
        if (context.canceled) {
            isPlayerCrouched = false;
        }
    }

    public bool GetIsPlayerCrouched () {
        return isPlayerCrouched;
    }

    /// <summary>
    /// Spawns new scouts with a delay
    /// </summary>
    /// <param name="delay">Time in seconds to wait</param>
    /// <returns></returns>
    private IEnumerator SpawnDelay (float delay) {
        yield return new WaitForSeconds(delay);
        SpawnScout();
    }

    /// <summary>
    /// Spawns a scout in the scene as a child of ScoutSpawner
    /// </summary>
    private void SpawnScout () {
        GameObject newScout = Instantiate(scoutObject, transform.position, Quaternion.identity);
        newScout.transform.SetParent(transform);
        newScout.GetComponent<AIScouting>().basePosition = transform.position;
    }

    private void SetSpawnStatus (bool status) {
        spawnStatus = status;
    }

    private bool GetSpawnStatus () {
        return spawnStatus;
    }

}