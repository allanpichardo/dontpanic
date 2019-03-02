using UnityEngine;

public class VectorStatistics
{
    private Vector3 K;
    private Vector3 Ex;
    private Vector3 Ex2;
    private int N;

    public VectorStatistics()
    {
        K = new Vector3();
        Ex2 = new Vector3();
        Ex = new Vector3();
        N = 0;
    }

    public void AddValue(Vector3 value)
    {
        if (N == 0)
        {
            K = value;
        }

        N += 1;
        Ex += value - K;
        Ex2 += Vector3.Scale(value - K, value - K);
    }

    public Vector3 GetMean()
    {
        if (N == 0)
        {
            return Vector3.zero;
        }
        return K + Ex / N;
    }

    public Vector3 GetVariance()
    {
        if (N == 0)
        {
            return Vector3.zero;
        }
        return (Ex2 - Vector3.Scale(Ex,Ex) / N) / (N - 1);
    }

    public Vector3 GetStandardDeviation()
    {
        Vector3 variance = GetVariance();
        
        float x = Mathf.Sqrt(variance.x);
        float y = Mathf.Sqrt(variance.y);
        float z = Mathf.Sqrt(variance.z);
        
        return new Vector3(x,y,z);
    }

    public Vector3 Normalize(Vector3 vector)
    {
        Vector3 num = vector - GetMean();
        Vector3 den = GetStandardDeviation();
        return new Vector3(num.x/den.x,num.y/den.y,num.z/den.z);
    }
}