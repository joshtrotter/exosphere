using UnityEngine;
using System.Collections;

public abstract class BallTransform : MonoBehaviour
{
	public Material transformMaterial;
	public PhysicMaterial transformPhysicMaterial; 
	public string morphName;

	public virtual void Apply(BallController ball) 
	{
		ball.GetComponent<Renderer> ().material = transformMaterial;
		ball.GetComponent<Collider> ().material = transformPhysicMaterial;
	}

	public virtual void Remove(BallController ball)
	{

	}

	public virtual void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{

	}

	public virtual void OnLaserExit(LaserDiffuser laserDiffuser)
	{

	}
	
}
