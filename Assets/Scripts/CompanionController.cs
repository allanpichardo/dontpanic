using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;
using UnityEngine.AI;

public class CompanionController : MonoBehaviour
{
    public AudioClip neutralBreathing;
    public AudioClip scaredBreathing;
    public List<AudioClip> randomPhrases;
    public List<AudioClip> negativePhrases;
    public List<AudioClip> neutralPhrases;
    public List<AudioClip> positivePhrases;

    private bool isTalking;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private static readonly int _speed = Animator.StringToHash("speed");
    private static readonly int _valence = Animator.StringToHash("valence");

    private const float BreathingVolume = 0.891f;
    private const float TalkingVolume = 3.0f;

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
        if (obsArr.Length == 0) return;
        
        float sum = 0.0f;

        for (int i = 0; i < obsArr.Length; i++)
        {
            sum += obsArr[i];
        }

        float mean = sum / obsArr.Length;
        float std = CalculateSd(obsArr, mean);
        
        
        Debug.Log("Mean: " + mean+", Std: "+std);
        Debug.Log("Score: "+mean/0.5f);
        
        StartTalking(GetClipByScore(mean));
    }
    
    float CalculateSd(float[] data, float mean)
    {
        float standardDeviation = 0.0f;

        for (int i = 0; i < data.Length; ++i)
        {
            float diff = data[i] - mean;
            standardDeviation += diff * diff;
        }

        return Mathf.Sqrt(standardDeviation / data.Length);
    }

    public void StartTalking(AudioClip clip)
    {
        if (!isTalking)
        {
            StartCoroutine(Talk(clip));
        }
    }

    private AudioClip GetClipByScore(float score)
    {
        AudioClip clip;
        if (score > 0.66)
        {
            clip = negativePhrases[Random.Range(0, negativePhrases.Count)];
        }else if (score > 0.33 && score <= 0.66)
        {
            clip = neutralPhrases[Random.Range(0, neutralPhrases.Count)];
        }
        else
        {
            clip = positivePhrases[Random.Range(0, positivePhrases.Count)];
        }

        return clip;
    }

    private IEnumerator Talk(AudioClip clip)
    {
        isTalking = true;
        audioSource.Stop();
        audioSource.PlayOneShot(clip, TalkingVolume);
        yield return new WaitForSeconds(clip.length);
        isTalking = false;
    }

    public void SetValence(float valence)
    {
        float chance = Random.Range(0.0f, 1.0f);
        if (chance >= 0.999f)
        {
            StartTalking(randomPhrases[Random.Range(0, randomPhrases.Count)]);
        }
        if (!isTalking)
        {
            audioSource.volume = BreathingVolume;
            animator.SetFloat(_valence, valence);
            if (valence > 0.3f)
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

}
