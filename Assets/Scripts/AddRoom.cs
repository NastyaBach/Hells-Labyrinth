using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Doors")]
    public GameObject[] doors;
    public GameObject doorEffect;
    public GameObject lockedDoor;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public GameObject bossType;
    public Transform[] enemySpawners;
    public Transform bossSpawner;

    [Header("PowerUps")]
    public GameObject shield;
    public GameObject healthPotion;

    [HideInInspector] public List<GameObject> enemies;

    private bool spawned;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;

            foreach(Transform spawner in enemySpawners)
            {
                int rand = Random.Range(0, 11);
                if (rand < 9)
                {
                    GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                }
                else if (rand == 9)
                {
                    Instantiate(healthPotion, spawner.position, Quaternion.identity);
                }
                else if (rand == 10)
                {
                    Instantiate(shield, spawner.position, Quaternion.identity);
                }
            }

            if (bossSpawner != null)
            {
                GameObject boss = Instantiate(bossType, bossSpawner.position, Quaternion.identity) as GameObject;
                boss.transform.parent = transform;
                enemies.Add(boss);
            }

            StartCoroutine(CheckEnemies());
        }
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        DestroyDoors();
    }

    public void DestroyDoors()
    {
        foreach(GameObject door in doors)
        {
            if(door != null && door.transform.childCount != 0)
            {
                Instantiate(doorEffect, door.transform.position, Quaternion.identity);
                Destroy(door);
            }
        }
    }

}
