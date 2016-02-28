using UnityEngine;
using System.Collections;

public class PressureControlledElevator : PressureReceiver {
	
	//public float height = 4f;
	public Vector3 posChange = new Vector3 (0, 4, 0);
	//public bool pressureToRaise = true;

	//private float baseY;
	private Vector3 startPos;

	void Awake() {
		//this.baseY = transform.position.y;
		startPos = transform.position;
	}

	public override void Apply (float pressureAmount)
	{
		/*Vector3 newPos = transform.position;
		//newPos.y = baseY + Mathf.Clamp01 (pressureAmount) * height * (pressureToRaise ? 1 : -1);
		newPos = */
		transform.position = startPos + Mathf.Clamp01 (pressureAmount) * posChange;
	}

	void OnTriggerEnter(Collider coll){
		//Debug.Log ("Staying in trigger");
		if (coll.CompareTag ("Player")) {
			coll.transform.parent = this.transform;
		}
	}

	void OnTriggerExit(Collider coll){
		//Debug.Log ("exiting trigger");
		coll.transform.parent = null;
	}

	public override void ReloadState (int state){}
}
;