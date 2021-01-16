using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PCMWSBehavior : MonoBehaviour {

    public WebSocketInterface ws;

    public int PCMNumber = 0 ;

    public bool closedLoop = false ;

    public double current = 0.0 ;
    public bool isOn = true ;
    public bool pressureSwitch = false ;


    public void ProcessData(JToken data) {
        closedLoop = data.Value<bool>("<closed_loop");
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("PCM", $"{PCMNumber}", ProcessData);
        }
    }


    // Update is called once per frame
    void Update() {

        if (ws) {
            Newtonsoft.Json.Linq.JObject jo = new JObject();
            jo.Add(new JProperty("type", "PCM"));
            jo.Add(new JProperty("device", $"{PCMNumber}"));

            Newtonsoft.Json.Linq.JObject dataObject = new JObject();
            dataObject.Add(new JProperty(">current", current));
            dataObject.Add(new JProperty(">on", isOn));
            dataObject.Add(new JProperty(">pressure_switch", pressureSwitch));
            jo.Add("data", dataObject);
            string message = JsonConvert.SerializeObject(jo);
            ws.Send(message);
        }


    }
}
