using UnityEngine;
using System.Collections;

public abstract class Door : SwitchableObject {

	public bool IsClosed = true;

	//called on SwitchableObject by a switch
	public override void Activate() {
		SwapState ();
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

	}
	
	public virtual void Open ()
	{

	}
}