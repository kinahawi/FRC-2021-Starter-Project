using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DriverStationWSInterface : MonoBehaviour {

    public WebSocketInterface ws;

    private bool newData = true ;

    private bool enable = false;
    private bool teleop = true;
    private bool autonomous = false ;
    private bool testing = false ;

    private bool estop = false ;
    private bool fmsConnected = false ;
    private bool dsConnected = true ;
    private string station = "blue1" ;
    private float matchTime = -1.0f ;
    private string gameData = "" ;


    public bool Enabled() {
        return enable;
    }

    public bool Teleop() {
        return teleop;
    }

    public bool Autonomous() {
        return autonomous ;
    }

    public bool Testing() {
        return testing ;
    }


    public void ProcessData(JToken data) {
    }

    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("DriverStation", "", ProcessData);
        }

    }

    // Update is called once per frame
    void Update() {

        if (ws) {
            Newtonsoft.Json.Linq.JObject jo = new JObject();
            jo.Add(new JProperty("type", "DriverStation"));
            jo.Add(new JProperty("device", ""));

            Newtonsoft.Json.Linq.JObject dataObject = new JObject();
            dataObject.Add(new JProperty(">new_data", newData));
            //dataObject.Add(new JProperty(">enabled", enable));

            dataObject.Add(new JProperty(">enabled", enable ? 1 : 0));

            dataObject.Add(new JProperty(">autonomous", autonomous ? 1 : 0));
            //dataObject.Add(new JProperty(">test", testing ? 1 : 0));
            dataObject.Add(new JProperty(">estop", estop ? 1 : 0));
            dataObject.Add(new JProperty(">fms", fmsConnected));
            dataObject.Add(new JProperty(">ds", dsConnected));
            dataObject.Add(new JProperty(">station", station));
            // dataObject.Add(new JProperty(">match_time", matchTime));
            dataObject.Add(new JProperty(">game_data", gameData));
            jo.Add("data", dataObject);

            string message = JsonConvert.SerializeObject(jo);
            if (enable && teleop) {
                //    Debug.Log($"Driverstation message {message}");
            }
            ws.Send(message);
        }

        newData = false ;

    }


    public void GotoDisabled() {
        enable = !enable ;
        newData = true ;
    }

    public void GotoTeleop() {
        teleop = true ;
        autonomous = false ;
        testing = false ;
        newData = true;
    }


    public void GotoAutonomous() {
        teleop = false;
        autonomous = true;
        testing = false;
        newData = true;
    }

    public void GotoTesting() {
        teleop = false;
        autonomous = false;
        testing = true;
        newData = true;
    }





}
