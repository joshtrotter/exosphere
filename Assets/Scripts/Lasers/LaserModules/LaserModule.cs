using UnityEngine;
using System.Collections;

public class LaserModule : MonoBehaviour {

	public bool IsEnabled = true;

	public void Enable(){
		IsEnabled = true;
	}

	public void Disable(){
		IsEnabled = false;
	}

	public virtual void DoHitStart(ArcReactorHitInfo hitInfo){

	}

	public virtual void DoHitContinue(ArcReactorHitInfo hitInfo){

	}

	public virtual void DoHitEnd(){

	}

}
