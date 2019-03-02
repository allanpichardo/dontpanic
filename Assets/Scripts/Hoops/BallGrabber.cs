using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallGrabber : MonoBehaviour
{
    public SteamVR_Action_Boolean grabPinch;
    public SteamVR_Input_Sources inputSource;

    private void Start()
    {
        grabPinch.AddOnChangeListener((fromAction, fromSource, newState) => 
            {
                if (newState)   // pressed
                {
                    Debug.Log("pressed");
                }
                else
                {
                    Debug.Log("released");
                }
            }
            , inputSource);
    }
 

}
