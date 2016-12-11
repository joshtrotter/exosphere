using UnityEngine;
using System.Collections;

public class SkyboxRotator : MonoBehaviour {

	private Material skybox;
	public float rotateSpeed = 0.2f;
	public bool rotateClockwise = false;
	public float initialRotation = 0f;

	void Awake(){
		skybox = RenderSettings.skybox;
	}
	
	void FixedUpdate()
	{
		skybox.SetFloat("_Rotation", (initialRotation + (Time.time * rotateSpeed * (rotateClockwise ? -1 : 1))) % 360);
	}
}
