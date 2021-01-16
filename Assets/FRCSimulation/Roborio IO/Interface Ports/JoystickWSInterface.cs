using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class JoystickWSInterface : MonoBehaviour {

    public WebSocketInterface ws;

    public int JoystickNumber;

    // wpk need to get rid of hard coded values at some point.

    const int NumAxes = 8;
    const int NumButtons = 9;

    public string[] AxisName = new string[NumAxes];
    public string[] ButtonName = new string[NumButtons];


    public void ProcessData(JToken data) {
    }



    // Start is called before the first frame update
    void Start() {
        if (ws != null) {
            ws.RegisterPort("Joystick", $"{JoystickNumber}", ProcessData);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (ws) {

            Newtonsoft.Json.Linq.JObject jo  = new JObject() ;
            jo.Add(new JProperty("type", "Joystick"));
            jo.Add(new JProperty("device", JoystickNumber.ToString()));

            Newtonsoft.Json.Linq.JObject dataObject = new JObject() ;

            double[] axisValues= new double[NumAxes] ;
            for (int i = 0; i < AxisName.Length; i++) {
                if ( AxisName[i].Trim() != "" ) {
                    axisValues[i] = Input.GetAxis(AxisName[i]) ;
                } else{
                    axisValues[i] = 0.0;
                }
            }
            dataObject.Add(new JProperty(">axes", axisValues)) ;

            bool[] buttonValues = new bool[NumButtons];
            for (int i = 0; i < ButtonName.Length; i++) {
                if (ButtonName[i].Trim() != "") {
                    buttonValues[i] = Input.GetAxis(ButtonName[i]) > 0;
                }
                else {
                    buttonValues[i] = false ;
                }
            }
            dataObject.Add(new JProperty(">buttons", buttonValues));

            jo.Add("data", dataObject);

            string message = JsonConvert.SerializeObject(jo);
            ws.Send(message) ;
        }



    }
}
