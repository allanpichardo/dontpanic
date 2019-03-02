using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TransformNormalizer : MonoBehaviour
{
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private Vector3 minRotation;
    private Vector3 maxRotation;

    private Vector3 minVelocity;
    private Vector3 maxVelocity;

    private void Start()
    {
        minPosition = new Vector3();
        maxPosition = new Vector3();
        minRotation = new Vector3();
        maxRotation = new Vector3();
        minVelocity = new Vector3();
        maxVelocity = new Vector3();
    }

    public void UpdateValues(Vector3 velocity)
    {
        if (velocity.x < minVelocity.x)
        {
            minVelocity.x = velocity.x;
        }

        if (velocity.y < minVelocity.y)
        {
            minVelocity.y = velocity.y;
        }

        if (velocity.z < minVelocity.z)
        {
            minVelocity.z = velocity.z;
        }

        if (velocity.x > maxVelocity.x)
        {
            maxVelocity.x = velocity.x;
        }

        if (velocity.y > maxVelocity.y)
        {
            maxVelocity.y = velocity.y;
        }

        if (velocity.z > maxVelocity.z)
        {
            maxVelocity.z = velocity.z;
        }
    }

    public void UpdateValues(Transform transform)
    {
        if (transform.localPosition.x < minPosition.x)
        {
            minPosition.x = transform.localPosition.x;
        }
        if (transform.localPosition.y < minPosition.y)
        {
            minPosition.y = transform.localPosition.y;
        }
        if (transform.localPosition.z < minPosition.z)
        {
            minPosition.z = transform.localPosition.z;
        }
        
        if (transform.localEulerAngles.x < minRotation.x)
        {
            minRotation.x = transform.localEulerAngles.x;
        }
        if (transform.localEulerAngles.y < minRotation.y)
        {
            minRotation.y = transform.localEulerAngles.y;
        }
        if (transform.localEulerAngles.z < minRotation.z)
        {
            minRotation.z = transform.localEulerAngles.z;
        }
        
        if (transform.localPosition.x > maxPosition.x)
        {
            maxPosition.x = transform.localPosition.x;
        }
        if (transform.localPosition.y > maxPosition.y)
        {
            maxPosition.y = transform.localPosition.y;
        }
        if (transform.localPosition.z > maxPosition.z)
        {
            maxPosition.z = transform.localPosition.z;
        }
        
        if (transform.localEulerAngles.x > maxRotation.x)
        {
            maxRotation.x = transform.localEulerAngles.x;
        }
        if (transform.localEulerAngles.y > maxRotation.y)
        {
            maxRotation.y = transform.localEulerAngles.y;
        }
        if (transform.localEulerAngles.z > maxRotation.z)
        {
            maxRotation.z = transform.localEulerAngles.z;
        }
        
    }

    private float NormValue(float current, float min, float max)
    {
        if (Math.Abs(max - min) < 0.0000001) return current;
        
        return (current - min) / (max - min);
    }

    public Vector3 GetNormalizedPosition(Transform transform)
    {
        UpdateValues(transform);
        
        Vector3 norm = new Vector3(
            NormValue(transform.localPosition.x, minPosition.x, maxPosition.x),
            NormValue(transform.localPosition.y, minPosition.y, maxPosition.y),
            NormValue(transform.localPosition.z, minPosition.z, maxPosition.z)
            );

        return norm;
    }

    public Vector3 GetNormalizedRotation(Transform transform)
    {
        UpdateValues(transform);
        
        return transform.localEulerAngles / 360.0f;
        
    }

    public Vector3 GetNormalizedVelocity(Vector3 rawVelocity)
    {
        UpdateValues(rawVelocity);
        
        Vector3 norm = new Vector3(
            NormValue(rawVelocity.x, minVelocity.x, maxVelocity.x),
            NormValue(rawVelocity.y, minVelocity.y, maxVelocity.y),
            NormValue(rawVelocity.z, minVelocity.z, maxVelocity.z)
        );

        return norm;
    }

    public void ClearStats()
    {
    }
}