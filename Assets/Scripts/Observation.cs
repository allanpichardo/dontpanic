
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]  
public class Observation
{
    [DataMember] public float state;
    [DataMember] public float arousal;

    [DataMember] public float headPositionX;
    [DataMember] public float headPositionY;
    [DataMember] public float headPositionZ;
    [DataMember] public float headRotationX;
    [DataMember] public float headRotationY;
    [DataMember] public float headRotationZ;
    
    [DataMember] public float leftHandPositionX;
    [DataMember] public float leftHandPositionY;
    [DataMember] public float leftHandPositionZ;
    [DataMember] public float leftHandRotationX;
    [DataMember] public float leftHandRotationY;
    [DataMember] public float leftHandRotationZ;
    
    [DataMember] public float rightHandPositionX;
    [DataMember] public float rightHandPositionY;
    [DataMember] public float rightHandPositionZ;
    [DataMember] public float rightHandRotationX;
    [DataMember] public float rightHandRotationY;
    [DataMember] public float rightHandRotationZ;
}