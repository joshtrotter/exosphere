using UnityEngine;
using System.Collections;

public class PressureControlledManualDoor : PressureReceiver {

	public ManualDoor door;
	public bool pressureToOpen = true;

	public override void Apply (float pressureAmount)
	{
		door.SetOpenAmount (pressureAmount * (pressureToOpen ? 1 : -1));
	}
}
