using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NativeWebSocket;
using System;

public class WebSocketInterface : MonoBehaviour {

    public string ServerAddress ;
    public int ServerPort ;

    public delegate void DataProcessingFunction( JToken data );

    private WebSocket ws;

    Dictionary<string, DataProcessingFunction> portFunctions;

    HashSet<string> unhandledPorts = new HashSet<string>();

    // Use this for initialization
    async void Start() {

        // Create WebSocket instance
        ws = new WebSocket($"ws://{ServerAddress}:{ServerPort}/wpilibws");

        // Add OnOpen event listener
        ws.OnOpen += () => {
            Debug.Log("WS connected!");
            Debug.Log("WS state: " + ws.State.ToString());
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) => {
            var message = System.Text.Encoding.UTF8.GetString(msg);

            // if ( message.Contains("Encoder") && message.Contains("position")) {
            //     Debug.Log($"Received data is {message}") ;
            // }

            dynamic stuff = JsonConvert.DeserializeObject(message);
            Newtonsoft.Json.Linq.JObject jo = stuff as Newtonsoft.Json.Linq.JObject;
            JToken data = jo.Property("data").Value;

            string typeField = jo.Property("type").Value.ToString();
            string deviceName = jo.Property("device").Value.ToString();

            if (portFunctions != null) {
                string portName = $"{typeField}/{deviceName}";
                if (portFunctions.ContainsKey(portName)) {
                    portFunctions[portName](data) ;
                }
                else {
                    if (!unhandledPorts.Contains(portName)) {
                        if (portName.ToLower().Contains("gyro") || portName.ToLower().Contains("analog") ) {
                            //Debug.Log($"Unhandled IO type={typeField}, device={deviceName}");
                        }
                        unhandledPorts.Add(portName);
                    }
                }
            }
        };

        // Add OnError event listener
        ws.OnError += (string errMsg) => {
            Debug.Log("WS error: " + errMsg);
        };

        // Add OnClose event listener
        ws.OnClose += (WebSocketCloseCode code) => {
            Debug.Log("WS closed with code: " + code.ToString());
        };

        // Connect to the server
        await ws.Connect();

    }


    public void RegisterPort(string type, string device, DataProcessingFunction f) {
        if (portFunctions == null) {
            portFunctions = new Dictionary<string, DataProcessingFunction>();
        }
        string portName = $"{type}/{device}";
        if (!portFunctions.ContainsKey(portName)) {
            portFunctions.Add(portName, f);
        }
    }


    private async void OnApplicationQuit() {
        await ws.Close();
    }



    async public void Send(string data) {
        if (ws.State == WebSocketState.Open) {
            await ws.SendText(data);
        }
    }

    // Update is called once per frame
    void Update() {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws.DispatchMessageQueue();
#endif
    }


    async public void OnDestroy() {
        if (ws.State != WebSocketState.Closed) {
            await ws.Close();
        }
    }
}
