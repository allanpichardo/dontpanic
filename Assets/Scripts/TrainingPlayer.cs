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
    private List<Trial> trials;
    private Trial currentTrial;
    private Animator animator;
    public Agent agent;
    private float length;
    
    private void InitializeManifest()
    {
        TextAsset data = Resources.Load<TextAsset>("manifest");
        trials = new List<Trial>();
        
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data.text));  
        DataContractJsonSerializer ser = new DataContractJsonSerializer(trials.GetType());  
        
        trials = ser.ReadObject(ms) as List<Trial>;  
        ms.Close();
    }

    public void LoadRandomTrial()
    {
        int n = Random.Range(0, trials.Count);
        currentTrial = trials[n];
//        animator.SetInteger("subject", currentTrial.subject);
//        animator.SetInteger("trial", currentTrial.trial);
        String name = currentTrial.subject.ToString("00") + "_" + currentTrial.trial.ToString("00") + " (batch)";
        Debug.Log(name);
        int layer = animator.GetLayerIndex("Base Layer");
        animator.Play(name, layer);
        length = animator.GetCurrentAnimatorStateInfo(layer).length;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        InitializeManifest();
        LoadRandomTrial();
    }

    public Trial GetCurrentTrial()
    {
        return currentTrial;
    }

    public void OnAnimationEnd()
    {
        agent.Done();
    }

    public float getLength()
    {
        return length;
    }
}
