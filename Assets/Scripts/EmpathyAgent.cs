using System;
using MLAgents;
using UnityEngine;

public class EmpathyAgent : Agent
{
    public TrainingPlayer trainingPlayer;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    private Material material;
    private float lastDistance;
    
    public override void InitializeAgent()
    {
        material = GetComponent<MeshRenderer>().material;
        material.color = Color.white;
        trainingPlayer.LoadRandomTrial();
    }

    public override void CollectObservations()
    {
        Vector3 leftPosition = leftHand.transform.position.normalized;
        Quaternion leftRotation = leftHand.transform.rotation.normalized;

        Vector3 rightPosition = rightHand.transform.position.normalized;
        Quaternion rightRotation = rightHand.transform.rotation.normalized;
        
        Vector3 headPosition = head.transform.position.normalized;
        Quaternion headRotation = head.transform.rotation.normalized;

        Vector3 leftUp = leftHand.transform.TransformDirection(Vector3.up);
        Vector3 rightUp = rightHand.transform.TransformDirection(Vector3.up);
        Vector3 headUp = head.transform.TransformDirection(Vector3.up);
        
        AddVectorObs(leftPosition);
        AddVectorObs(leftRotation);
        AddVectorObs(rightPosition);
        AddVectorObs(rightRotation);
        AddVectorObs(headPosition);
        AddVectorObs(headRotation);
        AddVectorObs(leftUp);
        AddVectorObs(rightUp);
        AddVectorObs(headUp);
    }

    private float CalculateReward(float predicted, float actual)
    {
        float absDistance = Mathf.Abs(predicted - actual);
        double sigmoid = (1 - (2.16395  * Math.Tanh(absDistance)));
        return (float) sigmoid;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float valence = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);
        float arousal = Mathf.Clamp(vectorAction[1], -1.0f, 1.0f);

        Trial trial = trainingPlayer.GetCurrentTrial();
        float valenceReward = CalculateReward(valence, trial.valence);
        float arousalReward = CalculateReward(arousal, trial.energy);
        float reward = (0.5f * valenceReward + 0.5f * arousalReward);
        
        Vector2 actual = new Vector2(trial.valence, trial.energy);
        Vector2 guess = new Vector2(valence, arousal);
        float distance = Vector2.Distance(actual, guess);
        distance = Mathf.Clamp(distance, 0.0f, 1.0f);
        
        Monitor.Log("Step Reward", reward);
        Monitor.Log("Valence", new []{trial.valence, valence});
        Monitor.Log("Arousal", new []{trial.energy, arousal});

        //Debug.Log(distance);
        
//        SetReward(1 - distance);
        SetReward(reward);

        if (reward > 0.95)
        {
            SetReward(1.0f);
            Done();
        }

        if (reward > 0)
        {
            material.color = new Color(0,reward,0);
//            if (reward > 0.90)
//            {
//                SetReward(1.0f);
//                Done();
//            }
        }
        else if(reward < 0)
        {
            material.color = new Color(Mathf.Abs(reward), 0, 0);
        }
        else
        {
            material.color = Color.white;
        }

        lastDistance = distance;

    }

    public override void AgentReset()
    {
        material.color = Color.white;
        trainingPlayer.LoadRandomTrial();
    }
}
