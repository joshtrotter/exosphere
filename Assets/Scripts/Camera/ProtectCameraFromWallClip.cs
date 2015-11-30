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
        public float clipMoveTime = 0.05f;              // time taken to move when avoiding clipping (low value = fast, which it should be)
        public float returnTime = 0.4f;                 // time taken to move back towards desired position, when not clipping (typically should be a higher value than clipMoveTime)
        public float sphereCastRadius = 0.1f;           // the radius of the sphere used to test for object between camera and target        
        public float closestDistance = 0.5f;            // the closest distance the camera can be from the target        
        public string dontClipTag = "Player";           // don't clip against objects with this tag (useful for not clipping against the targeted object)

		private Transform cam;                  // the transform of the camera
		private Transform pivot;                // the point at which the camera pivots around
        private float originalDistance;             // the original distance to the camera before any modification are made
        private float moveVelocity;             // the velocity at which the camera moved
        private float currentDistance;              // the current distance from the camera to the target
        private Ray ray;                        // the ray used in the lateupdate for casting between the camera and the target
        private RaycastHit[] hits;              // the hits between the camera and the target
        private RayHitComparer rayHitComparer;  // variable to compare raycast hit distances


        private void Start()
        {
            // find the camera in the object hierarchy
			cam = GetComponentInChildren<Camera>().transform;
            pivot = cam.parent;
            originalDistance = cam.localPosition.magnitude;
            currentDistance = originalDistance;

            // create a new RayHitComparer
            rayHitComparer = new RayHitComparer();
        }


        private void LateUpdate()
        {
            // initially set the target distance
            float targetDist = originalDistance;

			// set the origin of the sphere in front of the camera
            ray.origin = pivot.position + pivot.forward*sphereCastRadius;
            ray.direction = -pivot.forward;

            // initial check to see if start of spherecast intersects anything
            var cols = Physics.OverlapSphere(ray.origin, sphereCastRadius);

            bool initialIntersect = false;
            bool hitSomething = false;

            // loop through all the collisions to check if something we care about
            for (int i = 0; i < cols.Length; i++)
            {
                if ((!cols[i].isTrigger) &&
                    !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.CompareTag(dontClipTag)))
                {
                    initialIntersect = true;
                    break;
                }
            }

            // if there is a collision
            if (initialIntersect)
            {
                ray.origin += pivot.forward*sphereCastRadius;

                // do a raycast and gather all the intersections
                hits = Physics.RaycastAll(ray, originalDistance - sphereCastRadius);
            }
            else
            {
                // if there was no collision do a sphere cast to see if there were any other collisions
                hits = Physics.SphereCastAll(ray, sphereCastRadius, originalDistance + sphereCastRadius);
            }

            // sort the collisions by distance
            Array.Sort(hits, rayHitComparer);

            // set the variable used for storing the closest to be as far as possible
            float nearest = Mathf.Infinity;

            // loop through all the collisions
            for (int i = 0; i < hits.Length; i++)
            {
                // only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
                if (hits[i].distance < nearest && (!hits[i].collider.isTrigger) &&
                    !(hits[i].collider.attachedRigidbody != null &&
                      hits[i].collider.attachedRigidbody.CompareTag(dontClipTag)))
                {
                    // change the nearest collision to latest
                    nearest = hits[i].distance;
                    targetDist = -pivot.InverseTransformPoint(hits[i].point).z;
                    hitSomething = true;
                }
            }

            // visualise the cam clip effect in the editor
            if (hitSomething)
            {
                Debug.DrawRay(ray.origin, -pivot.forward*(targetDist + sphereCastRadius), Color.red);
            }

            // hit something so move the camera to a better position            
            currentDistance = Mathf.SmoothDamp(currentDistance, targetDist, ref moveVelocity,
                                           currentDistance > targetDist ? clipMoveTime : returnTime);
            currentDistance = Mathf.Clamp(currentDistance, closestDistance, originalDistance);
            cam.localPosition = -Vector3.forward*currentDistance;
        }


        // comparer for check distances in ray cast hits
        public class RayHitComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return ((RaycastHit) x).distance.CompareTo(((RaycastHit) y).distance);
            }
        }
    }
}
