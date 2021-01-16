using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class TalonSRXWSInterface : MonoBehaviour {

    public WebSocketInterface WebSocketInterface;
    public int Id;

    // Talon SRX[]
    // <percentOutput
    // >busVoltage
    // >supplyCurrent
    // >motorCurrent

    // Talon SRX[]/Analog In
    // <init
    // >voltage

    // Talon SRX[]/Encoder
    // >position   **no callback for that.
    // >rawPostionInput
    // >velocity

    // Talon SRX[]/DutyCycleInput
    // >connected
    // >position

    // Talon SRX[]/Fwd Limit
    // <init
    // <input
    // <>value

    // Talon SRX[]/Rev Limit
    // <init
    // <input
    // <>value


    public MotorBehavior motor;
    public TalonSRXWSInterface followerMotorController;
    public EncoderBehavior encoder;
    public bool sensorPhase;

    public float percentOutput = 0.0f;

    public bool Inverted = false ;

    public double busVoltage = 0.0;
    public double supplyCurrent = 0.0;
    public double motorCurrent = 0.0;

    public double analogInVoltage = 0.0;

    public double encoderPosition = 0.0;
    public double encoderVelocity = 0.0;

    private int lastEncoderValue = 0;

    public double dutyCycleInputPosition = 0.0;



    private void ProcessMain(JToken data) {
        if (data.SelectToken("<percentOutput") != null) {
            percentOutput = data.Value<float>("<percentOutput");
        }
    }

    private void ProcessDutyCycleInput(JToken data) {
        // Since this data is all provided by sim, don't need to process value back from robot code.
    }

    private void ProcessEncoder(JToken data) {
        // Since this data is all provided by sim, don't need to process value back from robot code.
    }

    private void ProcessAnalogIn(JToken data) {
        // Since this data is all provided by sim, don't need to process value back from robot code.
    }

    private void ProcessFwdLimit(JToken data) {
        // Since this data is all provided by sim, don't need to process value back from robot code.
    }

    private void ProcessRevLimit(JToken data) {
        // Since this data is all provided by sim, don't need to process value back from robot code.
    }


    // Start is called before the first frame update
    void Start() {
        if (WebSocketInterface != null) {
            WebSocketInterface.RegisterPort("CANMotor", $"Talon SRX[{Id}]", ProcessMain);
            // WebSocketInterface.RegisterPort("CANDutyCycle", $"Talon SRX[{Id}]/Pulse Width Input", ProcessDutyCycleInput);
            // WebSocketInterface.RegisterPort("CANAIn", $"Talon SRX[{Id}]/Analog In", ProcessAnalogIn);
            // WebSocketInterface.RegisterPort("CANEncoder", $"Talon SRX[{Id}]/Quad Encoder", ProcessEncoder);
            // WebSocketInterface.RegisterPort("CANDIO", $"Talon SRX[{Id}]/Rev Limit", ProcessRevLimit);
            // WebSocketInterface.RegisterPort("CANDIO", $"Talon SRX[{Id}]/Fwd Limit", ProcessFwdLimit);
        }
        else {
            string name = this.name;
            Debug.Log($"WebSocket interface is null in TalonSRXInterface in {name}");
        }
    }

    void UpdateState() {
        float output= percentOutput * (Inverted ? -1.0f : 1.0f) ;
        motor.setInput( output ) ;
        if ( followerMotorController) {
            followerMotorController.motor.setInput(output) ;
        }
        if (encoder) {
            int counts = encoder.Counts * (sensorPhase ? -1 : 1);
            // compute velocity.Position already figured by encoder.
            encoderPosition = counts;
            // Compute velocity as change in position. Delta ticks /time in seconds / 10 to get time per 100 mSec
            encoderVelocity = ((counts - lastEncoderValue)) / (Time.deltaTime) / 10.0f;
            lastEncoderValue = counts;
        }
        supplyCurrent = Mathf.Abs((float)percentOutput) * 30 * Random.Range(0.95f, 1.05f);
        busVoltage = 12.0 - (percentOutput * percentOutput) * 0.75 * Random.Range(0.95f, 1.05f);
    }


    // Update is called once per frame
    void FixedUpdate() {
        UpdateState() ;

        // Send encoder data
        Newtonsoft.Json.Linq.JObject jo = new JObject();
        jo.Add(new JProperty("type", "CANEncoder"));
        jo.Add(new JProperty("device", $"Talon SRX[{Id}]/Quad Encoder"));
        Newtonsoft.Json.Linq.JObject dataObject = new JObject();
        dataObject.Add(new JProperty(">rawPositionInput", encoderPosition));
        dataObject.Add(new JProperty(">velocity", encoderVelocity));
        jo.Add("data", dataObject);
        string message = JsonConvert.SerializeObject(jo);
        WebSocketInterface.Send(message);

        jo = new JObject();
        jo.Add(new JProperty("type", "CANMotor"));
        jo.Add(new JProperty("device", $"Talon SRX[{Id}]"));
        dataObject = new JObject();
        dataObject.Add(new JProperty(">busVoltage", busVoltage));
        dataObject.Add(new JProperty(">supplyCurrent", supplyCurrent));
        dataObject.Add(new JProperty(">motorCurrent", motorCurrent));
        jo.Add("data", dataObject);
        message = JsonConvert.SerializeObject(jo);
        WebSocketInterface.Send(message);

    }
}
