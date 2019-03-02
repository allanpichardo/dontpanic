using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public float speed = 1.0f;
    public float maxBound = 60.0f;
    public float minBound = -60.0f;

    private float xPos = 0.0f;
    private bool isDirectionRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (isDirectionRight)
        {
            if (position.x < maxBound)
            {
                transform.Translate(Time.deltaTime * speed, 0, 0);
            }
            else
            {
                isDirectionRight = false;
            }
        }
        else
        {
            if (position.x > minBound)
            {
                transform.Translate(-Time.deltaTime * speed, 0, 0);
            }
            else
            {
                isDirectionRight = true;
            }
        }
    }
}
