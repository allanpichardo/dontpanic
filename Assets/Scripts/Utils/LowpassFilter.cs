
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowpassFilter
{
    private Vector3 lastVector;

    public LowpassFilter()
    {
        this.lastVector = new Vector3();
    }

    public Vector3 GetFilteredVector(Vector3 unfiltered, float beta)
    {
        lastVector = Vector3.Lerp(lastVector, unfiltered, beta);
        return lastVector;
    }

}
