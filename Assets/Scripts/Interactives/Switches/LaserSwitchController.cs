using UnityEngine;
using System.Collections;

public class LaserSwitchController : LaserHitManager {

	private Switch laserSwitch;

	void Awake(){
		laserSwitch = GetComponent<Switch> ();
	}

	public override void DoHitStart (ArcReactorHitInfo hitInfo)
	{
		StartCoroutine (TrackHit ());
		laserSwitch.OnLaserEnter();
	}

	public override void DoHitEnd()
	{
		laserSwitch.OnLaserExit();
	}

	//this function tracks whether calls to DoHitStart are part of the same laser hit
	private IEnumerator TrackHit()
	{
		while (hitLastFrame || hitThisFrame) {
			yield return new WaitForEndOfFrame();
			hitLastFrame = hitThisFrame;
			hitThisFrame = false;
		}
		DoHitEnd ();
	}
}
