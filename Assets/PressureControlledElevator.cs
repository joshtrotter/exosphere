using UnityEngine;
using System.Collections;

public class PressureControlledElevator : PressureReceiver {
	
	public float height = 4f;
	public bool pressureToRaise = true;

	private float baseY;

	void Awake() {
		this.baseY = transform.position.y;
	}

	public override void Apply (float pressureAmount)
	{
		Vector3 newPos = transform.position;
		newPos.y = baseY + Mathf.Clamp01 (pressureAmount) * height * (pressureToRaise ? 1 : -1);
		transform.position = newPos;
	}

	void OnCollisionStay(Collision coll){
		coll.transform.parent = this.transform;
	}

	void OnCollisionExit(Collision coll){
		coll.transform.parent = null;
	}
}
;