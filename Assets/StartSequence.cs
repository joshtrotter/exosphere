using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StartSequence : MonoBehaviour {

	public ParticleSystem spawnEffect;
	public float cameraWaitTime = 15f;
	
	private GameObject startSpawn;
	private GameObject cameraRig;

	private bool completed = false;

	// Use this for initialization
	void Awake () {
		startSpawn = GameObject.FindGameObjectWithTag ("StartSpawn");
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig");
		spawnEffect.transform.position = startSpawn.transform.position;
	}
	
	public void init() {
		cameraRig.GetComponent<SplineController> ().enabled = true;
		cameraRig.GetComponent<SplineInterpolator> ().enabled = true;
	}

	public bool IsCompleted() {
		return completed;
	}

	private void DoSpawn() {
		cameraRig.transform.DOMoveY (cameraRig.transform.position.y - 8, spawnEffect.duration).Play ().OnComplete (Finalise);		
		spawnEffect.Play ();
	}

	private void Finalise() {
		completed = true;
	}
	
}
