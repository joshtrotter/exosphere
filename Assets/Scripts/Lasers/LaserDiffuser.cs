using UnityEngine;
using System.Collections;

/**
 * Used to diffuse a new ray from a ray hit 
 */
public class LaserDiffuser : ArcReactor_Launcher {

	public void Diffuse(ArcReactorHitInfo hit)
	{
		//Launch a new ray and set it to the same lifecycle stage as the source ray so that they will appear as one continuous ray
		this.LaunchRay ();
		this.rays [0].arc.elapsedTime = hit.rayInfo.arc.elapsedTime;
	}

	public void Disable()
	{
		foreach (RayInfo ray in rays) {
			Destroy (ray.arc.gameObject);
		}
	}
}
