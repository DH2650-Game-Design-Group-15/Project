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

    private void Start () {
        human = GetComponent<NavMeshAgent>();
        setInitPosition(human.transform.position);
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

    public void setInitPosition (Vector3 pos) {
        initPosition = pos;
    }

    public Vector3 getInitPosition () {
        return initPosition;
    }

    public Vector3 RandomNavMeshLocation () {
        Vector3 finalPosition = getInitPosition();
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < 30; i++){
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * maxDist;
            randomPosition += currentPosition;
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxDist, 1)) {
                if (InsideScoutRadius(hit.position)) {
                    finalPosition = hit.position;
                    break;
                } else {
                    finalPosition = currentPosition;
                }
            }
        }
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