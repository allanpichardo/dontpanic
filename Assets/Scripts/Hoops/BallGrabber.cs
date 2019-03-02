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
	private bool isGrabbing = false;
	public float strength = 0.1f;

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
		if(isGrabbing) {
			ball.transform.position = this.transform.position;
		}
    }

    private void GrabBall()
    {
        ball.transform.parent = this.transform;
		isGrabbing = true;
    }

    private void ThrowBall()
    {
        //ball.transform.parent = null;
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        Vector3 velocity = new Vector3(0,1,1) * strength;
        rigidbody.AddForce(velocity.normalized, ForceMode.VelocityChange);
        ball = null;
		isGrabbing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
			Debug.Log("Touching Ball");
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
