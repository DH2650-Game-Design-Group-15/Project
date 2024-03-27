using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class AIScouting : MonoBehaviour {
    public NavMeshAgent human;
    public float speed;
    public float scoutRadius;

    private void Start () {
        human = GetComponent<NavMeshAgent>();
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

    public Vector3 RandomNavMeshLocation () {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * scoutRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, scoutRadius, 1)) {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

}