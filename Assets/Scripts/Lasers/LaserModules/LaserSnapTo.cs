using UnityEngine;
using System.Collections;

public class LaserSnapTo : LaserModule {
		
	private LaserSnapFrom source;

	public override void DoHitStart (ArcReactorHitInfo hitInfo)
	{
		source = (LaserSnapFrom)hitInfo.launcher;
		source.StartSnap (this);
	}

	//end a snap if there is one, can be called from other scripts such as the Glass Transform
	public override void DoHitEnd ()
	{
		if (source != null) {
			source.EndSnap ();
			source = null;
		}
	}

}