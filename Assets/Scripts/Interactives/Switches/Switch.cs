using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public bool IsOn = true;
	//Switch is on when the lightcone is green, off when it orange
	
	//store a reference to the object to be activated by the switch
	public SwitchableObject target;
	
	public virtual void SwapState(){
		if (IsOn) {
			TurnOff();
		} else {
			TurnOn();
		}
	}
	
	public virtual void TurnOn(){
		IsOn = true;
	}
	
	public virtual void TurnOff(){
		IsOn = false;
	}

	public virtual void OnLaserEnter(){

	}

	public virtual void OnLaserExit(){

	}
}
