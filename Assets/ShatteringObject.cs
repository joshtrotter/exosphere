using UnityEngine;
using System.Collections;

public class ShatteringObject : MonoBehaviour {

	private ParticleSystem shatter;
	private MeshRenderer mesh;
	private bool safeToSmash = false;

	void Awake () {
		mesh = this.GetComponent<MeshRenderer> ();
		shatter = this.GetComponentInChildren<ParticleSystem> ();
		StartCoroutine (delayInitialCollisions());
	}

	void OnLevelWasLoaded(){
		StartCoroutine (delayInitialCollisions());
	}

	/* Prevent the object from shattering on awake due to other level geometry */
	private IEnumerator delayInitialCollisions(){
		yield return new WaitForSeconds (1f);
		safeToSmash = true;
	}
	
	void OnCollisionEnter(Collision coll){
		//let other things break it if safe time has passed, or always break for player
		if (safeToSmash || coll.gameObject.CompareTag("Player")) {
			mesh.enabled = false;
			shatter.Play ();
			this.GetComponent<Collider> ().enabled = false;
		}
	}
}
