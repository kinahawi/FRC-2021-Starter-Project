using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoubleSolenoidBehavior : MonoBehaviour {

    public SolenoidPortWSBehavior openChannel;
    public SolenoidPortWSBehavior closeChannel;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {

    }

    public bool PressureFromOpenPort() {
        return openChannel.output && !closeChannel.output;
    }

    public bool PressureFromClosePort() {
        return closeChannel.output && !openChannel.output;
    }




}
