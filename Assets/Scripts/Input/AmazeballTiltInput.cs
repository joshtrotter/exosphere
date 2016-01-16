using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

/**
 * The purpose of this class is to take input from the mobile devices accelerometer and feed that data into the CrossPlatformInputManagers
 * axis for horizontal / vertical control.
 */
public class AmazeballTiltInput : MonoBehaviour
{
	//The minimum amount of acceleration the user must apply along each axis before it will be registered as an intentional movement. Accelerations below
	//this threshold will register a zero value on the axis input. This is to give the player a bit of a neutral zone in which they can hold the device.
	public float verticalAccelerationThreshold = 0.05f;
	public float horizontalAccelerationThreshold = 0.05f;

	//The amount of acceleration the user must apply along each axis to reach the maximum acceleration input value (-1 or 1). Note that increasing this value will mean
	//that the player needs to rotate the device further in order to achieve max acceleration. 
	public float verticalAccelerationMax = 0.3f;
	public float horizontalAccelerationMax = 0.4f; 

	//We create our two virtual axis for vertical and horizontal input that we will be updating the values of based on the accelerometer readings
	private CrossPlatformInputManager.VirtualAxis verticalAxis;
	private CrossPlatformInputManager.VirtualAxis horizontalAxis;

	//Track an offset for the vertical acceleration so that the player can choose their own neutral orientation for the device
	private float verticalAccelerationOffset;
				
	//If this rig is enabled we will register the two virtual axis with the CrossPlatformInputManager. This means these axis values will be derived from this script.
	private void OnEnable ()
	{
		//TODO this can probably be tidied up
		if (!CrossPlatformInputManager.AxisExists ("Vertical")) {
			verticalAxis = new CrossPlatformInputManager.VirtualAxis ("Vertical");
			CrossPlatformInputManager.RegisterVirtualAxis (verticalAxis);
		} else {
			verticalAxis = CrossPlatformInputManager.VirtualAxisReference("Vertical");
		}

		if (!CrossPlatformInputManager.AxisExists ("Horizontal")) {
			horizontalAxis = new CrossPlatformInputManager.VirtualAxis ("Horizontal");
			CrossPlatformInputManager.RegisterVirtualAxis (horizontalAxis);
		} else {
			horizontalAxis = CrossPlatformInputManager.VirtualAxisReference("Horizontal");
		}
	}
		
	//If this rig is disabled we will deregister the two virtual axis from the CrossPlatformInputManager. This means these axis values will be derived from the default keyboard input.
	private void OnDisable ()
	{
		if (verticalAxis != null) {
			verticalAxis.Remove ();
		}
		if (horizontalAxis != null) {
			horizontalAxis.Remove ();
		}
	}

	void Start ()
	{
		ConfigureVerticalOrientationOffset ();
	}
							
	// Update is called once per frame
	void Update ()
	{
		//If space is pressed then reset the 'neutral' device orientation
		if (Input.GetKeyDown (KeyCode.Space)) {
			ConfigureVerticalOrientationOffset ();
		}

		//Get the base acceleration values
		float verticalAcceleration = GetValueAboveThreshold (Input.acceleration.y - verticalAccelerationOffset, verticalAccelerationThreshold);
		float horizontalAcceleration = GetValueAboveThreshold (Input.acceleration.x, horizontalAccelerationThreshold);

		//Transform into scaled acceleration values (-1 to +1)
		float verticalAxisValue = GetAccelerationValueAsRatioOfMaxAcceleration (verticalAcceleration, verticalAccelerationMax);
		float horizontalAxisValue = GetAccelerationValueAsRatioOfMaxAcceleration (horizontalAcceleration, horizontalAccelerationMax);

		//Update the axis values in the CrossPlatformInputManager, ready for other game components to read as input
		verticalAxis.Update (verticalAxisValue);
		horizontalAxis.Update (horizontalAxisValue);
	}

	//Set the vertical orientation offset based upon the devices current orientation - TODO have the user explicitly acknowledge when they are in the neutral position
	public void ConfigureVerticalOrientationOffset ()
	{
		verticalAccelerationOffset = Input.acceleration.y;
	}

	//We use this to ignore any acceleration which is below the threshold value
	private float GetValueAboveThreshold (float value, float threshold)
	{
		if (value > threshold || value < -threshold) {
			return value;
		} else {
			return 0f;
		}
	}

	//Map acceleration values to between -1 and +1 based on where they sit between -maxAcceleration and +maxAcceleration
	private float GetAccelerationValueAsRatioOfMaxAcceleration (float acceleration, float maxAcceleration)
	{
		//Note the (* 2 - 1) takes the value returned by InverseLerp which is between 0 and +1 and maps to between -1 and +1
		return Mathf.InverseLerp (-maxAcceleration, maxAcceleration, acceleration) * 2 - 1;
	}
	
}