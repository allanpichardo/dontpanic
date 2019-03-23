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
    private Vector3 lastLeftPos = Vector3.zero;
    private Vector3 lastRightPos = Vector3.zero;

    private LowpassFilter lowpassFilter;
    
    public override void InitializeAgent()
    {
        lowpassFilter = new LowpassFilter();
    }

    public override void CollectObservations()
    {
        float maxV = 20.0f;
        float minV = 0.0f;
        
        RigidPose rightHandPose = VivePose.GetPose(HandRole.RightHand);
        RigidPose leftHandPose = VivePose.GetPose(HandRole.LeftHand);
        RigidPose headPose = VivePose.GetPose(DeviceRole.Hmd);

        Vector3 leftPosition = headPose.InverseTransformPoint(leftHandPose.pos).normalized;
        Vector3 rightPosition = headPose.InverseTransformPoint(rightHandPose.pos).normalized;
        
        float leftVelocity = 0f;
        if (lastLeftPos.sqrMagnitude > 0)
        {
            leftVelocity = ((leftPosition - lastLeftPos) / Time.fixedDeltaTime).magnitude;
            leftVelocity = (leftVelocity - minV) / (maxV - minV);
        }
        
        float rightVelocity = 0f;
        if (lastRightPos.sqrMagnitude > 0)
        {
            rightVelocity = ((rightPosition - lastRightPos) / Time.fixedDeltaTime).magnitude;
            rightVelocity = (rightVelocity - minV) / (maxV - minV);
        }
        
        //Debug.Log(leftPosition+" "+rightPosition+" "+leftAngle+", "+rightAngle);
        
        //AddVectorObs(leftPosition);
        //AddVectorObs(rightPosition);
        AddVectorObs(leftVelocity);
        AddVectorObs(rightVelocity);

        lastLeftPos = leftPosition;
        lastRightPos = rightPosition;
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
