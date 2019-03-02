using System;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimedEmpathyAgent : Agent
{
    public float maxTimePerEpisode = 10.0f;
    public Reader playbackReader;
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    public Text expectedText;
    [FormerlySerializedAs("actualText")] public Text guessText;

    private TransformNormalizer transformNormalizerLeft;
    private TransformNormalizer transformNormalizerRight;
    private Material material;
    private float time;
    
    public override void InitializeAgent()
    {
        transformNormalizerLeft = leftHand.GetComponent<TransformNormalizer>();
        transformNormalizerRight = rightHand.GetComponent<TransformNormalizer>();
        
        material = GetComponent<MeshRenderer>().material;
        material.color = Color.white;

        playbackReader.agent = this;
    }

    public override void CollectObservations()
    {
        AddVectorObs(transformNormalizerLeft.GetNormalizedPosition(leftHand.transform));
        AddVectorObs(transformNormalizerLeft.GetNormalizedRotation(leftHand.transform));
        AddVectorObs(transformNormalizerRight.GetNormalizedPosition(rightHand.transform));
        AddVectorObs(transformNormalizerRight.GetNormalizedRotation(rightHand.transform));
    }

    private float CalculateReward(float predicted, float actual)
    {
        float absDistance = Mathf.Abs(predicted - actual);
        double sigmoid = (1 - (2.16395  * Math.Tanh(absDistance)));
        return (float) sigmoid;
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float action = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);
        float reward = CalculateReward(action, playbackReader.GetCurrentState());
        Debug.Log("Reward: "+reward);
        SetReward(reward);
        
        if (reward > 0)
        {
            material.color = new Color(0,reward,0);
        }
        else if(reward < 0)
        {
            material.color = new Color(Mathf.Abs(reward), 0, 0);
        }
        else
        {
            material.color = Color.white;
        }
        
        expectedText.text = "Expected: "+playbackReader.GetCurrentState();
        guessText.text = "Guess: "+action;

        if (time > maxTimePerEpisode)
        {
            Done();
        }
    }

    public override void AgentReset()
    {
        time = 0;
        material.color = Color.white;
        transformNormalizerLeft.ClearStats();
        transformNormalizerRight.ClearStats();
        playbackReader.LoadRandomReplayFile();
    }
}
