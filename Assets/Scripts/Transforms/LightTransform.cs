using UnityEngine;
using System.Collections;

public class LightTransform : BallTransform {

	public override void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo){
		GameObject.FindGameObjectWithTag("Player").GetComponent<BallDestroyer>().Pop();
	}
}
