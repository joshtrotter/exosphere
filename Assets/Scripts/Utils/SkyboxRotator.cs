using UnityEngine;
using System.Collections;

public class SkyboxRotator : MonoBehaviour {

	private Material skybox;
	public float rotateSpeed = 0.2f;
	public bool rotateClockwise = false;

	void Awake(){
		skybox = RenderSettings.skybox;
	}
	
	void FixedUpdate()
	{
		skybox.SetFloat("_Rotation", (Time.time * rotateSpeed * (rotateClockwise ? -1 : 1)) % 360);
	}
}
