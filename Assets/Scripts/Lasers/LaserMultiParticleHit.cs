using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserMultiParticleHit : MonoBehaviour {

	public ParticleSystem hitParticles;

	public class SingleLaserHit {
		public ParticleSystem particles; 
		public bool hitThisFrame;
		public bool hitLastFrame;
	}

	private Dictionary<ArcReactor_Launcher, SingleLaserHit> allLaserHits = new Dictionary<ArcReactor_Launcher, SingleLaserHit>();
	
	//Invoked from the ArcReactor system whenever a laser hits the collider this object is attached to
	public void ArcReactorHit(ArcReactorHitInfo hitInfo)
	{
		SingleLaserHit hit;
		if (allLaserHits.TryGetValue (hitInfo.launcher, out hit)) {
			DoHitContinue (hit, hitInfo);
		} else {
			DoHitStart(hitInfo);
		}
	}
	
	public void DoHitStart(ArcReactorHitInfo hit){
		//Update the hit position of the particle effect
		SingleLaserHit newHit = new SingleLaserHit ();
		newHit.particles = GameObject.Instantiate<ParticleSystem> (hitParticles);
		newHit.particles.transform.position = hit.raycastHit.point;
		newHit.particles.transform.LookAt(hit.raycastHit.point + hit.raycastHit.normal);
		newHit.hitThisFrame = true;
		allLaserHits.Add (hit.launcher, newHit);
		StartCoroutine(TrackHit (newHit, hit.launcher));
	}

	private void DoHitContinue(SingleLaserHit existingHit, ArcReactorHitInfo hit){
		existingHit.particles.transform.position = hit.raycastHit.point;
		existingHit.particles.transform.LookAt(hit.raycastHit.point + hit.raycastHit.normal);
		existingHit.hitThisFrame = true;
	}
	
	public void DoHitEnd(SingleLaserHit hit, ArcReactor_Launcher launcher)
	{
		hit.particles.enableEmission = false;
		hit.particles.gameObject.SetActive (false);
		allLaserHits.Remove (launcher);
	}

	protected IEnumerator TrackHit(SingleLaserHit hit, ArcReactor_Launcher launcher)
	{
		while (hit.hitLastFrame || hit.hitThisFrame) {
			yield return new WaitForEndOfFrame();
			hit.hitLastFrame = hit.hitThisFrame;
			hit.hitThisFrame = false;
		}
		DoHitEnd (hit, launcher);
	}
}
