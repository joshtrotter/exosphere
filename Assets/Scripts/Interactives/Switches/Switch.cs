using UnityEngine;
using System.Collections;

public class Switch : HasLevelState {

	protected const int ON_STATE = 0;
	protected const int OFF_STATE = 1;

	//store a reference to the object to be activated by the switch
	public SwitchableObject[] targets;
	
	public virtual void SwapState(){
		if (currentState == ON_STATE) {
			TurnOff();
		} else {
			TurnOn();
		}
	}
	
	public virtual void TurnOn(){
		RegisterStateChange (ON_STATE);
		foreach (SwitchableObject target in targets) {
			target.Activate ();
		}
	}
	
	public virtual void TurnOff(){
		RegisterStateChange (OFF_STATE);
		foreach (SwitchableObject target in targets) {
			target.Activate ();
		}
	}

	public override void ReloadState(int state) {
		if (state != currentState) {
			SwapState();
		}
	}

	public virtual void OnLaserEnter(){

	}

	public virtual void OnLaserExit(){

	}
}
