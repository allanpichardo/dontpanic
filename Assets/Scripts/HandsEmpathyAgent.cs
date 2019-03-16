using System;
using HTC.UnityPlugin.Utility;
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
        RigidPose rightHandPose = VivePose.GetPose(HandRole.RightHand);
        RigidPose leftHandPose = VivePose.GetPose(HandRole.LeftHand);
        RigidPose headPose = VivePose.GetPose(DeviceRole.Hmd);

        Vector3 leftPosition = headPose.InverseTransformPoint(leftHandPose.pos);
        Vector3 rightPosition = headPose.InverseTransformPoint(rightHandPose.pos);
        float leftAngle = Vector3.Angle(headPose.forward, leftHandPose.forward);
        float rightAngle = Vector3.Angle(headPose.forward, rightHandPose.forward);
        
        //Debug.Log(leftPosition+" "+rightPosition+" "+headPose.pos);
        
        AddVectorObs(leftPosition);
        AddVectorObs(rightPosition);
        AddVectorObs(leftAngle);
        AddVectorObs(rightAngle);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float arousal = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);

        Vector3 inference = lowpassFilter.GetFilteredVector(new Vector3(arousal, arousal, 0), filterBeta);
        
        if (showDebug)
        {
            Monitor.Log("Excitement", inference.x);
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
