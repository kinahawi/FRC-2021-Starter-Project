using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EncoderBehavior : MonoBehaviour {

    public WheelCollider Wheel;

    public EncoderPortWSBehavior encoderPort;

    public int CountsPerRevolotion = 4096;

    public double ZeroRPMTollerance = 0.01;

    public int Counts = 0 ;

    public double rpm= 0.0 ;  //wpk delete after debug

    // Should this be here or in encoder port?
    private double reverse = 1.0;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        rpm = this.Wheel.rpm ;  // wpk remove
        if (Mathf.Abs(this.Wheel.rpm) > ZeroRPMTollerance) {
            this.Counts += (int) (reverse * CountsPerRevolotion * Time.deltaTime * (Wheel.rpm / 60.0) );
            if ( encoderPort) {
                encoderPort.SetCounts(Counts);
            }
        }

    }
}
