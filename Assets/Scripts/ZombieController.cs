using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public Transform player;
    private static readonly int _speed = Animator.StringToHash("speed");
    private bool isDead = false;
    private static readonly int _distance = Animator.StringToHash("distance");

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            navMeshAgent.SetDestination(player.position);
            animator.SetFloat(_speed, navMeshAgent.velocity.magnitude);
            animator.SetFloat(_distance, Vector3.Distance(this.transform.position, player.position));
        }
    }

    public void SetDead(bool isDead)
    {
        this.isDead = isDead;
    }
}
