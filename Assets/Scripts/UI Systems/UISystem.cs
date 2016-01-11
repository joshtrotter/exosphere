using UnityEngine;
using System.Collections;

/* base class for all UISystems managed by the UISystemController */
public abstract class UISystem : MonoBehaviour {

	private UISystemController controller;

	public virtual void Awake(){
		controller = GetComponentInParent<UISystemController> ();
		Hide ();
	}

	public virtual void RequestToBeShown(){
		controller.RegisterRequest (this);
	}

	public virtual void ShowRequestAccepted(){
		Show ();
	}
	
	public virtual void Deregister(){
		controller.Deregister (this);
	}

	public abstract void Show ();

	public abstract void Hide ();

}
