using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LaserAutoGun : LaserSnapFrom {

	public override void Start() {
		base.Start ();
		this.LaunchRay ();
	}

}
