using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearboxBehavior : MonoBehaviour
{

//    public int NumMotors ;

    //public List<CIMBehavior> Motors ;
    public List<MotorBehavior> Motors ;

//    public int NumWheels ;

    public List<WheelCollider> Wheels ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        double totalTorque = 0.0 ;
        foreach( CIMBehavior m in Motors) {
            totalTorque += m.getTorque() ;
        }

Debug.Log($"Total torque for {name} is {totalTorque}") ;
        foreach( WheelCollider w in Wheels) {
            w.motorTorque = (float)totalTorque ;
        }
        
    }
}
