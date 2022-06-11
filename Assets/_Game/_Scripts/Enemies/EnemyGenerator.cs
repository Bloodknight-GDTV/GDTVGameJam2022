using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List<GameObject> enemies;
//(Add to Start()) 
// GameObject spawner = GameObject.FindGameObjectWithTag("EnemySpawn");
// enemyspawns = spawner.FindGameObjectsWithTag("Enemy");
// enemies = new List<GameObject>();

public class EnemyGenerator : MonoBehaviour
{
    private GameObject[] enemyspawns;
    [SerializeField] private GameObject enemy;

    private float spawnDelay = 0.5f;

    void Start()
    {
        enemyspawns = GameObject.FindGameObjectsWithTag("Enemy");

        StartCoroutine(SpawnEnemy(spawnDelay));
    }

    IEnumerator SpawnEnemy(float delay)
    {
        foreach (var spawnPoint in enemyspawns)
        {
            Vector3 pos = spawnPoint.transform.position;
            Instantiate(enemy, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
    }
}
