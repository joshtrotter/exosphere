using System;
using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{
	public enum Slot {LEFT, RIGHT, UPPER_LEFT, UPPER_RIGHT};

	//The available UI slots where a pickup can be equipped
	public PickupSlot leftSlot;
	public PickupSlot rightSlot;
	public PickupSlot upperLeftSlot;
	public PickupSlot upperRightSlot;

	//The particle effect to play when an object is picked up
	public ParticleSystem pickupEffect;

	public DragManager dragManager;

	//For right handers we will prefer to use the right hand slots first, opposite for left handers (need to make this a config option)
	public bool rightHander;

	//How long in milliseconds the phone should vibrate on a touch and hold
	public long vibrationMillis = 10L;

	//Length of time the pulse effect should run for to highlight slots
	public float pulseTime = 2f;
	private BallController ball;

	private void Start ()
	{
		// Set up the references to other game objects
		ball = GetComponent<BallController> ();

		PickupSlot[] slots = HUD.controller.GetComponentsInChildren<PickupSlot> ();

		foreach (PickupSlot slot in slots) {
			if (slot.name == "LeftPickupSlot") {
				leftSlot = slot;
			} else if (slot.name == "RightPickupSlot") {
				rightSlot = slot;
			}
			else if (slot.name == "UpperLeftPickupSlot") {
				upperLeftSlot = slot;
			}
			else if (slot.name == "UpperRightPickupSlot") {
				upperRightSlot = slot;
			}
		}

		dragManager = HUD.controller.GetComponentInChildren<DragManager> ();
	}

	public void AddPickup (Pickup pickup)
	{
		//Play a particle effect
		pickupEffect.Play ();

		//Before adding pickup to a new slot, check if we already have it equipped somewhere...
		PickupSlot slot = FindSlotWithPickup (pickup);
		if (slot == null) {
			//... Otherwise find the best available slot for it to go
			slot = FindBestAvailableSlot ();
		}
		if (slot != null) {
			slot.EquipPickup (pickup);
		}
		PulseAllSlots ();
	}

	public void UsePickup(Slot slot)
	{
		LookupSlot(slot).UsePickup (ball);
	}

	public void TouchPickup(Slot slot)
	{
		LookupSlot(slot).AddPickupHighlight ();
	}

	public void StartDragging(Slot slot, Vector2 fingerPos)
	{
		//Shake the phone - hoping to emulate the feel of android drag and drop
		Vibration.Vibrate (vibrationMillis);

		//Delegate the actual dragging to the drag manager
		dragManager.StartDragging (LookupSlot (slot), slot == Slot.LEFT ? 1 : -1, fingerPos);
	}

	public void Drag(Vector2 pos)
	{
		dragManager.Drag (pos);
	}

	public void EndDrag() 
	{
		dragManager.EndDrag ();
	}

	private PickupSlot LookupSlot(Slot slot) 
	{
		return slot == Slot.RIGHT ? rightSlot : leftSlot;
	}

	private PickupSlot FindBestAvailableSlot ()
	{
		PickupSlot slot = null;
		if (rightHander) {
			if (!rightSlot.IsEquipped ()) {
				slot = rightSlot;
			} else if (!leftSlot.IsEquipped ()) {
				slot = leftSlot;
			} else if (!upperRightSlot.IsEquipped ()) {
				slot = upperRightSlot;
			} else if (!upperLeftSlot.IsEquipped ()) {
				slot = upperLeftSlot;
			}
		} else {
			if (!leftSlot.IsEquipped ()) {
				slot = leftSlot;
			} else if (!rightSlot.IsEquipped ()) {
				slot = rightSlot;
			} else if (!upperLeftSlot.IsEquipped ()) {
				slot = upperLeftSlot;
			} else if (!upperRightSlot.IsEquipped ()) {
				slot = upperRightSlot;
			}
		}
		return slot;
	}

	private PickupSlot FindSlotWithPickup (Pickup pickup)
	{
		PickupSlot slot = null;
		if (pickup.Equals (rightSlot.GetPickup ())) {
			slot = rightSlot;
		} else if (pickup.Equals (leftSlot.GetPickup ())) {
			slot = leftSlot;
		} else if (pickup.Equals (upperRightSlot.GetPickup ())) {
			slot = upperRightSlot;
		} else if (pickup.Equals (upperLeftSlot.GetPickup ())) {
			slot = upperLeftSlot;
		}
		return slot;
	}

	private void PulseAllSlots ()
	{
		rightSlot.Pulse (pulseTime);
		leftSlot.Pulse (pulseTime);
		upperRightSlot.Pulse (pulseTime);
		upperLeftSlot.Pulse (pulseTime);
	}
	
}

