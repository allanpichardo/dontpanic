using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PrisonLevelController : MonoBehaviour
{
    public List<GameObject> zombiePrefabs;

    public List<Transform> spawnPoints;

    public int spawnInterval = 8;

    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.FloorToInt(elapsedTime) % spawnInterval == 0)
        {
            elapsedTime = 0.0f;
            SpawnZombie();
        }
        elapsedTime += Time.deltaTime;
    }

    private void SpawnZombie()
    {
        int rnd = UnityEngine.Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[rnd];
        
        rnd = UnityEngine.Random.Range(0, zombiePrefabs.Count);
        GameObject zombie = zombiePrefabs[rnd];

        Instantiate(zombie, spawnPoint);
    }
}
