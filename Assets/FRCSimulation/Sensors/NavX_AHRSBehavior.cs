using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class NavX_AHRSBehavior : MonoBehaviour {


    public WebSocketInterface WebSocketInterface;

    public Rigidbody Body ;

    public int PortNumber = 0;

    public double rate = 0.0;
    public double roll = 0.0;
    public double pitch = 0.0;
    public double yaw = 0.0;
    public double compassHeading = 0.0;
    public double fusedHeading = 0.0;

    public string accelX ;
    public string accelY ;
    public string accelZ ;


    private const string DeviceType = "SimDevice";
    private string deviceName = "";

    private double lastYaw = 0.0;
    private Vector3 lastVelocity ;


    private void ProcessMain(JToken data) {
        // Not expecting any outputs from the robot
    }


    // Start is called before the first frame update
    void Start() {
        deviceName = $"navX-Sensor[{PortNumber}]";

        // wpk - test to see if this is needed or can be removed.
        if (WebSocketInterface != null) {
            WebSocketInterface.RegisterPort(DeviceType, deviceName, ProcessMain);
        }
        else {
            string name = this.name;
            Debug.Log($"WebSocket interface is null in {this.GetType().Name} in game object {name}");
        }
        lastYaw = this.transform.rotation.eulerAngles.y;
        lastVelocity = Body.velocity ;
    }


    private double AdjustedAngle(double a) {
        return a < 180.0 ? a : a - 360.0 ;
    }

    // Update is called once per frame
    void Update() {

        if (Body) {
            // In unity, the axis normal to the ground is the Y axis.
            yaw = AdjustedAngle(this.transform.rotation.eulerAngles.y);
            pitch = AdjustedAngle(this.transform.rotation.eulerAngles.x);
            roll = AdjustedAngle(this.transform.rotation.eulerAngles.z);
            rate = Mathf.Rad2Deg * Body.angularVelocity.y ;
            compassHeading = this.transform.rotation.eulerAngles.y ;
            fusedHeading = this.transform.rotation.eulerAngles.y;
            lastYaw = yaw;

            Vector3 linearWorldAccel = (Body.velocity - lastVelocity ) / Time.deltaTime ;
            accelX = $"{linearWorldAccel.x:0.000}" ;
            accelY = $"{linearWorldAccel.y:0.000}" ;
            accelZ = $"{linearWorldAccel.z:0.000}" ;
            lastVelocity = Body.velocity ;

            // Send encoder data
            Newtonsoft.Json.Linq.JObject jo = new JObject();
            jo.Add(new JProperty("type", DeviceType));
            jo.Add(new JProperty("device", deviceName));
            Newtonsoft.Json.Linq.JObject dataObject = new JObject();
            dataObject.Add(new JProperty(">Yaw", yaw));
            dataObject.Add(new JProperty(">Pitch", pitch));
            dataObject.Add(new JProperty(">Roll", roll));
            dataObject.Add(new JProperty(">Rate", rate));

            dataObject.Add(new JProperty(">CompassHeading", compassHeading));
            dataObject.Add(new JProperty(">FusedHeading", fusedHeading));

            // The following code maps from unity axes to NavX axes.
            dataObject.Add(new JProperty(">LinearWorldAccelX", linearWorldAccel.x));
            dataObject.Add(new JProperty(">LinearWorldAccelY", linearWorldAccel.z));
            dataObject.Add(new JProperty(">LinearWorldAccelZ", linearWorldAccel.y));

            jo.Add("data", dataObject);
            string message = JsonConvert.SerializeObject(jo);
            WebSocketInterface.Send(message);

        }


    }
}
