using UnityEngine;
using System.Collections;

public abstract class PressureReceiver : MonoBehaviour {
	
	public abstract void Apply(float pressureAmount);

}
