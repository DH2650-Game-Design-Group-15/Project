using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class AIScouting : MonoBehaviour {
    public NavMeshAgent human;
    public float speed;
    public float maxDist;
    public float scoutRadius;
    public Vector3 basePosition;
    private Vector3 initPosition;
    private Vector3 prevDestination;

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

    public Vector3 RandomNavMeshLocation () {
        Vector3 finalPosition = GetInitPosition();
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < 30; i++){
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * maxDist;
            randomPosition += currentPosition;
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxDist, 1)) {
                Vector3 pathVec = hit.position - currentPosition;
                float angle = Vector3.Angle(human.transform.forward, pathVec);
                if (InsideScoutRadius(hit.position) && angle <= 90) {
                    finalPosition = hit.position;
                    break;
                } else {
                    finalPosition = currentPosition;
                }
            }
        }
        SetPrevDest(finalPosition);
        return finalPosition;
    }

    public bool InsideScoutRadius (Vector3 pos) {
        float dist = Vector3.Distance(pos, basePosition);
        if (dist <= scoutRadius) {
            return true;
        }
        return false;
    }

}