using UnityEngine;
using System.Collections;

public class SeeSawController : MonoBehaviour {

	private TransformController transformController;

	public float massBase = 0.5f;
	public float massBias = 0.75f;
	public bool biasStartsAtBottom = true;

	public Rigidbody rampBottom;
	public Rigidbody rampTop;

	void Awake() {
		transformController = GameObject.FindGameObjectWithTag ("Player").GetComponent<TransformController> ();
		rampBottom.mass = biasStartsAtBottom ? massBias : massBase;
		rampTop.mass = biasStartsAtBottom ? massBase : massBias;
	}
	
	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player") && !ShouldIgnoreBall()) {
			RemoveBias();
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player") && !ShouldIgnoreBall()) {
			ResetBias(coll);
		}
	}

	private void RemoveBias() {
		rampBottom.mass = massBase;
		rampTop.mass = massBase;
	}

	public void ResetBias(Collider coll) {
		float bottomRampDistance = (coll.transform.position - rampBottom.transform.position).magnitude;
		float topRampDistance = (coll.transform.position - rampTop.transform.position).magnitude;
		
		if (bottomRampDistance < topRampDistance) {
			rampBottom.mass = massBias;
		} else {
			rampTop.mass = massBias;
		}
	}

	private bool ShouldIgnoreBall() {
		return transformController.currentTransform.GetType () == typeof(LightTransform);
	}
}
