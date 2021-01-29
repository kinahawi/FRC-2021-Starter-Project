using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class AnalogInWSBehavior : MonoBehaviour {


    public WebSocketInterface ws;

    public int PortNumber;

    public double voltage;
    private double lastVoltage = 0.0 ;

    // what about other fields?
    // <avg_bits
    // <oversample_bits
    // <accum_init
    // >accum_value
    // >accum_count


    // set input to Roborio (its an output from the sim)
    public void setVoltage(float val) {
        voltage = val;
    }


    public void ProcessData(JToken data) {
        // There are no outputs from the Roborio for the simulation to process
    }


    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("AI", $"{PortNumber}", ProcessData);
        }
    }

    // Update is called once per frame
    void Update() {
        if (ws && voltage != lastVoltage) {
            Newtonsoft.Json.Linq.JObject jo = new JObject();
            jo.Add(new JProperty("type", "AI"));
            jo.Add(new JProperty("device", $"{PortNumber}"));

            Newtonsoft.Json.Linq.JObject dataObject = new JObject();
            dataObject.Add(new JProperty(">voltage", voltage));
            jo.Add("data", dataObject);
            string message = JsonConvert.SerializeObject(jo);
            ws.Send(message);
        }

        lastVoltage = voltage ;
         
    }
}
