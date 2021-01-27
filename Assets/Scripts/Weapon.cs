using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon:MonoBehaviour {
    [SerializeField] Camera playerCamera;
    [SerializeField] Text ammoAmountText;
    [SerializeField] Text killAmountText;
    [SerializeField] int maxAmmoAmount;
    [SerializeField] float weaponRange;
    [SerializeField] float weaponDamage;
    [SerializeField] float weaponFireDelay;
    [SerializeField] float weaponReloadTime;
    [SerializeField] AudioClip gunshotClip;
    [SerializeField] AudioClip magEmptyClip;
    [SerializeField] AudioClip magReloadClip;

    private AudioSource audioSource;
    private float nextFireTime = 0f;
    private int ammoAmount = 0;
    private int killAmount = 0;
    private bool magEmptied;
    private bool canInteractWithWeapon = true;

    private void Start() {
        ammoAmount = maxAmmoAmount;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        ProcessInputs();
        UpdateGUIElements();
    }

    private void ProcessInputs() {
        if(Input.GetMouseButton(0)) {
            TryFireWeapon();
        }
        if(Input.GetMouseButtonDown(0)) {
            // if mag empty, play magEmptyClip
            if(ammoAmount == 0) {
                audioSource.PlayOneShot(magEmptyClip);
            }
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            TryReloadWeapon();
        }
    }

    private void TryReloadWeapon() {
        if(canInteractWithWeapon) {
            // reload
            ammoAmount = maxAmmoAmount;
            audioSource.PlayOneShot(magReloadClip);
            canInteractWithWeapon = false;
            Invoke("EnableFiring", weaponReloadTime);
        }
    }

    private void EnableFiring() {
        canInteractWithWeapon = true;
    }

    private void UpdateGUIElements() {
        ammoAmountText.text = ammoAmount + "/" + maxAmmoAmount;
        killAmountText.text = killAmount + " kills";
    }

    private void TryFireWeapon() {
        if(canInteractWithWeapon && Time.time > nextFireTime) {
            if(ammoAmount > 0) {
                // can fire
                ammoAmount -= 1;
                CheckTargetHits();
                audioSource.PlayOneShot(gunshotClip);
            } else {
                if(ammoAmount == 0 && !magEmptied) {
                    // audio notification of empty mag
                    audioSource.PlayOneShot(magEmptyClip);
                    magEmptied = true;
                }
            }
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
