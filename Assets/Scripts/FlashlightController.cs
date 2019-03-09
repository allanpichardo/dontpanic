using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashlightController : HandsEmpathyAgent
{
	private float emotion;
	private float energy;
	public light flashlight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNewPrediction(Vector3 inference){
    	this.emotion = inference.x;
    	this.energy = inference.y;

    	// emotion = ((emotion+1)*0.5);
    	// energy = ((energy+1)*0.5);
    	// float outputMultiplier = ((emotion+energy)*0.5);
    	this.flashlight.spotAngle = 30+(energy*15);

    }
}
