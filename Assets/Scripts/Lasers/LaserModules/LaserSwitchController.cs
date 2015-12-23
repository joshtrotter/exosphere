using UnityEngine;
using System.Collections;

public class LaserSwitchController : LaserModule {

	private Switch laserSwitch;

	void Awake(){
		laserSwitch = GetComponent<Switch> ();
	}

	public override void DoHitStart (ArcReactorHitInfo hitInfo)
	{
		laserSwitch.OnLaserEnter();
	}

	public override void DoHitEnd()
	{
		laserSwitch.OnLaserExit();
	}
}
