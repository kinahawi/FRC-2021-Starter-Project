using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualInputGearbox : MonoBehaviour {


    public MotorBehavior Motor1;
    public MotorBehavior Motor2;

    public List<WheelCollider> Wheels;


    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        // motors need to be turning in opposite directions
        double totalTorque = Motor1.getTorque() + Motor2.getTorque();

        foreach (WheelCollider w in Wheels) {
            w.motorTorque = (float)totalTorque;
        }

    }

}
