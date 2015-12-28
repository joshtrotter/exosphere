using UnityEngine;
using System.Collections;

/* The purpose of this script is to control the locking and unlocking of all manual doors in children objects
 * This script should be the target of the unlocking mechanism
 */
public class LockController : SwitchableObject {

	private Door[] doors;
	public bool IsLocked = true;

	void Awake(){
		Debug.Log (GetInstanceID () + " awake");
		doors = GetComponentsInChildren<Door> ();
		Debug.Log (doors[0], doors[1]);
		IsLocked = !IsLocked;
		SwapState ();
	}

	public override void Activate() {
		SwapState ();
	}
	
	public void SwapState(){
		if (IsLocked) {
			Unlock ();		
		} else {
			Lock ();
		}
	}

	public void Unlock(){
		IsLocked = false;
		foreach (Door door in doors) {
			door.Unlock ();
		}
	}

	public void Lock(){
		IsLocked = true;
		foreach (Door door in doors) {
			door.Lock ();
		}
	}
}
