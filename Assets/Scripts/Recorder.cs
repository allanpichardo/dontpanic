using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEditor;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    private Queue<Observation> observations;

    public bool record;
    
    public string fileName = "observation.json";
    public Transform leftHand;
    public Transform rightHand;
    public int state = 0;
    
    private const string BasePath = "Assets/Recordings/";
    // Start is called before the first frame update
    void Start()
    {
        observations = new Queue<Observation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (record)
        {
            Observation observation = new Observation();
            if (leftHand != null)
            {
                var localPosition = leftHand.localPosition;
                var localRotation = leftHand.localEulerAngles;
                observation.leftHandPositionX = localPosition.x;
                observation.leftHandPositionY = localPosition.y;
                observation.leftHandPositionZ = localPosition.z;
                observation.leftHandRotationX = localRotation.x;
                observation.leftHandRotationY = localRotation.y;
                observation.leftHandRotationZ = localRotation.z;
            }

            if (rightHand != null)
            {
                var localPosition = rightHand.localPosition;
                var localRotation = leftHand.localEulerAngles;
                observation.rightHandPositionX = localPosition.x;
                observation.rightHandPositionY = localPosition.y;
                observation.rightHandPositionZ = localPosition.z;
                observation.rightHandRotationX = localRotation.x;
                observation.rightHandRotationY = localRotation.y;
                observation.rightHandRotationZ = localRotation.z;
            }

            observation.state = state;
        
            observations.Enqueue(observation);
        }
    }
    
    void SaveToFile(string path, string data)
    {
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(data);
        writer.Close();
    }

    private void OnApplicationQuit()
    {
        if (observations.Count > 0)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Observation[]));
            ser.WriteObject(stream1, observations.ToArray());
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);

            SaveToFile(BasePath + fileName, sr.ReadToEnd());
        }
    }
}
