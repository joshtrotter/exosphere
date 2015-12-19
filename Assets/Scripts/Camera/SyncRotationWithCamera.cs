using UnityEngine;
using System.Collections;

public class SyncRotationWithCamera : MonoBehaviour {

	Transform cam;

	void Awake()
	{
		cam = GameObject.FindWithTag ("CameraRig").transform;
	}

	void Update(){
		transform.rotation = cam.rotation;
	}
}
