using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner:MonoBehaviour {
    [SerializeField] float spawnDelay;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform target;

    private void Start() {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop() {
        while(true) {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.GetComponent<EnemyAI>().SetTarget(target);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
