using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneumaticPistonBehavior : MonoBehaviour
{

    // public Rigidbody RodEndRigidBody ;
    // public Rigidbody BaseEndRigidBody ;

    public DoubleSolenoidBehavior solenoid ;

    private GameObject rodObject ;
    private GameObject tubeObject ;

    // Start is called before the first frame update
    void Start() {
        rodObject = this.transform.Find("Rod").gameObject;
        tubeObject = this.transform.Find("Tube").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( solenoid ) {
            bool open = solenoid.PressureFromOpenPort() ;
            bool closed = solenoid.PressureFromClosePort() ;
            rodObject.GetComponent<PneumaticCylindarBehavior>().extended = (open && !closed) ;  
        }      
    }
}
