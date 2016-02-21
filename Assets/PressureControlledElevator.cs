using UnityEngine;
using System.Collections;

public class PressureControlledElevator : PressureReceiver {
	
	public float height = 4f;
	public Vector3 posChange = new Vector3 (0, 4, 0);
	public bool pressureToRaise = true;

	private float baseY;
	private Vector3 startPos;

	void Awake() {
		this.baseY = transform.position.y;
		startPos = transform.position;
	}

	public override void Apply (float pressureAmount)
	{
		Vector3 newPos = transform.position;
		//newPos.y = baseY + Mathf.Clamp01 (pressureAmount) * height * (pressureToRaise ? 1 : -1);
		newPos = startPos + Mathf.Clamp01 (pressureAmount) * posChange;
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