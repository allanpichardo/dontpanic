using System;
using HTC.UnityPlugin.Vive;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

public abstract class HandsEmpathyAgent : Agent
{
    public bool showDebug = false;
    public float filterBeta = 0.5f;
    public Transform worldCenter;

    private LowpassFilter lowpassFilter;
    
    public override void InitializeAgent()
    {
        lowpassFilter = new LowpassFilter();
    }

    public override void CollectObservations()
    {
        Pose rightHandPose = VivePose.GetPose(HandRole.RightHand);
        rightHandPose = rightHandPose.GetTransformedBy(worldCenter);
        
        Pose leftHandPose = VivePose.GetPose(HandRole.LeftHand);
        leftHandPose = leftHandPose.GetTransformedBy(worldCenter);
        
        Pose headPose = VivePose.GetPose(DeviceRole.Hmd);
        headPose = headPose.GetTransformedBy(worldCenter);
        
        Vector3 leftPosition = leftHandPose.position.normalized;
        Quaternion leftRotation = leftHandPose.rotation.normalized;

        Vector3 rightPosition = rightHandPose.position.normalized;
        Quaternion rightRotation = rightHandPose.rotation.normalized;

        Vector3 headPosition = headPose.position.normalized;
        Quaternion headRotation = headPose.rotation.normalized;

        Vector3 leftUp = leftHandPose.up;
        Vector3 rightUp = rightHandPose.up;
        Vector3 headUp = headPose.up;
        
        AddVectorObs(leftPosition);
        AddVectorObs(leftRotation);
        AddVectorObs(rightPosition);
        AddVectorObs(rightRotation);
        AddVectorObs(headPosition);
        AddVectorObs(headRotation);
//        AddVectorObs(leftUp);
//        AddVectorObs(rightUp);
//        AddVectorObs(headUp);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float valence = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);
        float arousal = Mathf.Clamp(vectorAction[1], -1.0f, 1.0f);

        Vector3 inference = lowpassFilter.GetFilteredVector(new Vector3(valence, arousal, 0), filterBeta);
        
        if (showDebug)
        {
            Monitor.Log("Valence", inference.x);
            Monitor.Log("Arousal", inference.y);
        }

        OnNewPrediction(inference);
    }

    /**
     * Override this method to receive predictions
     */
    public abstract void OnNewPrediction(Vector3 inference);

    public override void AgentReset()
    {
    }
}
