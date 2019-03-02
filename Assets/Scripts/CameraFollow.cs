using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 heading = player.transform.position - transform.position;
            heading = heading / heading.magnitude;
            
            transform.SetPositionAndRotation(
                new Vector3(player.transform.position.x, player.transform.position.y + 4, player.transform.position.z + 3),
                Quaternion.LookRotation(heading)
                );
        }
    }
}
