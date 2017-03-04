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

## Pickup System

## Level Management

## Procedural Generation

### Info
This repository contains only the scripts from the Unity project. Binary file are kept out of the public repo as they contain art assets purchased from the Unity Asset Store.
