using System;
using MLAgents;
using UnityEngine;

public class EmpathyAgent : Agent
{
    public TrainingPlayer trainingPlayer;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    private EmpathyAcademy academy;
    private float threshold = 0.7f;

    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    private float lastDistance;

    public override void InitializeAgent()
    {
        academy = FindObjectOfType<EmpathyAcademy>();
        trainingPlayer.LoadRandomTrial();
    }

    public override void CollectObservations()
    {
        Vector3 leftPosition = head.transform.InverseTransformPoint(leftHand.transform.position).normalized;
        Vector3 rightPosition = head.transform.InverseTransformPoint(rightHand.transform.position).normalized;
        float leftAngle = Vector3.Angle(head.transform.TransformDirection(Vector3.forward), leftHand.transform.TransformDirection(Vector3.left));
        float rightAngle = Vector3.Angle(head.transform.TransformDirection(Vector3.forward), rightHand.transform.TransformDirection(Vector3.right));


//        float leftVelocity = 0f;
//        if (lastLeftPos.sqrMagnitude > 0)
//        {
//            leftVelocity = ((leftPosition - lastLeftPos) / Time.fixedDeltaTime).magnitude;
//        }
//        
//        float rightVelocity = 0f;
//        if (lastRightPos.sqrMagnitude > 0)
//        {
//            rightVelocity = ((rightPosition - lastRightPos) / Time.fixedDeltaTime).magnitude;
//        }

        Debug.Log(leftPosition+", "+rightPosition+", "+leftAngle+", "+rightAngle);
        
        AddVectorObs(leftPosition);
        AddVectorObs(rightPosition);
        AddVectorObs(leftAngle);
        AddVectorObs(rightAngle);

        lastLeftPos = leftPosition;
        lastRightPos = rightPosition;
    }

    private Quaternion TransformRotationToLocalOf(Quaternion world, Quaternion target)
    {
        Quaternion LocalRotation = Quaternion.Inverse(target) * world;
        return LocalRotation;
    }

    private float CalculateReward(float predicted, float actual)
    {
        float absDistance = Mathf.Abs(predicted - actual);
        double sigmoid = (1 - (2.16395  * Math.Tanh(absDistance)));
        return (float) sigmoid;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //float valence = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);
        float arousal = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);

        Trial trial = trainingPlayer.GetCurrentTrial();
        //float valenceReward = CalculateReward(valence, trial.valence);
        float reward = Mathf.Clamp(CalculateReward(arousal, trial.energy), -1.0f, 1.0f);
        //float reward = (0.5f * valenceReward) + (0.5f * arousalReward);

//        float maxDistance = 1.414215f;
//        Vector2 guess = new Vector2(valence, arousal);
//        Vector2 actual = new Vector2(trial.valence, trial.energy);
//        float distance = Vector2.Distance(guess, actual);
//        float r = (maxDistance - distance) / maxDistance;
        
        Monitor.Log("Step Reward", reward/700f);
        //Monitor.Log("Valence", new []{trial.valence, valence});
        Monitor.Log("Arousal", new []{trial.energy, arousal});

//        if (reward > 0.95)
//        {
//            SetReward(1.0f);
//            Done();
//        }
//
//        if (reward > 0)
//        {
//            SetReward(reward);
//        }

        //SetReward(reward/700);
        
        if (reward >= threshold)
        {
            SetReward(1.0f);
            Done();
        }
        
        
        
//        if (distance < lastDistance)
//        {
//            SetReward(1f / 700.0f);
//        }else if (distance > lastDistance)
//        {
//            SetReward(-1f / 700.0f);
//        }


//        lastDistance = distance;
    }

    public override void AgentOnDone()
    {
        base.AgentOnDone();
    }

    public override void AgentReset()
    {
        threshold = academy.resetParameters["target"];
        lastLeftPos = Vector3.zero;
        lastRightPos = Vector3.zero;
        trainingPlayer.LoadRandomTrial();
    }
}
