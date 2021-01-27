using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class ADXRS450_GyroBehavior : MonoBehaviour {


    public WebSocketInterface WebSocketInterface;

    public int PortNumber = 0 ;

    public double rate = 0.0;
    public double angle = 0.0;

    private const string DeviceType = "Gyro" ;
    private string deviceName = "" ;

    private double lastAngle = 0.0 ;


    private void ProcessMain(JToken data) {
        // Not expecting any outputs from the robot
    }



    // Start is called before the first frame update
    void Start() {
        deviceName = $"ADXRS450[{PortNumber}]" ;

        if (WebSocketInterface != null) {
            WebSocketInterface.RegisterPort(DeviceType, deviceName, ProcessMain);
        }
        else {
            string name = this.name;
            Debug.Log($"WebSocket interface is null in TalonSRXInterface in {name}");
        }
        this.lastAngle = this.transform.rotation.eulerAngles.y;
    }


    // Update is called once per frame
    void Update() {

        // In unity, the axis normal to the ground is the Y axis.
        angle = this.transform.rotation.eulerAngles.y ;
        rate = (angle-lastAngle) / Time.deltaTime ;
        lastAngle = angle ;

        // Send encoder data
        Newtonsoft.Json.Linq.JObject jo = new JObject();
        jo.Add(new JProperty("type", DeviceType));
        jo.Add(new JProperty("device", deviceName));
        Newtonsoft.Json.Linq.JObject dataObject = new JObject();
        dataObject.Add(new JProperty(">angle_x", this.angle));
        dataObject.Add(new JProperty(">rate_x", this.rate));
        jo.Add("data", dataObject);
        string message = JsonConvert.SerializeObject(jo);
        WebSocketInterface.Send(message);


    }
}
