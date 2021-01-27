using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon:MonoBehaviour {
    [SerializeField] Camera playerCamera;
    [SerializeField] float weaponRange;
    [SerializeField] float weaponDamage;
    [SerializeField] float weaponFireDelay;
    [SerializeField] AudioClip gunshotClip;

    private AudioSource audioSource;
    private float nextFireTime = 0f;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(Input.GetMouseButton(0)) {
            TryFireWeapon();
        }
    }

    private void TryFireWeapon() {
        if(Time.time > nextFireTime) {
            // can fire
            CheckTargetHits();
            audioSource.PlayOneShot(gunshotClip);
            nextFireTime = Time.time + weaponFireDelay;
        }

    }

    private void CheckTargetHits() {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, weaponRange) && hit.transform.name == "Enemy") {
            // enemy object hit, deal damage
            hit.transform.GetComponent<EnemyHealth>().TakeDamage(weaponDamage);
        }
    }
}
