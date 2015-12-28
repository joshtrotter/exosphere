using UnityEngine;
using System.Collections;

/* Laser launchers which should snap to objects should inherit from this class
 * Lasers will snap to objects which have the LaserSnapTo laser module and a laserHitManager attached
 * */
public class LaserSnapFrom : ArcReactor_Launcher {

	private Quaternion startRotation;
	private bool snapped = false;
	private LaserSnapTo target;

	//the amount in the degrees the laser will turn to follow the object
	public float followThreshold = 10f;
	//the speed at which the laser will lerp to the required position
	public float turnSpeed = 10f;


	public virtual void Start(){
		startRotation = this.transform.localRotation;
	}
	
	public void StartSnap(LaserSnapTo snapTarget)
	{
		snapped = true;
		target = snapTarget;
		StartCoroutine (Follow ());
	}

	public void EndSnap(){
		snapped = false;
	}
	
	//this function causes the laser to follow the object while within the follow threshold
	private IEnumerator Follow()
	{
		while (snapped) { 
			yield return new WaitForFixedUpdate ();
			Quaternion targetRot = Quaternion.LookRotation(target.transform.position-transform.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);

			//compare source's current rotation angle with it's original and decide whether it exceeds the followThreshold
			if (Quaternion.Angle (transform.localRotation, startRotation) > followThreshold) {
				snapped = false;
			}
		}
		StartCoroutine(ReturnToNorm());
	}

	//this function returns the laser to its normal rotation
	private IEnumerator ReturnToNorm(){
		//Debug.Log ("Returning to normal");
		const float error = 1e-12f;
		while (Quaternion.Angle(transform.localRotation,startRotation) > error && !snapped) {
			transform.localRotation = Quaternion.Lerp (transform.localRotation, startRotation, turnSpeed * Time.fixedDeltaTime);
			//transform.localRotation = startRotation;
			yield return new WaitForFixedUpdate();
		}
		//make return exact
		transform.localRotation = startRotation;
	}
}

