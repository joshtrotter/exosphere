using UnityEngine;
using System.Collections;

public class LaserParticleHit : LaserModule {

	public ParticleSystem hitParticles;

	void Awake () {
		hitParticles = GameObject.Instantiate<ParticleSystem> (hitParticles);
		hitParticles.enableEmission = false;
		hitParticles.gameObject.SetActive(false);
	}

	public override void DoHitStart(ArcReactorHitInfo hit){
		//Update the hit position of the particle effect
		Debug.Log ("Laser Hit!");
		hitParticles.transform.position = hit.raycastHit.point;
		hitParticles.transform.LookAt(hit.raycastHit.point + hit.raycastHit.normal);

		hitParticles.gameObject.SetActive (true);
		hitParticles.enableEmission = true;
	}

	public override void DoHitContinue(ArcReactorHitInfo hit){
		//Update the hit position of the particle effect
		hitParticles.transform.position = hit.raycastHit.point;
		hitParticles.transform.LookAt(hit.raycastHit.point + hit.raycastHit.normal);
	}

	public override void DoHitEnd()
	{
		Debug.Log ("Laser Lost!");
		hitParticles.enableEmission = false;
	}
}
