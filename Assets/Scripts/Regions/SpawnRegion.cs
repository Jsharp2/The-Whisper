using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegion : MonoBehaviour
{
    public List<GameObject> overworldEnemies = new List<GameObject>();

    public float minSpawnRate = 5f;
    public float maxSpawnRate = 10f;
    float nextSpawn;

    float time;

    public float distance = 15f;
    public int maxSpawns = 5;

    public bool spawning = true;
    // Start is called before the first frame update
    void Start()
    {
        nextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)
        {
            time += Time.deltaTime;
            if (time >= nextSpawn)
            {
                time = 0;
                int count = 0;
                GameObject[] allSpawned = GameObject.FindGameObjectsWithTag("Spawned Enemies");
                foreach (GameObject enemy in allSpawned)
                {
                    if (enemy.GetComponent<OverworldEnemy>().home = gameObject.GetComponent<SpawnRegion>())
                    {
                        count++;
                    }
                }
                if (count < maxSpawns)
                {
                    SpawnEnemy();
                }
            }
        }
        float xDiff = Mathf.Pow((transform.position.x - GameObject.Find("Player").transform.position.x), 2);
        float yDiff = Mathf.Pow((transform.position.y - GameObject.Find("Player").transform.position.y), 2);
        if (Mathf.Sqrt(xDiff + yDiff) > distance)
        {
            spawning = false;
        }
        else if(!spawning)
        {
            spawning = true;
            SpawnEnemy();
            time = 0;
        }
    }

    void SpawnEnemy()
    {
        Vector3 renderer = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 transform = gameObject.GetComponent<Transform>().position;
        GameObject enemy = Instantiate(overworldEnemies[Random.Range(0, overworldEnemies.Count)], new Vector3(Random.Range(transform.x - (renderer.x/2), transform.x + (renderer.x / 2)), Random.Range(transform.y - (renderer.y / 2), transform.y + (renderer.y / 2)), 0), Quaternion.identity) as GameObject;
        enemy.GetComponent<OverworldEnemy>().home = gameObject.GetComponent<SpawnRegion>();
        DontDestroyOnLoad(enemy);
    }
}
