# FRC-2021-Starter-Project
Unity project to use when getting started with simulations for FIRST Robotics during the 2021 season.


Here is a quick getting started script. Here is a link to the YouTube video that accompanies it.

<!-----
NEW: Check the "Suppress top comment" option to remove this info from the output.

Conversion time: 0.5 seconds.


Using this Markdown file:

1. Paste this output into your source file.
2. See the notes and action items below regarding this conversion run.
3. Check the rendered output (headings, lists, code blocks, tables) for proper
   formatting and use a linkchecker before you publish this page.

Conversion notes:

* Docs to Markdown version 1.0β29
* Sat Jan 16 2021 09:40:41 GMT-0800 (PST)
* Source doc: First Chassis Script
----->



# Building and running a simple robot chassis using Unity and WPILib simulation capabilities

Prerequisites 



*   Unity version 2020.2.1f1 installed and user has basic familiarity with Unity
*   WPILib tools for 2021 Season are installed and that user has some familiarity with developing robot code.
*   FRC Unity starter kit downloaded and available to open
    *   [https://github.com/kinahawi/FRC-2021-Starter-Project](https://github.com/kinahawi/FRC-2021-Starter-Project)
*   Download Differential drive example from CTRE Website
    *   [https://github.com/CrossTheRoadElec/Phoenix-Examples-Languages/tree/sim-examples](https://github.com/CrossTheRoadElec/Phoenix-Examples-Languages/tree/sim-examples)
*   Download latest CTRE release from
    *   [https://github.com/CrossTheRoadElec/Phoenix-Releases/releases/tag/v5.19.4.1](https://github.com/CrossTheRoadElec/Phoenix-Releases/releases/tag/v5.19.4.1)

The steps that follow outline what is being done in the video. They are only a summary and it’s best to watch the video to clearly understand what is being done.

Open Unity Hub

Add the Starter Project and open it.

Create basic hierarchy



1. Add empty Game Object and name it My Robot
2. Add empty Game Object to MyRobot and name it Chassis
3. Add empty Game Object to Chassis and name it Drive System

Add rigid body to My Robot



*   Mass at 25kg

Basic Chassis



1. In hierarchy, add cube to chassis
2. Scale cube to 32”x32”x4” (.8128, 0.1016)
3. Y position at 4” (.1016 m)

Add Wheels



1. Create Wheels object
2. Find Six Inch High Grip wheel in drive system components
3. Drag onto new wheels object and rename it to rm_wheel
4. Set position to y = 3” 0.0762, x = 17” - 0.4318, z=0 to place it in the middle of the right side of the bot

Create rest of the wheels



1. Copy rm_wheel and rename to rf_wheel. Change z position to 14” (.3556)
2. Copy rf_wheel and rename to lr_wheel. Negate z position
3. Select rm_wheel and copy it. Rename to lm_wheel. Negate x position and change y rotation to 180
4. Repeat steps 1 and 2 for remaining left side wheels

Add encoders to wheels.



1. Find Encoder prefab in FRC Simulation/Sensors
2. Drag it onto Wheels game object to add it to hierarchy
3. Rename it to Left Encoder
4. Fill in properties in the Inspector
    1. Drag desired encoder port from Roborio/Encoders to set Encoder Port
    2. Drag Left middle wheel onto wheel.
5. Copy left encoder and rename for right side. Update wheel property. Leave encoder port as none.

Add motors



1. Create empty gameobject called motors in Drive System
2. Drag CIM motor prefab into motors
3. Rename to Left Motor 1
4. Take a look at parameters in inspector
5. Copy Left Motor 1 and rename to Left Motor 2.
6. Repeat to create Right side motors

Add motor Controllers



1. Find TalonSRX Motor Controller in FRCSimulation Drive System Components.
2. Add it by dragging it onto Motors game object. Rename it to Left Primary Motor Controller
3. Fill in parameters in inspector
    1. Websocket interface
    2. Motor ID
    3. Motor
4. Create left side follower motor controller
    4. Drag TalonSRX Motor Controller from FRCSimulation Drive System Components and rename it Left Follower Motor Controller
5. Reselect Left Primary Motor Controller and drag Left Follower Motor Controller object onto “Follower Motor Controller” inspector property.
6. Make sure the motor IDs match those called out in the DifferentialDrive example from CTRE
    5. Left primary = 1, Left follower = 10
    6. Right primary = 2, right follower = 20

Create Gearboxes



1. Create new empty object called “Gearboxes” under “Drive System” in the hierarchy.
2. Find Gearbox in FRCSimulation/Drive System Components in the project and drag it onto the Gearboxes game object created in step 1
3. Rename it to “Left Side Gearbox”
4. Add motors and wheels to the gearbox
    1. In the inspector, enter 2 for the number of motors and 3 for the number of wheels
    2. Drag each of the left motors into the slots for motors
    3. Drag each of the left side wheels into the slots for wheels

Add Camera to demonstrate and help visualize the front of the robot.



1. Create empty object under MyRobot and name it camera.
2. Add a cube to the camera object. Change its scale to be 2”x2”x2” (0.0504m)
3. Drag Anodized Aluminum material onto cube to make it easier to see.
4. Add Camera object to cube
    1. Turn off audio listener
    2. Change Target Display to Display 3
    3. Change clipping planes to 0.001 and 50

Setting up the Robo RIO Code

Setup the project



1. Open up the DifferentialDrive project from the CRE examples
    1. If VS Code prompts you to import to the version you have install, go ahead and do that.
2. Replace thePhoenix.jsonfile in vendordeps with that obtained when downloading latest code from CTRE ([https://github.com/CrossTheRoadElec/Phoenix-Releases/releases/tag/v5.19.4.1](https://github.com/CrossTheRoadElec/Phoenix-Releases/releases/tag/v5.19.4.1))
3. Make sure the Websocket server based simulation option is available by uncommenting the appropriate line:

    ```
       simulation wpi.deps.sim.ws_server(wpi.platforms.desktop, false)
    ```


4. Comment out line 184-188 in PhysicsSim.java
5. Build the code
6. Execute WPI command to simulate the robot code
7. Go to Unity and “play” the simulation. Confirm that vehicle moves
8. Adjust “Inverted” option on right and left motor controllers so that robot moves forward/backward and turns appropriately.

        ```
        

