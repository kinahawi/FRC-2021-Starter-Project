using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneumaticCylindarBehavior : MonoBehaviour
{

    public float StrokeLength ;

    public bool extended = false ;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        ConfigurableJoint j = this.GetComponent<ConfigurableJoint>() ;
        float desiredPosition = 0.0f ;
        if ( extended ) {
            desiredPosition = StrokeLength ;
        }
        j.targetPosition = new Vector3(0.0f, desiredPosition, 0.0f) ;
    }





// void OnDrawGizmosSelected()
//     {

//         HingeJoint hj = this.GetComponent<HingeJoint>() ;

//         Gizmos.color = Color.blue;
//         //Gizmos.matrix = transform.localToWorldMatrix;
//         Gizmos.DrawSphere(hj.anchor, 0.0254f);


//         Gizmos.color = Color.red;

//         Vector3 conectedAnchor = hj.connectedAnchor ;
//         //Gizmos.DrawSphere(conectedAnchor, 1.05f);


//         // Vector3 jointPos = new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, joint.connectedAnchor.z);
//         // Vector3 worldPos = hj.connectedBody.transform.TransformPoint(jointPos); 
        
//         Gizmos.matrix = hj.connectedBody.transform.localToWorldMatrix;
//         Gizmos.color = Color.green;
//         Gizmos.DrawSphere(conectedAnchor, 0.5f);


//         // Transform t = hj.connectedBody.transform ;
//         // Vector3 p = t.TransformPoint( connectedAnchor) ;
//         // DrawSphere( )

 

//         Gizmos.color = Color.white;
//     }




}
