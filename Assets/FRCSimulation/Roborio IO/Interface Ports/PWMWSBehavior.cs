using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class PWMWSBehavior : MonoBehaviour {

    public WebSocketInterface ws;
    public int PortNumber;

    private double position = 0.0;
    private double speed = 0.0 ;


    public double GetSpeed() {
        return speed;
    }

    public double GetPosition() {
        return position ;
    }

    public void ProcessData( JToken data) {
        speed = data.Value<double>("<speed");
        position = data.Value<double>("<position");
    }


    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("PWM", $"{PortNumber}", ProcessData) ;
        }
    }

    // Update is called once per frame
    void Update() {
    }
}
