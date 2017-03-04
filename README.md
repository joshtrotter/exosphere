# Exosphere

A ball-rolling puzzle game for mobile that uses tilt based controls. The player must morph the exo ball to adapt to their environment and solve physics based puzzles. The game comes with designed levels as well as a procedurally generated endless runner mode.

[![Launch Trailer](Assets/Screenshots/SelectedShots/CalamityLevelShot.png?raw=true "Launch Trailer")](https://www.youtube.com/watch?v=v6KqbtJ24PE)

*Project Status:* [Released on Play Store](https://play.google.com/store/apps/details?id=trotterj.ExoSphere)

## Custom Cross-Platform Input
The [TiltInputReader](Assets/Scripts/Input/AmazeballTiltInput.cs) detects accelerometer values and drives the character controller through the [BallInputReader](Assets/Scripts/Ball/BallInputReader.cs)

## Physics Based Character Movement
The [BallController](Assets/Scripts/Ball/BallController.cs) and [BrakeController](Assets/Scripts/Ball/BrakeController.cs) are the base controllers through which movement is handled using physical forces and drag. Triggers are available to manipulate movement properties or setup custom movements such as a rail-grind or controlled-floating.

## Camera Controls
The [CameraController](Assets/Scripts/Camera/AmazeballCam.cs) uses the accelerometer readings to pivot and tilt around the ball. The camera is prevented from clipping through world geometry. Triggers are available to manipulate or constrain the camera angle, zoom and camera effects in order to customise gameplay sequences.

## Morph System
The [MorphController](Assets/Scripts/Transform/TransformController.cs) manages the different ball [Morphs](Assets/Scripts/Transfrom/BallTransform.cs) which change the physical properties of the ball and how it moves. Some morphs also enable specialised gameplay functions such as the ability to reflect lasers.

## Pickup System
The [PickupController](Assets/Scripts/Pickups/PickupController.cs) manages the players inventory of [Pickups](Assets/Scripts/Pickups/Pickup.cs). The HUD for pickups is managed through [PickupSlots](Assets/Scripts/Pickups/PickupSlot.cs). Pick ups are consumable items that are triggered by tapping the icon on the HUD.

## Level Management
Game state such as level unlocks, achievements, and player settings are persisted to disk. Temporary level state such as collectables and interactives are stored in memory using a checkpointing system.

## Procedural Generation
The [TunnelSpawnController](Assets/Scripts/Tunnel/TunnelSpawnController.cs) builds an endless sequence of [Tunnel Pieces](Assets/Scripts/Tunnel/TunnelPiece.cs). Tunnel pieces are selected semi-randomly based on a preference system to create exciting and challenging sequences. Each piece can be further randomised with missing panels, obstacles, materials, decals and collectables. The system uses object pooling for performance.

### Info
This repository contains only the scripts from the Unity project. Binary file are kept out of the public repo as they contain art assets purchased from the Unity Asset Store.
