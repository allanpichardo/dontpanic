using System;
using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;
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
    public AudioSource heartbeat;

    private List<float> phaseObservations;
    private float elapsedTime;
    private float phaseTime = 0.0f;
    public float spawnInterval = 10.0f;
    public Boolean startSpawn = false;
    public int trial = 0;

    public UILineRenderer lineRenderer;
    private List<Vector2> toGraph;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        phaseObservations = new List<float>();
        toGraph = new List<Vector2>();
        flashlight.enabled = false;
        StartCoroutine(didYouHear());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.Done();
            SceneManager.UnloadSceneAsync(1);
            SceneManager.LoadScene (0);
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
                Debug.Log("End of Zombie Phase " + trial+1);
                phaseTime = 0;
                startSpawn = false;
                trial = (trial < 2) ? trial + 1 : 0;
                StartCoroutine(ShowSummary());
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            toggleLight = !toggleLight;
            godLight.intensity = (toggleLight)? 2.5f : 0;
        }
        
        
    }

    IEnumerator ShowSummary()
    {
        guidanceText.text = "Phase " + trial + " Complete";
        guidanceText.enabled = true;
        lineRenderer.enabled = true;
        companion.SetPhaseObservations(phaseObservations.ToArray());
        int i = 0;
        for (int k = 0; k < phaseObservations.Count; k += 10)
        {
            toGraph.Add(new Vector2(++i, phaseObservations[k]*0.66f));
        }
        lineRenderer.Points = toGraph.ToArray();
        phaseObservations.Clear();
        toGraph.Clear();
        yield return new WaitForSeconds(10);
        guidanceText.enabled = false;
        lineRenderer.enabled = false;
        this.startSpawn = true;

    }

    IEnumerator didYouHear()
    {
        yield return new WaitForSeconds(3);
        companion.StartTalking(introClip);
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
        flashlight.spotAngle = 40+(inference.x * -38);
        flashlight.intensity = 1.45f + (inference.x * -0.75f);
        heartbeat.pitch = Mathf.Lerp(0.85f, 1.5f, inference.x);
        companion.SetValence(inference.x);

        if (startSpawn)
        {
            phaseObservations.Add(inference.x);
        }
    }
}
