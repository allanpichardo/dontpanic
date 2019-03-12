using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PrisonLevelController : HandsEmpathyAgent
{
    public List<GameObject> zombiePrefabs;
    public List<Transform> spawnPoints;
    public GameObject player;
    public float spawnInterval = 10.0f;
    public Light flashlight;
    public CompanionController companion;

    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if (Math.Abs(elapsedTime - spawnInterval) < 0.001)
        {
            elapsedTime = 0.0f;
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        int rnd = UnityEngine.Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[rnd];
        
        rnd = UnityEngine.Random.Range(0, zombiePrefabs.Count);
        GameObject zombie = zombiePrefabs[rnd];

        zombie.GetComponent<ZombieController>().player = player.transform;

        Instantiate(zombie, spawnPoint);
    }

    public override void OnNewPrediction(Vector3 inference)
    {
        flashlight.spotAngle = 40+(inference.x * -15);
        companion.SetValence(-inference.x);
    }
}
