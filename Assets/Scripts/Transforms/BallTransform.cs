using UnityEngine;
using System.Collections;

public abstract class BallTransform : MonoBehaviour
{
	public Material transformMaterial;
	public PhysicMaterial transformPhysicMaterial;

	public float ballMassScale = 1f;
	public float ballMovePowerScale = 1f;
	public float ballMaxAngularVelocityModifier = 0f;

	public string morphName;

	public virtual void Apply(BallController ball) 
	{
		ball.GetComponent<Renderer> ().material = transformMaterial;
		ball.GetComponent<Collider> ().material = transformPhysicMaterial;

		ball.GetComponent<Rigidbody> ().mass *= ballMassScale;
		ball.movePower *= ballMovePowerScale;
		ball.GetComponent<Rigidbody> ().maxAngularVelocity += ballMaxAngularVelocityModifier;
	}

	public virtual void Remove(BallController ball)
	{
		ball.GetComponent<Rigidbody> ().mass /= ballMassScale;
		ball.movePower /= ballMovePowerScale;
		ball.GetComponent<Rigidbody> ().maxAngularVelocity -= ballMaxAngularVelocityModifier;
	}

	public virtual void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{

	}

	public virtual void OnLaserExit(LaserDiffuser laserDiffuser)
	{

	}
	
}
