using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverStationUIBehavior : MonoBehaviour {
    public DriverStationWSInterface DriverStationInterface;

    public UnityEngine.UI.Button EnabledButton;
    public UnityEngine.UI.Button TeleopButton;
    public UnityEngine.UI.Button AutonomousButton;
    public UnityEngine.UI.Button TestingButton;

    // Start is called before the first frame update
    void Start() {
    }


    private void Update() {

        if (DriverStationInterface) {
            var colors = EnabledButton.colors;
            UnityEngine.UI.Text textComponent = EnabledButton.GetComponentInChildren<UnityEngine.UI.Text>();
            if (DriverStationInterface.Enabled()) {
                colors.normalColor = Color.red;
                textComponent.text = "Disable";
            }
            else {
                colors.normalColor = Color.green;
                textComponent.text = "Enable";
            }
            EnabledButton.colors = colors;

            colors = TeleopButton.colors;
            if (DriverStationInterface.Teleop()) {
                colors.normalColor = Color.green;
            }
            else {
                colors.normalColor = Color.gray;
            }
            TeleopButton.colors = colors;

            colors = AutonomousButton.colors;
            if (DriverStationInterface.Autonomous()) {
                colors.normalColor = Color.green;
            }
            else {
                colors.normalColor = Color.gray;
            }
            AutonomousButton.colors = colors;

            colors = TestingButton.colors;
            if (DriverStationInterface.Testing()) {
                colors.normalColor = Color.green;
            }
            else {
                colors.normalColor = Color.gray;
            }
            TestingButton.colors = colors;

        }

    }

}
