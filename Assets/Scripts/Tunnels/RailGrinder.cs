﻿using UnityEngine;
using System.Collections;

public class RailGrinder : MonoBehaviour {

	private ParticleSystem sparks;
	private TunnelScoreController scoreController;
	private bool registeredSelfWithScorer;

	void Awake() {
		sparks = transform.parent.parent.FindChild("Sparks").GetComponentInParent<ParticleSystem> ();
		scoreController = GameObject.FindGameObjectWithTag ("Player").GetComponent<TunnelScoreController> ();
	}

	// Use this for initialization
	void OnCollisionStay (Collision collision) {
		collision.collider.GetComponent<BallController> ().SetGrind (true);
		Vector3 sparkPos = sparks.transform.position;
		sparkPos.z = collision.contacts [0].point.z;
		sparks.transform.position = sparkPos;
		sparks.transform.LookAt (collision.contacts [0].point + (Vector3.up * 0.5f));
		if (!sparks.isPlaying) {
			sparks.Play();
			sparks.enableEmission = true;
		}
		if (!registeredSelfWithScorer) {
			scoreController.StartGrind ();
			registeredSelfWithScorer = true;
		}
	}

	void OnCollisionExit (Collision collision) {
		collision.collider.GetComponent<BallController> ().SetGrind (false);
		registeredSelfWithScorer = false;
//		sparks.enableEmission = false;
	}
}
