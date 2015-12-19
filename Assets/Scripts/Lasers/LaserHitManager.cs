using UnityEngine;
using System.Collections;

/**
 * This class handles laser hits for the attached object. It will call all attached laser modules at the appropriate 
 * times and manages tracking of hits as a single event
 */ 
public class LaserHitManager : MonoBehaviour {

	protected bool hitThisFrame;
	protected bool hitLastFrame;

	//an array of modules to call
	private LaserModule[] laserModules;
		
	void Awake () {
		laserModules = GetComponents<LaserModule> ();
		hitThisFrame = false;
		hitLastFrame = false;
	}

	//Invoked from the ArcReactor system whenever a laser hits the collider this object is attached to
	public void ArcReactorHit(ArcReactorHitInfo hitInfo)
	{
		hitThisFrame = true;
		if (!hitLastFrame) {
			DoHitStart (hitInfo);
		} else {
			//send all modules updated hitInfo
			foreach (LaserModule module in laserModules) {
				module.DoHitContinue(hitInfo);
			}
		}
	}

	//Called when a new laser hit is detected
	public virtual void DoHitStart(ArcReactorHitInfo hitInfo) 
	{
		StartCoroutine(TrackHit());
		foreach (LaserModule module in laserModules) {
			module.DoHitStart(hitInfo);
		}
	}
	
	//Called when an existing laser hit has ended
	public virtual void DoHitEnd()
	{
		foreach (LaserModule module in laserModules) {
			module.DoHitEnd();
		}
	}

	protected IEnumerator TrackHit()
	{
		while (hitLastFrame || hitThisFrame) {
			yield return new WaitForEndOfFrame();
			hitLastFrame = hitThisFrame;
			hitThisFrame = false;
		}
		DoHitEnd ();
	}

}
