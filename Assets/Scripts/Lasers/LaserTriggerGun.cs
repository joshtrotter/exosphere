using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LaserTriggerGun : LaserSnapFrom {
	
	private bool triggered = false;
	
	public void Trigger() 
	{
		if (!triggered) {
			this.LaunchRay ();
			triggered = true;
		}
	}
	
}
