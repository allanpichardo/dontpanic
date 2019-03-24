using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public GameObject scatterPoint;
    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSphere(float[] observations)
    {
        foreach(float obs in observations)
        {
            Instantiate(scatterPoint, new Vector3(0, 0, obs), Quaternion.identity);
        }
        
    }
}
