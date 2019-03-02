using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]  
public class Trial
{
    [DataMember] public int subject;
    [DataMember] public int trial;
    [DataMember] public float valence;
    [DataMember] public float energy;
}
