using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public Transform player;
    private static readonly int _speed = Animator.StringToHash("speed");
    private bool isDead = false;
    private static readonly int _distance = Animator.StringToHash("distance");
    private static readonly int _isDead = Animator.StringToHash("isDead");

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        navMeshAgent.ResetPath();
        animator.SetBool(_isDead, isDead);
        if (isDead)
        {
            audioSource.Stop();
        }
    }

    public void OnZombieAttack()
    {
        
    }

    public void OnDeath()
    {
        //Destroy(this.gameObject);
    }
}
