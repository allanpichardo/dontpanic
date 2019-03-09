using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Animator animator;

    private static readonly int _fire = Animator.StringToHash("fire");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        ViveInput.AddPressDown(HandRole.RightHand, ControllerButton.Trigger, OnTrigger);
    }

    private void OnDestroy()
    {
        ViveInput.RemovePressDown(HandRole.RightHand, ControllerButton.Trigger, OnTrigger);
    }

    private void OnTrigger()
    {
        if (animator)
        {
            animator.SetTrigger(_fire);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.white);
            if (hit.collider.gameObject.layer == 17)
            {
                ZombieController zombieController = hit.collider.gameObject.GetComponent<ZombieController>();
                zombieController.SetDead(true);
            }
        }
    }
}
