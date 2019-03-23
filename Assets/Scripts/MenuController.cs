using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger) || Input.GetKeyDown(KeyCode.S))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
