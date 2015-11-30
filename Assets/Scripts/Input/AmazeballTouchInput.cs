using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

/**
 * The purpose of this class is to take input from the mobile devices touch sensors and feed that data into the CrossPlatformInputManagers
 * buttons for left / right triggers.
 */
public class AmazeballTouchInput : MonoBehaviour
{
	//Used to determine the percentage of the screen that should be assigned to each button (should be less than 0.5)
	public float buttonHeightScreenPercent = 0.25f;
	public float buttonWidthScreenPercent = 0.25f;

	//We create our two virtual buttons for the pickup buttons
	private CrossPlatformInputManager.VirtualButton leftButton;
	private CrossPlatformInputManager.VirtualButton rightButton;
	
	//Input areas for the left and right buttons
	private Rect leftButtonArea;
	private Rect rightButtonArea;

	//Track the touch fingerId for each button so we can track finger movement across frames
	private int leftButtonFingerId = -1;
	private int rightButtonFingerId = -1;

	//If this rig is enabled we will register the two virtual buttons with the CrossPlatformInputManager. The button state will be driven from this script.
	private void OnEnable ()
	{				
		leftButton = new CrossPlatformInputManager.VirtualButton ("Left");
		CrossPlatformInputManager.RegisterVirtualButton (leftButton);
		
		rightButton = new CrossPlatformInputManager.VirtualButton ("Right");
		CrossPlatformInputManager.RegisterVirtualButton (rightButton);

		SetupInputAreas ();
	}
	
	//If this rig is disabled we will deregister the two virtual buttons from the CrossPlatformInputManager. This means these button values will be derived from the default keyboard input.
	private void OnDisable ()
	{				
		if (leftButton != null) {
			leftButton.Remove ();
		}
		if (rightButton != null) {
			rightButton.Remove ();
		}
	}

	//Setup Rects to detect if a touch input is in the right screen area to trigger the left or right buttons
	private void SetupInputAreas ()
	{
		float buttonHeight = Screen.height * buttonHeightScreenPercent;
		float buttonWidth = Screen.width * buttonWidthScreenPercent;
		leftButtonArea = new Rect (0, 0, buttonWidth, buttonHeight);
		rightButtonArea = new Rect (Screen.width - buttonWidth, 0, buttonWidth, buttonHeight);
	}

	void Update ()
	{
		//Test every touch against the left/right trigger input areas to determine if buttons have been pressed or released
		for (var i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch (i);

			if (touch.phase == TouchPhase.Began) {
				if (leftButtonArea.Contains (touch.position)) {
					leftButton.Pressed();
					leftButtonFingerId = touch.fingerId;
				}
				if (rightButtonArea.Contains (touch.position)) {
					rightButton.Pressed();
					rightButtonFingerId = touch.fingerId;
				}
			}
			if (touch.fingerId == leftButtonFingerId) {
				leftButton.Update(touch.position);
				if (touch.phase == TouchPhase.Ended) {
					leftButton.Released();
					leftButtonFingerId = -1;
				}
			}
			if (touch.fingerId == rightButtonFingerId) {
				rightButton.Update(touch.position);
				if (touch.phase == TouchPhase.Ended) {
					rightButton.Released();
					rightButtonFingerId = -1;
				}
			}						
		}
	}
}
