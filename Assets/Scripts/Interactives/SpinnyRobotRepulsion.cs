using UnityEngine;
using System.Collections;

public class SpinnyRobotRepulsion : MonoBehaviour {

	public float repulsePower = 3f;
	public float minScaleFactor = 0.5f;
	private AxisRotator robotRotator;

	// Use this for initialization
	void Start () {
		robotRotator = transform.parent.GetComponent<AxisRotator> ();
	}
	
	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.CompareTag ("Player")) {
			Debug.Log ("Hit! " + robotRotator.rotationsPerSecond);
			coll.rigidbody.AddForce (coll.contacts [0].normal * repulsePower * (robotRotator.rotationsPerSecond + minScaleFactor) * -1, ForceMode.Impulse);
		}
		//Debug.Log (coll.contacts [0].normal * repulsePower * powerModifier);
		Debug.DrawRay (this.transform.position, coll.contacts [0].normal * -100, Color.white, 10);
	}
}
