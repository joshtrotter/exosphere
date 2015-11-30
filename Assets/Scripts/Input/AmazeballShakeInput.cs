using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

/**
 * The purpose of this class is to detect when the player is shaking the accelerometer and feed that data into the CrossPlatformInputManager.
 */
public class AmazeballShakeInput : MonoBehaviour {

	//The required delta in vertical acceleration between 2 frames that will be considered as one shake - lower values will make shake detection more sensitive
	public float shakeDetectThreshold = 0.25f;
	
	//The maximum amount of time (in seconds) that can elapse between two shakes (exceeding the shakeDetectThreshold) in order for them to be treated as a continuous shake 
	public float shakeDetectInterval = 0.2f;

	//The number of continuous shakes (exceeding the shakeDetectThreshold) that must occur for the shake action to be triggered
	public int requiredShakes = 2;

	//When a continuous shake is detected it will be treated as though a 'shake' button was pressed
	private CrossPlatformInputManager.VirtualButton shakeButton;

	//Track the previous frames acceleration value
	private float prevVerticalAcceleration = 0f;
	//Track the time of the last shake
	private float prevShakeTime = 0f;
	//Track whether the previous shake was a forward or backward motion - continuous shakes need to alternate directions
	private bool prevShakeForward;
	//Track the current number of continuous shakes
	private int currentShakeStreak = 0;

	//If this rig is enabled we will register the virtual shake button with the CrossPlatformInputManager. The button state will be driven from this script.
	private void OnEnable ()
	{				
		shakeButton = new CrossPlatformInputManager.VirtualButton ("Shake");
		CrossPlatformInputManager.RegisterVirtualButton (shakeButton);
	}
	
	//If this rig is disabled we will deregister the virtual shake button from the CrossPlatformInputManager. This means these button values will be derived from the default keyboard input.
	private void OnDisable ()
	{				
		if (shakeButton != null) {
			shakeButton.Remove ();
		}
	}

	void Start() {
		prevVerticalAcceleration = Input.acceleration.y;
	}

	void Update () {
		//Release the shake button if it was pressed last frame - this is only necessary to allow us to press the button again
		if (shakeButton.GetButton) {
			shakeButton.Released ();
		}

		//Get the change in acceleration this frame
		float verticalAcceleration = Input.acceleration.y;
		float verticalAccelerationDelta = verticalAcceleration - prevVerticalAcceleration;

		//Check if we have exceeded the acceleration threshold
		if (Mathf.Abs (verticalAccelerationDelta) >= shakeDetectThreshold) {
			bool shakingForward = IsShakeForward(verticalAccelerationDelta);
			 
			//Check if this shake is continuing a previous shake
			if (Time.time - prevShakeTime <= shakeDetectInterval) {
				//We are continuing a previous shake so it only adds to the streak counter if the shake direction has been reversed
				if (prevShakeForward != shakingForward) {
					if (++currentShakeStreak >= requiredShakes) {
						shakeButton.Pressed();
						currentShakeStreak = 0;
					}
				}
			}
			//Otherwise start a new shake streak
			else {
				currentShakeStreak = 1;
			}
			//Record the shake values for future comparison
			prevShakeTime = Time.time;
			prevShakeForward = shakingForward;
		}

		//Record the acceleration value for future comparison
		prevVerticalAcceleration = verticalAcceleration;
	}

	private bool IsShakeForward(float verticalAccelerationDelta) 
	{
		return verticalAccelerationDelta > 0;
	}
}
