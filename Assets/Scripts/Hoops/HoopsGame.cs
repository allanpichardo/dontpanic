using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopsGame : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;

    private int score = 0;
    
    private void Awake()
    {
        Physics.gravity = new Vector3(0, -100, 0);
        ShouldSpawnNewBall();
    }

    public void ShouldSpawnNewBall()
    {
        Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);
    }

    public void OnScoreMade()
    {
        score++;
        ShouldSpawnNewBall();
    }
}
