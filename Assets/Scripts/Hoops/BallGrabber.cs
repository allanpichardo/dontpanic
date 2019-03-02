using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallGrabber : MonoBehaviour
{
    public Transform grabPoint;
    public SteamVR_Action_Boolean grabPinch;
    private SteamVR_Behaviour_Pose pose;
    private GameObject ball;

    private void Start()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        if (grabPinch.GetStateDown(pose.inputSource))
        {
            if (ball != null)
            {
                GrabBall();                
            }
        }
        else if(grabPinch.GetStateUp(pose.inputSource))
        {
            if (ball != null)
            {
                ThrowBall();
            }
        }
    }

    private void GrabBall()
    {
        ball.transform.parent = this.transform;
    }

    private void ThrowBall()
    {
        ball.transform.parent = null;
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        ball = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            ball = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            ball = other.gameObject;
        }
    }
}
