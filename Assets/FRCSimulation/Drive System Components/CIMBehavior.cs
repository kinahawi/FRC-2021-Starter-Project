using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIMBehavior : MotorBehavior {
    public double MaxTorque;

    public double input;

    public double torque;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    override public void setInput(double val) {
        input = val;
    }

    override public double getTorque() {
        // exttremely simple calculation here. Could be replaced with something based on motor
        // torque curves.
        return input * MaxTorque;
    }
}
