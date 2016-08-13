using UnityEngine;
using System.Collections;

public class BallDestroyer : MonoBehaviour {

	//how long the ball will stay destroyed for before a reload occurs
	public float reloadDelay = 2f;
	public ParticleSystem shatter;
	public ParticleSystem pop;
	private MeshRenderer mesh;
	private Rigidbody rb;
	private Collider coll;

	void Awake(){
		rb = GetComponent<Rigidbody> ();
		mesh = GetComponent<MeshRenderer> ();
		coll = GetComponent<Collider> ();
	}

	public void Shatter(){
		StartCoroutine(DestroyBall (shatter));
	}

	public void Pop(){
		StartCoroutine (DestroyBall (pop));
	}

	//destroy ball based on transform
	public void Crush(){
		if (GetComponent<TransformController> ().currentTransform.morphName == "Aero Ball") {
			Pop ();
		} else {
			Shatter ();
		}
	}

	private IEnumerator DestroyBall(ParticleSystem destruction){
		rb.isKinematic = true;
		mesh.enabled = false;
		coll.enabled = false;
		destruction.Play ();
		yield return new WaitForSeconds (reloadDelay);
		LevelManager.manager.ReloadLevel ();
		//Application.LoadLevel (2);
	}
}
