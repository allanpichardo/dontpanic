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
        if(ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Prison");
        }
    }
}
