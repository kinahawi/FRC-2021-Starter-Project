using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MotorBehavior : MonoBehaviour
{
    // The input received from a motor controller
    public abstract void setInput(double val) ;

    // The torque produced by the motor given the set input value.
    public abstract double getTorque() ;
}
