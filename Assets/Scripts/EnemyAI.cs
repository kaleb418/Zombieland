﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI:MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;
    [SerializeField] AudioClip moanClip;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip awakenClip;
    [SerializeField] float attackDelay;

    private AudioSource audioSource;
    private bool zomboiEngaged = false;
    private float distanceToTarget = Mathf.Infinity;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime = 0f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        CheckTargetDistance();
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }

    private void CheckTargetDistance() {
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if(distanceToTarget <= chaseRange) {
            // Start heading towards target
            zomboiEngaged = true;
            navMeshAgent.SetDestination(target.position);
            if(!zomboiEngaged) {
                audioSource.PlayOneShot(awakenClip);
                zomboiEngaged = true;
            }
        }

        if(distanceToTarget <= navMeshAgent.stoppingDistance && Time.time > nextAttackTime) {
            // Attack target
            // TODO Add attack mechanics
            audioSource.PlayOneShot(attackClip);
            nextAttackTime = Time.time + attackDelay;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, chaseRange);
    }
}
