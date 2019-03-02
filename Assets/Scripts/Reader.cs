using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reader : MonoBehaviour
{
    public bool playback = false;
    public int playbackIndex = 0;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public int maxRecordingFile = 152;
    public Agent agent;
    private int fileNum = 0;

    private List<Observation> observations;
    
    public void InitializeWithRecording(string data)
    {
        observations = new List<Observation>();
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));  
        DataContractJsonSerializer ser = new DataContractJsonSerializer(observations.GetType());  
        observations = ser.ReadObject(ms) as List<Observation>;  
        ms.Close();  
    }

    private void Start()
    {
        LoadRandomReplayFile();
    }
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    
    public void LoadRandomReplayFile()
    {
        playback = false;
        playbackIndex = 0;
        //fileNum = Random.Range(0, maxRecordingFile + 1);
        fileNum = (fileNum+1) % maxRecordingFile;
        TextAsset textAsset = Resources.Load<TextAsset>(fileNum.ToString());
        InitializeWithRecording(textAsset.text);
        leftHand.GetComponent<TransformNormalizer>().ClearStats();
        rightHand.GetComponent<TransformNormalizer>().ClearStats();
        //playbackIndex = Random.Range(0, observations.Count / 2);
        playback = true;
    }

    private void FixedUpdate()
    {
        if (playback && playbackIndex < observations.Count)
        {
            Observation observation = observations[playbackIndex];
            
            if (leftHand != null)
            {
                leftHand.localPosition = new Vector3(observation.leftHandPositionX, observation.leftHandPositionY, observation.leftHandPositionZ);
                leftHand.localEulerAngles = new Vector3(observation.leftHandRotationX, observation.leftHandRotationY, observation.leftHandRotationZ);
            }

            if (rightHand != null)
            {
                rightHand.localPosition = new Vector3(observation.rightHandPositionX, observation.rightHandPositionY, observation.rightHandPositionZ);
                rightHand.localEulerAngles = new Vector3(observation.rightHandRotationX, observation.rightHandRotationY, observation.rightHandRotationZ);
            }

            if (head != null)
            {
                head.localPosition = new Vector3(observation.headPositionX, observation.headPositionY, observation.headPositionZ);
                head.localEulerAngles = new Vector3(observation.headRotationX, observation.headRotationY, observation.headRotationZ);
            }

            playbackIndex++;
        }

        if (playbackIndex >= observations.Count)
        {
            if (agent != null)
            {
                agent.Done();
            }
            LoadRandomReplayFile();
        }
    }

    public int GetScenarioLength()
    {
        return observations.Count;
    }

    public float GetCurrentState()
    {
        return observations[playbackIndex].state;
    }

    public float GetCurrentArousal()
    {
        return observations[playbackIndex].arousal;
    }

    string ReadFromFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        string data = reader.ReadToEnd();
        reader.Close();
        return data;
    }
}