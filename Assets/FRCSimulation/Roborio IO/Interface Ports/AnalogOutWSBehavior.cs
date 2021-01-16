using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AnalogOutWSBehavior : MonoBehaviour {

    public WebSocketInterface ws;

    public int PortNumber;

    public double voltage;


    public double GetVoltage() {
        return voltage ;
    }



    public void ProcessData(JToken data) {
        voltage = data.Value<double>("<voltage");
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("AO", $"{PortNumber}", ProcessData);
        }
    }

    // Update is called once per frame
    void Update() {
        // nothing to send back to Roborio
    }
}
