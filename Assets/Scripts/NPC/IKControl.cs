using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {
    
    protected Animator animator;
    
    private int greet = Animator.StringToHash("female_greet");
    private int pant = Animator.StringToHash("female_idle_pant");
    
    private Transform player;

    public float attentionDistance = 10.0f;

    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(animator) {
            // Set the look target position, if one has been assigned
            if(player != null)
            {

                animator.SetLookAtWeight(0.75f);
                animator.SetLookAtPosition(player.position);

                int animationState = animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer"))
                    .shortNameHash;

                if (animationState == greet)
                {
                    Vector3 heading = player.position - transform.position;
                    heading = heading / heading.magnitude;
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(heading, Vector3.up));
                }

                if (animationState == pant)
                {
                    animator.SetLookAtWeight(0);
                }
            }  
        }
    }
}
