using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class DIOWSBehavior : MonoBehaviour {


    public WebSocketInterface ws;

    public int PortNumber;

    public bool isInput = true;
    public bool currentValue ;


    public bool getOutput() {
        if (!isInput) {
            return currentValue;
        }
        else {
            // should we throw an exception?
            return false;
        }
    }

    public void ProcessData(JToken data) {
        isInput = data.Value<bool>("<input");
        if ( !isInput ) {
            currentValue = data.Value<bool>("<>value") ;
        }
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("DIO", $"{PortNumber}", ProcessData);
        }
    }

    // Update is called once per frame
    void Update() {
        // If its an input to the Roborio (i.e.,output from the sim)
        if (isInput) {
            if (ws) {
                Newtonsoft.Json.Linq.JObject jo = new JObject();
                jo.Add(new JProperty("type", "DIO"));
                jo.Add(new JProperty("device", $"{PortNumber}"));

                Newtonsoft.Json.Linq.JObject dataObject = new JObject();
                dataObject.Add(new JProperty("<>value", currentValue));
                jo.Add("data", dataObject);
                string message = JsonConvert.SerializeObject(jo);
                ws.Send(message);
            }
        }
    }
}
