using System;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EllenAgent : HandsEmpathyAgent
{
    private Animator animator;
    private static readonly int Valence = Animator.StringToHash("valence");
    private static readonly int Arousal = Animator.StringToHash("arousal");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnNewPrediction(Vector3 inference)
    {
        if (animator)
        {
            animator.SetFloat(Valence, inference.x);
            animator.SetFloat(Arousal, inference.y);
        }
    }
}
