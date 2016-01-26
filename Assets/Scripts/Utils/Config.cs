using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public const float softDimIntensity = 0.35f;
	public const float hardDimIntensity = 0.2f;

	void Awake () {
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
	}
}
