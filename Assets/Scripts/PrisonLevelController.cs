using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PrisonLevelController : HandsEmpathyAgent
{
    public List<GameObject> zombiePrefabs;
    public List<Transform> spawnPoints;
    public GameObject player;
    public Light flashlight;
    public Light godLight;
    public CompanionController companion;
    private Boolean toggleLight = false;

    private float elapsedTime;
    private float phaseTime = 0.0f;
    public float spawnInterval = 10.0f;
    public Boolean startSpawn = false;
    private int trial = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawn && phaseTime<61)
        {
            phaseTime+=Time.deltaTime;
            this.zombiePhase(phaseTime);

            if(phaseTime == 60)
            {
                phaseTime = 0;
                startSpawn = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            toggleLight = !toggleLight;
            godLight.intensity = (toggleLight)? 2.5f : 0;
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

    private void zombiePhase(float time)
    {
        //Exponential decay function
        elapsedTime += Time.deltaTime;

        if (Math.Abs(elapsedTime - spawnInterval) < 0.001)
        {
            elapsedTime = 0.0f;
            SpawnZombie();
            spawnInterval = (30 - 10 * (trial)) * Mathf.Pow((float)Math.E, (float)(-0.05 * time));
            Debug.Log("Spawn Interval: " + spawnInterval+", phaseTime: "+phaseTime);
        }
    }

    public override void OnNewPrediction(Vector3 inference)
    {
        flashlight.spotAngle = 40+(inference.x * -15);
        companion.SetValence(-inference.x);
    }
}
