using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DestructibleWallPiece : DestructibleObject {

	private bool hasMoved;
	private Vector3 startPos;
	private Rigidbody rb;
	public float destroyDelay = 5f;

	void Awake(){
		startPos = transform.position;
		rb = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision coll){
		if (!hasMoved && Vector3.Distance(transform.position, startPos) > 0.5){
			StartCoroutine(WaitToDie());
			hasMoved = true;
		}
	}

	private IEnumerator WaitToDie(){
		Debug.Log ("Waiting to die");
		yield return new WaitForSeconds (destroyDelay);
		Debug.Log ("Fading"	);
		GetComponent<Renderer> ().material.DOFade (0, 1).Play ().OnComplete (DestroyObject);
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.CompareTag("Player")){
			TransformController	transformController = coll.GetComponent<TransformController>();
			if (transformController.currentTransform.GetType() != typeof(HeavyTransform) && !hasMoved){ 
				rb.isKinematic = true;
			}
		}
	}
	
	void OnTriggerExit(Collider coll){
		if (coll.gameObject.CompareTag("Player")){
			rb.isKinematic = false;
		}
	}
}
