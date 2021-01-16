using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SolenoidPortWSBehavior : MonoBehaviour {


    public WebSocketInterface ws;

    public int PCMNumber = 0;
    public int ChannelNumber = 0 ;

    public bool output = false ;

    public void ProcessData(JToken data) {
        output = data.Value<bool>("<output");
    }


    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("Solenoid", $"{PCMNumber},{ChannelNumber}", ProcessData);
        }
    }

    // Update is called once per frame
    void Update() {
        // nothing to send back to robot
    }
}
