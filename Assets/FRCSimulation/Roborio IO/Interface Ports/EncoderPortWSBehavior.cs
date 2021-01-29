using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class EncoderPortWSBehavior : MonoBehaviour {


    public WebSocketInterface ws;

    public int EncoderNumber = 0 ;

    private int samplesToAverage ;
    public int count = 0 ;

    private int lastCount = 0 ;


    public void SetCounts( int c) {
        count = c;
    }


    public void ProcessData(JToken data) {
        samplesToAverage = data.Value<int>("<samples_to_average");
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("Encoder", $"{EncoderNumber}", ProcessData);
        }
    }


    // Update is called once per frame
    void Update() {

        if (ws && count != lastCount) {
            Newtonsoft.Json.Linq.JObject jo = new JObject();
            jo.Add(new JProperty("type", "DIO"));
            jo.Add(new JProperty("device", $"{EncoderNumber}"));

            Newtonsoft.Json.Linq.JObject dataObject = new JObject();
            dataObject.Add(new JProperty(">count", count));
            jo.Add("data", dataObject);
            string message = JsonConvert.SerializeObject(jo);
            ws.Send(message);
        }
        lastCount= count ;
    }
}
