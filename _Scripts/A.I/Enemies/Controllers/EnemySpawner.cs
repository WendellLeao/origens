using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPos;
    //[SerializeField] private float timeToSpawn;
    [SerializeField] private int howManyEnemiesCanSpawn;
    private Transform bbPos;
    private GameObject enemyInstance;
    private float startTimeToSpawn;
    private bool canStart = false;
    private bool canSpawn = false;
    private event Action onPlayerEnterTrigger;

    void Start()
    {
        onPlayerEnterTrigger += StartSpawner;
        //startTimeToSpawn = timeToSpawn;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    void Update()
    {
        if(canStart)
        {
            if(howManyEnemiesCanSpawn > 0)
            {
                if(enemyInstance == null)
                {
                    howManyEnemiesCanSpawn--;
                    canSpawn = true;
                }
                
                if(canSpawn)
                {
                    enemyInstance = Instantiate(enemyPrefab, spawnPos.position, spawnPos.rotation);
                    //timeToSpawn = startTimeToSpawn;
                    canSpawn = false;
                }

            }
            else
            {
                StartCoroutine("TimeToClosePortal");
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        MovementWithKeyboard movement = other.GetComponent<MovementWithKeyboard>();

        if(movement != null)
            onPlayerEnterTrigger?.Invoke();
    }

    private void StartSpawner()
    {
        StartCoroutine("TimeToStart");
        //timeToSpawn = 1f;

        GetComponent<AnimationHandler>().PlayAnimation("OpenPortalAnim");

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator TimeToStart()
    {
        yield return new WaitForSeconds(0.6f);
        canStart = true;
    }

    IEnumerator TimeToClosePortal()
    {
        yield return new WaitForSeconds(0.6f);
        GetComponent<AnimationHandler>().PlayAnimation("ClosePortalAnim");
        Destroy(gameObject, 0.5f);
    }
    void OnDisable()
    {
        onPlayerEnterTrigger -= StartSpawner;
    }

    /*private void SpawnTimer()
    {
        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0f)
        {
            canSpawn = true;
            timeToSpawn = 0f;
        }
    }*/
}
