using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopsGame : MonoBehaviour
{
    private void Awake()
    {
        Physics.gravity = new Vector3(0, -100, 0);
    }
}
