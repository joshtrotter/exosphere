using UnityEngine;
using System.Collections;

public class PulseLight : MonoBehaviour {

	public float minIntensity = 6f;
	public float maxIntensity = 8f;
	public float minRange = 10f;
	public float maxRange = 12.5f;

	private Light pulseLight;
	private float bounceValue;

	// Use this for initialization
	void Awake () {
		pulseLight = GetComponent<Light> ();
		bounceValue = Random.Range (0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		bounceValue = Mathf.PingPong (Time.time, 1);
		pulseLight.intensity = Mathf.Lerp (minIntensity, maxIntensity, bounceValue);
		pulseLight.range = Mathf.Lerp (minRange, maxRange, bounceValue);
	}
}
