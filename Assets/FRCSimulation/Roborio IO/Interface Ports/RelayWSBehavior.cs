using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class RelayWSBehavior : MonoBehaviour {


    public WebSocketInterface ws;
    public int RelayNumber;

    public bool forward = true ;
    public bool reverse = false ;



    public void ProcessData(JToken data) {
        forward = data.Value<bool>("<fwd");
        reverse = data.Value<bool>("<rev");
    }



    public bool GetForward() {
        return forward ;
    }

    public bool GetReverse() {
        return reverse ;
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("Relay", $"{RelayNumber}", ProcessData);
        }
    }

    // Update is called once per frame
    void Update() {
        // no outputs
    }
}
