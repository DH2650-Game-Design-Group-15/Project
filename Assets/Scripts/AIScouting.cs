using System;
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
    private int areaCounter;
    public Vector3 basePosition;
    private Vector3 initPosition;
    private Vector3 prevDestination;
    private Vector3 currAreaCenter;

    private void Start () {
        human = GetComponent<NavMeshAgent>();
        SetInitPosition(human.transform.position);
        if (human != null) {
            human.speed = speed;
            human.SetDestination(RandomNavMeshLocation());
        }
    }

    private void Update () {
        if (human != null && human.remainingDistance <= human.stoppingDistance) {
            human.SetDestination(RandomNavMeshLocation());
        }
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
        SetPrevDest(finalPosition);
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
    
    private void SetAreaCounter (int count) {
        areaCounter = count;
    }

    private int GetAreaCounter () {
        return areaCounter;
    }

    private void SetInitPosition (Vector3 pos) {
        initPosition = pos;
    }

    private Vector3 GetInitPosition () {
        return initPosition;
    }

    private void SetPrevDest (Vector3 dest) {
        prevDestination = dest;
    }

    private Vector3 GetPrevDest () {
        return prevDestination;
    }

    private void SetCurrAreaCenter (Vector3 areaCenter) {
        currAreaCenter = areaCenter;
    }

    private Vector3 GetCurrAreaCenter () {
        return currAreaCenter;
    }

}