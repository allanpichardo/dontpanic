using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallGrabber : MonoBehaviour
{
    public SteamVR_Action_Boolean grabPinch;
    private SteamVR_Behaviour_Pose pose;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (grabPinch.GetStateDown(pose.inputSource))
        {
            Debug.Log("Pinch Down");
        }
        else if(grabPinch.GetStateUp(pose.inputSource))
        {
            Debug.Log("Pinch Up");
        }
    }
}
