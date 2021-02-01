using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon:MonoBehaviour {
    [SerializeField] Camera playerCamera;
    [SerializeField] ParticleSystem muzzleSmoke;
    [SerializeField] ParticleSystem muzzleSparks;
    [SerializeField] GameObject flashlight;

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
    [SerializeField] AudioClip flashlightClip;

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
            if(ammoAmount == 0 && canInteractWithWeapon) {
                audioSource.PlayOneShot(magEmptyClip);
            }
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            TryReloadWeapon();
        }
        if(Input.GetMouseButtonDown(1)) {
            // toggle flashlight
            flashlight.SetActive(!flashlight.activeSelf);
            audioSource.PlayOneShot(flashlightClip);
        }
    }

    private void TryReloadWeapon() {
        if(canInteractWithWeapon) {
            // reload
            audioSource.PlayOneShot(magReloadClip);
            canInteractWithWeapon = false;
            Invoke("ReloadAmmoCount", weaponReloadTime);
        }
    }

    private void ReloadAmmoCount() {
        ammoAmount = maxAmmoAmount;
        canInteractWithWeapon = true;
    }

    private void UpdateGUIElements() {
        ammoAmountText.text = ammoAmount + "/" + maxAmmoAmount;
        killAmountText.text = killAmount + " kills";
    }

    private void TryFireWeapon() {
        if(canInteractWithWeapon && Time.time > nextFireTime) {
            if(ammoAmount > 0) {
                FireWeapon();
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

    private void FireWeapon() {
        GetComponent<Animator>().SetTrigger("Fire");
        ammoAmount -= 1;
        CheckTargetHits();
        WeaponFX();
    }

    private void WeaponFX() {
        audioSource.PlayOneShot(gunshotClip);
        muzzleSparks.Play();
        muzzleSmoke.Play();
    }

    private void CheckTargetHits() {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, weaponRange) && hit.transform.gameObject.CompareTag("Enemy")) {
            // enemy object hit, deal damage
            hit.transform.GetComponent<EnemyHealth>().TakeDamage(hit.point, weaponDamage);
        }
    }
}
