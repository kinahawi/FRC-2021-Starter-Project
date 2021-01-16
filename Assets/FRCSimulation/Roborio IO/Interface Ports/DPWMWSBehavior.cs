using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class DPWMWSBehavior : MonoBehaviour {

    public WebSocketInterface ws;
    public int PortNumber;

    private double dutyCycle = 0.0 ;
    private int dioPin = 0 ;


    public double GetDutyCycle() {
        return dutyCycle ;
    }

    public int GetDIOPin() {
        return dioPin ;
    }

    public void ProcessData(JToken data) {
        dutyCycle = data.Value<double>("<duty_cycle");
        dioPin = data.Value<int>("<dio_pin");
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("dPWM", $"{PortNumber}", ProcessData );
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
