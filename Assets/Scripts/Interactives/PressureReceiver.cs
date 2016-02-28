using UnityEngine;
using System.Collections;

public abstract class PressureReceiver : HasLevelState {
	
	public abstract void Apply(float pressureAmount);

	public override abstract void ReloadState(int state);

}
