using UnityEngine;
using System.Collections;

public class DoorController : SwitchableObject {

	//create an array to store all children doors the doorcontroller needs to take care of
	private Door[] doors;

	public override void Activate() {
		SwitchActivation ();
	}

	void Awake(){
		doors = GetComponentsInChildren<Door>();
	}

	void SwitchActivation(){
		foreach (Door door in doors) {
			door.Activate ();
		}
	}

}
