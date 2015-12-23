using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public const float dimIntensity = 0.35f;

	void Awake () {
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
	}

}
