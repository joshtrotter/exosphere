using UnityEngine;
using System.Collections;

/*The purpose of this class is to make using lasers easier by snapping the laser to the object within
 * a certain threshold.
 */

public class LaserSnapperController : LaserModule {
	
	private ArcReactor_Launcher laserSource;
	private Quaternion sourceStartRotation;
	private bool snapped = false;

	//the amount in the degrees the laser will turn to follow the object
	public float followThreshold = 10f;

	public override void DoHitStart (ArcReactorHitInfo hitInfo)
	{
		snapped = true;
		laserSource = hitInfo.launcher;
		sourceStartRotation = laserSource.transform.localRotation;
		StartCoroutine (Follow ());
	}
	
	public override void DoHitEnd()
	{
		snapped = false;
		laserSource.transform.localRotation = sourceStartRotation;
	}
	
	//this function causes the laser to follow the object while within the follow threshold
	private IEnumerator Follow()
	{
		while (snapped) { 
			yield return new WaitForEndOfFrame ();
			laserSource.transform.LookAt (this.transform);
			//compare source's current transform with it's original and decide whether it exceeds the followThreshold
			Vector3 rotChange = laserSource.transform.localRotation.eulerAngles-sourceStartRotation.eulerAngles;
			if ((rotChange.x > followThreshold && rotChange.x < (360-followThreshold)) 
			    || (rotChange.y > followThreshold && rotChange.y < (360-followThreshold)) 
			    || (rotChange.z > followThreshold && rotChange.z < (360-followThreshold)))
			{
				snapped = false;
			}
		}
		DoHitEnd ();
	}

}