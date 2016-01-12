using UnityEngine;
using System.Collections;

public class BallDestroyer : MonoBehaviour {

	//how long the ball will stay destroyed for before a reload occurs
	public float reloadDelay = 2f;
	public ParticleSystem shatter;
	private MeshRenderer mesh;
	private Rigidbody rb;

	void Awake(){
		rb = GetComponent<Rigidbody> ();
		mesh = GetComponent<MeshRenderer> ();
	}

	public void Shatter(){
		StartCoroutine(DestroyBall (shatter));

	}

	private IEnumerator DestroyBall(ParticleSystem destruction){
		rb.isKinematic = true;
		mesh.enabled = false;
		destruction.Play ();
		yield return new WaitForSeconds (reloadDelay);
		LevelManager.manager.ReloadLevel ();
		//Application.LoadLevel (2);
	}
}
