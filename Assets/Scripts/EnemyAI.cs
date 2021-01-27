using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI:MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;

    private float distanceToTarget = Mathf.Infinity;
    private NavMeshAgent navMeshAgent;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        CheckTargetDistance();
    }

    private void CheckTargetDistance() {
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if(distanceToTarget <= chaseRange) {
            // Start heading towards target
            navMeshAgent.SetDestination(target.position);
        }

        if(distanceToTarget <= navMeshAgent.stoppingDistance) {
            // Attack target
            print("enemy attacked player");
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, chaseRange);
    }
}
