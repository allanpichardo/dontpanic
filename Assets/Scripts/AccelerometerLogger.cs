
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AccelerometerLogger : MonoBehaviour
{
    public enum AccelerationUnit
    {
        G, Ms2
    }

    [Range(0.0f, 1.0f)]
    public float sensitivity = 1.0f / 60.0f;
    public AccelerationUnit unit = AccelerationUnit.Ms2;
    public bool reportChangeOnly = true;
    public bool useRounding = true;
    public int roundingDecimalPlaces = 2;
    public System.MidpointRounding roundingBehavior = System.MidpointRounding.ToEven;

    private LowpassFilter lowpassFilter;
    private const float AccelerationEarth = 9.8f;
    private Vector3 lastValue = Vector3.zero;

    private void Start()
    {
        lowpassFilter = new LowpassFilter();
    }

    private void FixedUpdate()
    {
        Vector3 filteredAcceleration = lowpassFilter.GetFilteredVector(Input.acceleration, sensitivity);

        if(unit == AccelerationUnit.Ms2)
        {
            filteredAcceleration *= AccelerationEarth;
        }
        if(useRounding)
        {
            filteredAcceleration = new Vector3(
                (float)Math.Round(filteredAcceleration.x, roundingDecimalPlaces, roundingBehavior),
                (float)Math.Round(filteredAcceleration.y, roundingDecimalPlaces, roundingBehavior),
                (float)Math.Round(filteredAcceleration.z, roundingDecimalPlaces, roundingBehavior)
            );
        }

        if (reportChangeOnly)
        {
            printToLog(filteredAcceleration - lastValue);
        }
        else
        {
            printToLog(filteredAcceleration);
        }

        lastValue = filteredAcceleration;
    }

    private void printToLog(Vector3 vector)
    {
        Debug.Log(string.Format("Accel: X:{0} Y:{1} Z:{2}", vector.x, vector.y, vector.z));

    }

}
