using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth:MonoBehaviour {
    [SerializeField] float health;
    [SerializeField] ParticleSystem bloodFX;

    public void TakeDamage(Vector3 damageLocation, float damage) {
        health -= damage;
        bloodFX.transform.position = damageLocation;
        bloodFX.Play();
        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
