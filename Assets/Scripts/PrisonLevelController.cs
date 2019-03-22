using System;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public AudioClip introClip;
    public Text guidanceText;

    private List<float> phaseObservations;
    private float elapsedTime;
    private float phaseTime = 0.0f;
    public float spawnInterval = 10.0f;
    public Boolean startSpawn = false;
    public int trial = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        phaseObservations = new List<float>();
        flashlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashlight.enabled)
        {
            companion.StartTalking(introClip);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
        
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Pad) || Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = true;
            startSpawn = true;
            guidanceText.enabled = false;
        }
        
        if (startSpawn && phaseTime<61)
        {
            phaseTime +=Time.deltaTime;
            this.zombiePhase(phaseTime);

            if(phaseTime > 60)
            {
                Debug.Log("End of Zombie Phase " + trial);
                phaseTime = 0;
                startSpawn = false;
                companion.SetPhaseObservations(phaseObservations.ToArray());
                phaseObservations.Clear();
                trial = (trial < 2) ? trial + 1 : 0;
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
        //Debug.Log("Spawn Interval: " + spawnInterval + ", phaseTime: " + phaseTime + ", elapsedTime: " + elapsedTime);
        if (Math.Abs(elapsedTime - spawnInterval) < 0.1)
        {
            elapsedTime = 0.0f;
            SpawnZombie();
            spawnInterval = 1 + (30 - 10 * (trial)) * Mathf.Pow((float)Math.E, (float)(-0.05 * time));
        }
    }

    public override void OnNewPrediction(Vector3 inference)
    {
        flashlight.spotAngle = 40+(inference.x * -15);
        companion.SetValence(inference.x);

        if (startSpawn)
        {
            phaseObservations.Add(inference.x);
        }
    }
}
