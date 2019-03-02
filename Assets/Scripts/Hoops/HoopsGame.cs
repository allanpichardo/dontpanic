using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoopsGame : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    public Text scoreText;

    private int score = 0;
    
    private void Awake()
    {
        Physics.gravity = new Vector3(0, -100, 0);
        ShouldSpawnNewBall();
    }

    private void Start()
    {
        scoreText.text = "Score: 0";
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void ShouldSpawnNewBall()
    {
        Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);
    }

    public void OnScoreMade()
    {
        score++;
    }
}
