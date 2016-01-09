﻿using UnityEngine;
using System.Collections;

/* base class for all UISystems managed by the UISystemController */
public abstract class UISystem : MonoBehaviour {

	public virtual void RequestToBeShown(){
		UISystemController.controller.RegisterRequest (this);
	}

	public virtual void ShowRequestAccepted(){
		Show ();
	}
	
	public virtual void Deregister(){
		UISystemController.controller.Deregister (this);
	}

	public abstract void Show ();

	public abstract void Hide ();

}