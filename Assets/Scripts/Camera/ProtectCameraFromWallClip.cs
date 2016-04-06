using System;
using System.Collections;
using UnityEngine;

/**
 * This script is taken from the Unity Standard Assets project - it prevents the camera from
 * being obscured by obstacles in front of the player.
 */ 
namespace UnityStandardAssets.Cameras
{
	public class ProtectCameraFromWallClip : MonoBehaviour
	{
		public float clipMoveTime = 0f;                 // time taken to move when avoiding clipping (low value = fast, which it should be)
		public float returnTime = 0.2f;                 // time taken to move back towards desired position, when not clipping (typically should be a higher value than clipMoveTime)
		public float sphereCastRadius = 0.1f;           // the radius of the sphere used to test for object between camera and target        
		public float closestDistance = 0f;              // the closest distance the camera can be from the pivot
		public float closestPivotDistance = 0.5f;         // the closest distance the pivot can be from the ball
		public float trailDistanceToDropPivot = 2.5f;   // the distance at which moving the camera back towards the pivot also causes the pivot to drop towards the ball
		public float blockedPivotBufferHeight = 0.2f;   // places a buffer between the pivot and the collider that is blocking it from the ball
		public string dontClipTag = "Player";           // don't clip against objects with this tag (useful for not clipping against the targeted object)

		private Transform cam;                  		// the transform of the camera
		private Transform pivot;                		// the point at which the camera pivots around
        
		private float originalCamTrailDistance;         // the original distance to the camera from the pivot before any modification are made
		private float originalPivotHeight;				// the original distance from the ball to the pivot
        
		private float camMoveVelocity;             		// the velocity at which the camera moved
		private float pivotMoveVelocity;             	// the velocity at which the pivot moved
        
		private float currentCamTrailDistance;          // the current distance from the camera to the pivot
		private float currentPivotHeight;				// the current distance from the ball to the pivot

		private Ray ray;                        		// the ray used in the lateupdate for detecting obstructions between the camera and pivot or the pivot and ball
		private RaycastHit[] hits;              		// the ray cast hits
		private RayHitComparer rayHitComparer;  		// variable to compare raycast hit distances


		private void Start ()
		{
			// find the camera in the object hierarchy
			cam = GetComponentInChildren<Camera> ().transform;
			pivot = cam.parent;
            
			originalCamTrailDistance = cam.localPosition.magnitude;
			originalPivotHeight = pivot.localPosition.y;
            
			currentCamTrailDistance = originalCamTrailDistance;
			currentPivotHeight = originalPivotHeight;

			// create a new RayHitComparer
			rayHitComparer = new RayHitComparer ();
		}

		private void LateUpdate() 
		{
			CheckForClipBetweenBallAndPivot ();
			CheckForClipBetweenPivotAndCamera ();
		}

		private void CheckForClipBetweenBallAndPivot() 
		{
			bool hitSomething = false;
			float targetHeight = originalPivotHeight;
			
			//Use the ball as the ray origin
			ray.origin = transform.position;
			//Cast ray upwards towards the pivot
			ray.direction = Vector3.up;
			
			hits = Physics.RaycastAll (ray, targetHeight);
			Array.Sort (hits, rayHitComparer);
			
			// set the variable used for storing the closest to be as far as possible
			float nearest = Mathf.Infinity;					
			
			// loop through all the collisions
			for (int i = 0; i < hits.Length; i++) {
				// only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
				if (IsValidCollider(hits[i], nearest)) {					
					// change the nearest collision to latest
					nearest = hits [i].distance;
					targetHeight = transform.InverseTransformPoint (hits [i].point).y - blockedPivotBufferHeight;
					hitSomething = true;
				}
			}
			
			// visualise the cam clip effect in the editor
			if (hitSomething)
			{
				Debug.DrawRay (ray.origin, Vector3.up * (targetHeight), Color.red);
			}
			
			adjustPivot (targetHeight);
		}

		private void CheckForClipBetweenPivotAndCamera ()
		{
			// initially set the target distance
			float targetDist = originalCamTrailDistance;

			// set the origin of the sphere in front of the camera
			ray.origin = pivot.position + pivot.forward * sphereCastRadius;
			ray.direction = -pivot.forward;

			// initial check to see if start of spherecast intersects anything
			var cols = Physics.OverlapSphere (ray.origin, sphereCastRadius);

			bool initialIntersect = false;
			bool hitSomething = false;

			// loop through all the collisions to check if something we care about
			for (int i = 0; i < cols.Length; i++) {
				if ((!cols [i].isTrigger) &&
					!(cols [i].attachedRigidbody != null && cols [i].attachedRigidbody.CompareTag (dontClipTag))) {
					initialIntersect = true;
					break;
				}
			}

			// if there is a collision
			if (initialIntersect) {
				ray.origin += pivot.forward * sphereCastRadius;

				// do a raycast and gather all the intersections
				hits = Physics.RaycastAll (ray, originalCamTrailDistance - sphereCastRadius);
			} else {
				// if there was no collision do a sphere cast to see if there were any other collisions
				hits = Physics.SphereCastAll (ray, sphereCastRadius, originalCamTrailDistance + sphereCastRadius);
			}

			// sort the collisions by distance
			Array.Sort (hits, rayHitComparer);

			// set the variable used for storing the closest to be as far as possible
			float nearest = Mathf.Infinity;

			// loop through all the collisions
			for (int i = 0; i < hits.Length; i++) {
				// only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
				if (IsValidCollider (hits[i], nearest)) {
					// change the nearest collision to latest
					nearest = hits [i].distance;
					targetDist = -pivot.InverseTransformPoint (hits [i].point).z;
					hitSomething = true;
				}
			}

			// visualise the cam clip effect in the editor
            if (hitSomething)
            {
				Debug.DrawRay (ray.origin, -pivot.forward * (targetDist + sphereCastRadius), Color.red);
            }

			// hit something so move the camera to a better position            
			adjustCam (targetDist);

			// keep the ball in view
			if (currentCamTrailDistance < trailDistanceToDropPivot) {
				adjustPivot(Mathf.Min(currentPivotHeight, originalPivotHeight - (trailDistanceToDropPivot - currentCamTrailDistance)));
			}
		}

		private void adjustPivot(float targetHeight) {
			currentPivotHeight = Mathf.SmoothDamp (currentPivotHeight, targetHeight, ref pivotMoveVelocity,
			                                       currentPivotHeight > targetHeight ? clipMoveTime : returnTime);
			currentPivotHeight = Mathf.Clamp (currentPivotHeight, closestPivotDistance, originalPivotHeight);
			pivot.localPosition = Vector3.up * currentPivotHeight;
		}

		private void adjustCam(float targetDist) {
			currentCamTrailDistance = Mathf.SmoothDamp (currentCamTrailDistance, targetDist, ref camMoveVelocity,
			                                            currentCamTrailDistance > targetDist ? clipMoveTime : returnTime);
			currentCamTrailDistance = Mathf.Clamp (currentCamTrailDistance, closestDistance, originalCamTrailDistance);
			cam.localPosition = -Vector3.forward * currentCamTrailDistance;
		}

		private bool IsValidCollider(RaycastHit hit, float nearest)
		{
			return hit.distance < nearest && 
				(!hit.collider.isTrigger) &&
				!(hit.collider.attachedRigidbody != null &&
				  hit.collider.attachedRigidbody.CompareTag (dontClipTag));
		}


		// comparer for check distances in ray cast hits
		public class RayHitComparer : IComparer
		{
			public int Compare (object x, object y)
			{
				return ((RaycastHit)x).distance.CompareTo (((RaycastHit)y).distance);
			}
		}
	}
}
