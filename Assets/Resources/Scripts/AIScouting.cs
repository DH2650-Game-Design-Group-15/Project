using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class AIScouting : MonoBehaviour {
    public NavMeshAgent human;
    public float probAmplifier;
    public float speed;
    public float maxDist;
    public float scoutRadius;
    public float idleRadius;
    private float timerStart;
    private int areaCounter;
    private bool wait;
    private bool dialogue;
    public Vector3 basePosition;
    private Vector3 initPosition;
    private Vector3 prevPosition;
    private Vector3 currAreaCenter;
    private Quaternion prevRotation;
    private Coroutine idleTimer;

    private void Start () {
        SetDialogue(false);
        human = GetComponent<NavMeshAgent>();
        SetInitPosition(human.transform.position);
        SetPrevPosition(human.transform.position);
        if (human != null) {
            human.speed = speed;
            human.SetDestination(RandomNavMeshLocation());
        }
    }

    private void Update () {
        if (GetDialogue() == true) {
            human.isStopped = true;
        } else if (human != null) {
            if (human.isStopped) {
                human.transform.position = GetPrevPosition();
                human.transform.rotation = GetPrevRotation();
            }
            if (human.remainingDistance <= human.stoppingDistance) {
                if (!GetWait()){
                    SetWait(true);
                    StartCoroutine(waitTimer(UnityEngine.Random.Range(1.5f,4.0f)));
                }
            } else if (float.IsInfinity(human.remainingDistance)) {
                human.isStopped = false;
                human.SetDestination(RandomNavMeshLocation());
            }
        }
        SetPrevPosition(human.transform.position);
        SetPrevRotation(human.transform.rotation);
    }

    /// <summary>
    /// Generates a random position on the NavMesh and makes sure that the position is
    /// inside the scouting radius and that the direction change doesn't exceed a 90
    /// degree angle
    /// </summary>
    /// <returns>Random position on the NavMesh</returns>
    private Vector3 RandomNavMeshLocation () {
        Vector3 finalPosition = GetInitPosition();
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < 30; i++){
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * maxDist;
            randomPosition += currentPosition;
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxDist, 1)) {
                Vector3 pathVec = hit.position - currentPosition;
                float angle = Vector3.Angle(human.transform.forward, pathVec);
                if (InsideRadius(hit.position, scoutRadius) && angle <= 90) {
                    if (!NextArea(hit.position)) {
                        finalPosition = hit.position;
                        break;
                    }
                } else {
                    finalPosition = currentPosition;
                }
            }
        }
        return finalPosition;
    }

    /// <summary>
    /// Checks if a position is inside the given radius
    /// </summary>
    /// <param name="pos">The position to check</param>
    /// <returns>boolean</returns>
    private bool InsideRadius (Vector3 pos, float radius) {
        float dist = Vector3.Distance(pos, basePosition);
        if (dist <= radius) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Decides if npc should visit next area and increasing
    /// the probability of visiting the next area each time
    /// the function is called. If new area is visited, the
    /// areaCenter is updated
    /// </summary>
    /// <param name="pos">Position of next destination</param>
    /// <returns>boolean</returns>
    private bool NextArea (Vector3 pos) {
        if (GetCurrAreaCenter() != null) {
            if (InsideRadius(pos, idleRadius)) {
                float p = GetAreaCounter() * probAmplifier;
                if (p > 1) {
                    return true;
                }
                if (UnityEngine.Random.Range(0.0f, 1.0f) <= p) {
                    return true;
                }
                SetAreaCounter(GetAreaCounter() + 1);
                return false;
            }
        }
        SetCurrAreaCenter(pos);
        SetAreaCounter(1);
        return false;
    }

    /// <summary>
    /// Stops the AI for timeToWait seconds and then gives
    /// it a new destination to walk to
    /// </summary>
    /// <param name="timeToWait">The time the AI should wait
    /// at a location</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator waitTimer (float timeToWait) {
        human.isStopped = true;
        yield return new WaitForSeconds(timeToWait);
        SetWait(false);
        human.isStopped = false;
        human.SetDestination(RandomNavMeshLocation());
    }
    
    private void SetAreaCounter (int count) {
        areaCounter = count;
    }

    private int GetAreaCounter () {
        return areaCounter;
    }

    private void SetWait (bool b) {
        wait = b;
    }

    private bool GetWait () {
        return wait;
    }

    private void SetDialogue (bool status) {
        dialogue = status;
    }

    private bool GetDialogue () {
        return dialogue;
    }

    private void SetInitPosition (Vector3 pos) {
        initPosition = pos;
    }

    private Vector3 GetInitPosition () {
        return initPosition;
    }

    private void SetCurrAreaCenter (Vector3 areaCenter) {
        currAreaCenter = areaCenter;
    }

    private Vector3 GetCurrAreaCenter () {
        return currAreaCenter;
    }

    private void SetPrevPosition (Vector3 pos) {
        prevPosition = pos;
    }

    private Vector3 GetPrevPosition () {
        return prevPosition;
    }

    private void SetPrevRotation (Quaternion rot) {
        prevRotation = rot;
    }

    private Quaternion GetPrevRotation () {
        return prevRotation;
    }

}