using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionController : HandsEmpathyAgent
{
    public float distanceThreshold = 5.0f;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private Animator animator;
    private static readonly int _speed = Animator.StringToHash("speed");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.position) > distanceThreshold)
        {
            navMeshAgent.SetDestination(player.position);
            animator.SetFloat(_speed, navMeshAgent.velocity.magnitude);
        }
    }

    public override void OnNewPrediction(Vector3 inference)
    {
       Debug.Log(inference); 
    }
}
