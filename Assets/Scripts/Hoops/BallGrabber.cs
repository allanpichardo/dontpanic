using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallGrabber : MonoBehaviour
{
    public SteamVR_Action_Boolean TriggerClick;
    public SteamVR_Input_Sources inputSource;
 
    private void Start() {} //Monobehaviours without a Start function cannot be disabled in Editor, just FYI
 
    private void OnEnable()
    {
        TriggerClick.AddOnStateDownListener(Press, inputSource);
        TriggerClick.AddOnStateUpListener(Release, inputSource);
    }
 
    private void OnDisable()
    {
        TriggerClick.RemoveOnStateDownListener(Press, inputSource);
        TriggerClick.RemoveOnStateUpListener(Release, inputSource);
    }
 
    private void Press(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //put your stuff here
        print("Trigger Down");
    }

    private void Release(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger Release");
    }

}
