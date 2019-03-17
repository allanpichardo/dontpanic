using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainingPlayer : MonoBehaviour
{
//    private List<Trial> trials;
//    private Trial currentTrial;
    private Animator animator;
//    private float length;
    private float currentValence = -1.0f;
    private EmpathyAgent agent;

//    private void InitializeManifest()
//    {
//        TextAsset data = Resources.Load<TextAsset>("manifest");
//        trials = new List<Trial>();
//        
//        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data.text));  
//        DataContractJsonSerializer ser = new DataContractJsonSerializer(trials.GetType());  
//        
//        trials = ser.ReadObject(ms) as List<Trial>;  
//        ms.Close();
//    }

    private void Awake()
    {
//        InitializeManifest();
    }

//    public void LoadRandomTrial()
//    {
//        if (trials == null)
//        {
//            InitializeManifest();
//        }
//        int n = Random.Range(0, trials.Count);
//        currentTrial = trials[n];
//        String name = currentTrial.subject.ToString("00") + "_" + currentTrial.trial.ToString("00") + " (batch)";
//        Debug.Log(name);
//        int layer = animator.GetLayerIndex("Base Layer");
//        animator.Play(name, layer);
//        length = animator.GetCurrentAnimatorStateInfo(layer).length;
//    }

    private void Start()
    {
        animator = GetComponent<Animator>();
//        LoadRandomTrial();
    }


//    public Trial GetCurrentTrial()
//    {
//        return currentTrial;
//    }

    public void OnValence(float valence)
    {
        currentValence = valence;
    }

    public float GetCurrentValence()
    {
        return currentValence;
    }

    public void OnEnd()
    {
        currentValence = -1.0f;
    }

    public void OnReset()
    {
        agent.Done();
    }

    public void SetAgent(EmpathyAgent agent)
    {
        this.agent = agent;
    }

}
