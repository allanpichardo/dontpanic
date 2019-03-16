using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionController : MonoBehaviour
{
    public AudioClip neutralBreathing;
    public AudioClip scaredBreathing;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private static readonly int _speed = Animator.StringToHash("speed");
    private static readonly int _valence = Animator.StringToHash("valence");

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(player.position);
        animator.SetFloat(_speed, navMeshAgent.velocity.magnitude);
    }

    public void SetPhaseObservations(float[] obsArr)
    {
        float min = Mathf.Min(obsArr);
        float max = Mathf.Max(obsArr);
        float sum = 0.0f;

        for (int i = 0; i < obsArr.Length; i++)
        {
            sum += obsArr[i];
        }

        float mean = sum / obsArr.Length;

        float normalized = (mean - min) / (max - min);
        
        Debug.Log("Phase score: " + normalized);
    }

    public void SetValence(float valence)
    {
        animator.SetFloat(_valence, valence);
        if (valence < 0)
        {
            if (!audioSource.clip.Equals(scaredBreathing))
            {
                audioSource.clip = scaredBreathing;
                audioSource.Play();
            }
        }
        else
        {
            if (!audioSource.clip.Equals(neutralBreathing))
            {
                audioSource.clip = neutralBreathing;
                audioSource.Play();
            }
        }
        
    }

}
