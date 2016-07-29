using UnityEngine;
using System.Collections;

public class TunnelStandardTransform : StandardTransform {

	public override void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo){
		TunnelRunnerCompleteScreen.controller.PopDisplayAndReload ();
	}
}
