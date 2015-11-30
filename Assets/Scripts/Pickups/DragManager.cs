using UnityEngine;
using System.Collections;

/**
 * This class manages the dragging and dropping of the pickup inventory
 */ 
public class DragManager : MonoBehaviour {

	//How fast the pickup icon will trail the players finger during a drag 
	public float dragSpeed = 10f;
	//How fast the pickup icons will lerp towards the target slot
	public float swapSpeed = 2.5f;

	//The distance in pixels from its origin that the pickup icon will snap to when beginning a new drag
	public float dragSnapDistance = 64f;
	//The bias in pixels towards the current slot when starting a new drag - increasing this value will require the user to drag further to reach a new slot
	public float currentSlotBias = 16f;

	//The available UI slots where a pickup can be equipped
	public PickupSlot leftSlot;
	public PickupSlot rightSlot;
	public PickupSlot upperLeftSlot;
	public PickupSlot upperRightSlot;

	//The dragCentre is the point around which we determine which quadrant of the screen the player is dragging into
	private Vector3 dragCentre;
	//The pickup being dragged
	private PickupSlot dragSlot;
	//Track the finger as it moves so we can update the pickup icons position
	private Vector2 lastFrameFingerPosition;
	//The target position for the icon being dragged - we will lerp towards this position
	private Vector3 dragTargetPosition;

	//Track the pickup slot where the pickup would go if the player released their finger this frame
	private PickupSlot currentDropTarget;

	void Awake() 
	{
		//Scale the swapSpeed by the screen width so that it will be consistent across resolutions
		swapSpeed *= Screen.width;
	}

	public bool IsDragging()
	{
		return dragSlot != null;
	}

	public void StartDragging(PickupSlot dragSlot, int reflectXAxis, Vector2 fingerPosition)
	{
		if (IsDragging ()) {
			Debug.Log ("Received request to start a drag but we are already dragging something else so it will be ignored");
		} else {
			this.dragSlot = dragSlot;
			//Note that reflect x axis is used here so that we can snap either to the left or right of the origin depending on what side of the screen the origin is
			this.dragCentre = dragSlot.GetTransform().position + new Vector3((dragSnapDistance + currentSlotBias) * reflectXAxis, dragSnapDistance + currentSlotBias); 
			this.lastFrameFingerPosition = fingerPosition;

			//Snap the icon to the specified bias from the dragCentre
			Vector3 biasToOrigin = new Vector3(-currentSlotBias * reflectXAxis, -currentSlotBias);
			dragSlot.GetTransform().position = dragCentre + biasToOrigin;
			dragTargetPosition = dragSlot.GetTransform().position;
			UpdateCurrentDropTarget();

			//Make all slots visible during a drag so we can see the highlighted slot where the player is dragging towards
			ShowAllSlots ();
		}
	}

	public void Drag(Vector2 fingerPosition)
	{
		if (!IsDragging ()) {
			Debug.Log ("Received drag request with no corresponding start drag request so it will be ignored");
		} else {
			//Move the icon to follow the players finger movements
			Vector2 fingerDelta = fingerPosition - lastFrameFingerPosition;
			dragTargetPosition = dragTargetPosition + new Vector3(fingerDelta.x, fingerDelta.y);
			dragSlot.GetTransform().position = Vector3.Lerp (dragSlot.GetTransform().position, dragTargetPosition, Time.deltaTime * dragSpeed);
			lastFrameFingerPosition = fingerPosition;
			UpdateCurrentDropTarget();
		}
	}

	public void EndDrag()
	{
		if (!IsDragging ()) {
			Debug.Log ("Received end drag request with no corresponding start drag request so it will be ignored");
		} else {
			StartCoroutine(SwapPickups());
		}
	}

	private IEnumerator SwapPickups() 
	{
		//We only need to swap to/from the slots that actually have something equipped
		bool sourceFinished = !dragSlot.IsEquipped ();
		bool targetFinished = !currentDropTarget.IsEquipped ();

		//Lerp the pickup icons between the swapped slot locations
		while (!sourceFinished || !targetFinished) {
			if (!sourceFinished) {
				dragSlot.GetTransform().position = Vector3.MoveTowards(dragSlot.GetTransform().position, currentDropTarget.GetHomeCoords(), Time.deltaTime * swapSpeed);
				sourceFinished = dragSlot.GetTransform().position.Equals(currentDropTarget.GetHomeCoords());
			}
			if (!targetFinished) {
				currentDropTarget.GetTransform().position = Vector3.MoveTowards(currentDropTarget.GetTransform().position, dragSlot.GetHomeCoords(), Time.deltaTime * swapSpeed);
				targetFinished = currentDropTarget.GetTransform().position.Equals(dragSlot.GetHomeCoords());
			}
			yield return 0;
		}

		//Remove the highlights that were established as part of the drag
		dragSlot.RemovePickupHighlight();
		currentDropTarget.RemoveSlotHighlight();

		//Complete the actual swap of the pickups
		dragSlot.SwapPickup(currentDropTarget);
		//The slots that have pickups equipped may have changed so recalculate which ones need to be visible
		ResetVisibilityOnAllSlots ();

		//End this drag
		dragSlot = null;
		currentDropTarget = null;
	}

	//Highlights the pickup slot where the pickup would go if the player released their finger this frame
	private void UpdateCurrentDropTarget()
	{
		PickupSlot newDropTarget = DetermineDropTarget();
		if (newDropTarget != currentDropTarget) {
			if (currentDropTarget != null) {
				currentDropTarget.RemoveSlotHighlight();
			}
			newDropTarget.AddSlotHighlight();
			currentDropTarget = newDropTarget;
		}
	}

	//Figure out which quadrant around the dragCentre the current dragTargetPosition is in - this will determine the newDropTarget
	private PickupSlot DetermineDropTarget() 
	{
		float xOffset = dragTargetPosition.x - dragCentre.x; 
		float yOffset = dragTargetPosition.y - dragCentre.y;
		
		if (xOffset > 0) {
			if (yOffset > 0) {
				return upperRightSlot;
			} else {
				return rightSlot;
			}
		} else {
			if (yOffset > 0) {
				return upperLeftSlot;
			} else {
				return leftSlot;
			}
		}
	}

	//While dragging make sure all slots are visible
	private void ShowAllSlots ()
	{
		rightSlot.SetVisible (true);
		leftSlot.SetVisible (true);
		upperRightSlot.SetVisible (true);
		upperLeftSlot.SetVisible (true);
	}

	//After dragging then restore slots to the correct visibility
	private void ResetVisibilityOnAllSlots ()
	{
		rightSlot.UpdateVisibility ();
		leftSlot.UpdateVisibility ();
		upperRightSlot.UpdateVisibility ();
		upperLeftSlot.UpdateVisibility ();
	}


}
