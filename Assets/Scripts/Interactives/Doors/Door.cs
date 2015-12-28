using UnityEngine;
using System.Collections;

public abstract class Door : SwitchableObject {

	public bool IsClosed = true;

	//[HideInInspector] //locking is managed by DoorLock script
	public bool IsLocked = false;

	//called on SwitchableObject by a switch
	public override void Activate() {
		//door will only be activated if it is not locked
		if (!IsLocked) {
			SwapState ();
		}
	}
	
	public virtual void SwapState(){
		if (IsClosed) {
			Open ();		
		} else {
			Close ();
		}
	}
	
	public virtual void Close ()
	{
		IsClosed = true;
	}
	
	public virtual void Open ()
	{
		IsClosed = false;
	}

	public virtual void Lock ()
	{
		IsLocked = true;
	}
	
	public virtual void Unlock ()
	{
		IsLocked = false;
	}
}