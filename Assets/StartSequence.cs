﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StartSequence : MonoBehaviour {

	public ParticleSystem spawnEffect;
	public float cameraWaitTime = 15f;
	
	private GameObject startSpawn;
	private GameObject cameraRig;
	private Transform cameraStartWaypoint;
	private Transform skipNotification;

	private bool completed = false;
	private bool splineStarted = false;
	private bool splineEnded = false;
	private bool splineSkipped = false;

	// Use this for initialization
	void Awake () {
		startSpawn = GameObject.FindGameObjectWithTag ("StartSpawn");
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig");
		cameraStartWaypoint = GameObject.FindGameObjectWithTag("CameraStartWaypoint").transform;
		skipNotification = transform.FindChild ("SkipNotification");
		spawnEffect.transform.position = startSpawn.transform.position;
	}
	
	public void init() {
		StartCoroutine (WaitForSplineToEnd ());
		StartCoroutine (CheckSplineState ());
		splineStarted = true;
		SetSkipNotificationActive (true);
		CameraFade.StartAlphaFade (Color.black, true, 2f);
		cameraRig.GetComponent<SplineController> ().enabled = true;
		cameraRig.GetComponent<SplineInterpolator> ().enabled = true;
	}

	public bool IsCompleted() {
		return completed;
	}

	private void DoSpawn() {
		SetSkipNotificationActive(false);
		SetCameraPosition ();
		cameraRig.transform.DOMoveY (cameraRig.transform.position.y - 8, spawnEffect.duration).Play ().OnComplete (Finalise);		
		spawnEffect.Play ();
	}

	void Update() {
		if (splineStarted && !splineEnded && !splineSkipped && (Input.touchCount > 0 || Input.GetMouseButtonDown (0))) {
			splineSkipped = true;
			SetSkipNotificationActive(false);
			CameraFade.StartAlphaFade (Color.black, false, 4f, 0f, () => EndSpline());
		}
	}

	private void SetSkipNotificationActive(bool active) {
		skipNotification.gameObject.SetActive (active);
	}

	private IEnumerator CheckSplineState() {
		while (!splineEnded && !completed) {
			yield return new WaitForEndOfFrame();
		}
		if (!completed) {
			DoSpawn ();
		}
	}

	private IEnumerator WaitForSplineToEnd() {
		yield return new WaitForSeconds (cameraWaitTime);
		splineEnded = true;
	}

	private void EndSpline() {
		Destroy (cameraRig.GetComponent<SplineController> ());
		Destroy (cameraRig.GetComponent<SplineInterpolator> ());
		//DoSpawn ();
		SetCameraPosition ();
		Finalise ();
	}

	private void SetCameraPosition() {
		cameraRig.transform.position = cameraStartWaypoint.position;
		cameraRig.transform.rotation = cameraStartWaypoint.rotation;
	}

	private void Finalise() {
		completed = true;
	}
	
}
