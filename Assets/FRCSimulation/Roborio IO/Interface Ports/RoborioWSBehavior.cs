using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class RoborioWSBehavior : MonoBehaviour {

    public WebSocketInterface ws;


    // These values made public for the time being so they can be set at run-time using the
    // Unity inspector.
    public bool fpgaButton = false ;
    public double vinVoltage = 5.0 ;
    public double vinCurrent = 0.0 ;

    public double sixvVoltage = 6.0 ;
    public double sixvCurrent = 0.0 ;
    public bool sixvActive = false ;
    public int sixvFaults = 0 ;

    public double fivevVoltage = 5.0;
    public double fivevCurrent = 0.0;
    public bool fivevActive = false;
    public int fivevFaults = 0;

    public double threeThreevVoltage = 5.0;
    public double threeThreevCurrent = 0.0;
    public bool threeThreevActive = false;
    public int threeThreevFaults = 0;



    public void ProcessData(JToken data) {
        // nothing going to robo rio
    }


    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("RoboRIO", "", ProcessData);
        }
    }

    // Update is called once per frame
    // Inputs to robot code
    void Update() {
        // add processing for each value

        Newtonsoft.Json.Linq.JObject jo = new JObject();
        jo.Add(new JProperty("type", "RoboRIO"));
        jo.Add(new JProperty("device", ""));

        Newtonsoft.Json.Linq.JObject dataObject = new JObject();
        dataObject.Add(new JProperty(">fpga_button", fpgaButton));
        dataObject.Add(new JProperty(">vin_voltage", vinVoltage));
        dataObject.Add(new JProperty(">vin_current", vinCurrent));

        dataObject.Add(new JProperty(">6v_voltage", sixvVoltage));
        dataObject.Add(new JProperty(">6v_current", sixvCurrent));
        dataObject.Add(new JProperty(">6v_active", sixvActive));
        dataObject.Add(new JProperty(">6v_faults", sixvFaults));

        dataObject.Add(new JProperty(">5v_voltage", fivevVoltage));
        dataObject.Add(new JProperty(">5v_current", fivevCurrent));
        dataObject.Add(new JProperty(">5v_active", fivevActive));
        dataObject.Add(new JProperty(">5v_faults", fivevFaults));

        dataObject.Add(new JProperty(">3v3_voltage", threeThreevVoltage));
        dataObject.Add(new JProperty(">3v3_current", threeThreevCurrent));
        dataObject.Add(new JProperty(">3v3_active", threeThreevActive));
        dataObject.Add(new JProperty(">3v3_faults", threeThreevFaults));

        jo.Add("data", dataObject);
        string message = JsonConvert.SerializeObject(jo);
        ws.Send(message);


    }
}
