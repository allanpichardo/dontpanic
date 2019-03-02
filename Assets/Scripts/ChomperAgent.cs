using System;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChomperAgent : Agent
{
    public GameObject leftHand;
    public GameObject rightHand;
    [FormerlySerializedAs("actualText")] public Text guessText;

    private TransformNormalizer transformNormalizerLeft;
    private TransformNormalizer transformNormalizerRight;
    private Chomper chomper;
    
    public override void InitializeAgent()
    {
        chomper = GetComponent<Chomper>();
        transformNormalizerLeft = leftHand.GetComponent<TransformNormalizer>();
        transformNormalizerRight = rightHand.GetComponent<TransformNormalizer>();
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

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float action = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);

        if (guessText != null)
        {
            guessText.text = "Guess: "+action;
        }
        
        chomper.TakeAction(action);
    }

    public override void AgentReset()
    {
        transformNormalizerLeft.ClearStats();
        transformNormalizerRight.ClearStats();
    }
}
