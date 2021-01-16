using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WheelBehavior : MonoBehaviour
{

    void Start()
    {
    }

    public void FixedUpdate() 
    {

        if (this.transform.childCount == 0) {
            return;
        }
     
        Vector3 position;
        Quaternion rotation;
        WheelCollider wc = gameObject.GetComponent<WheelCollider>() ;
        
        wc.steerAngle = this.transform.localEulerAngles.y ;

        if ( wc) {
            wc.GetWorldPose(out position, out rotation);
            Transform visualWheel = this.transform.GetChild(0);
            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }


    }

}
