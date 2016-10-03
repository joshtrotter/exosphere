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

	public Sprite morphIcon;
	public Color morphColor;

	public float minEmission = 0.1f;
	public float maxEmission = 1.5f;
	public float brakeEmission = 5f;

	public string morphEffectName;
	
	public virtual void Apply(BallController ball) 
	{
		ball.GetComponent<Renderer> ().material = transformMaterial;
		ball.GetComponent<Collider> ().material = transformPhysicMaterial;

		EnablePhysicalModifiers (ball);
	}

	public virtual void Remove(BallController ball)
	{
		DisablePhysicalModifiers (ball);
	}

	public virtual void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{
		laserDiffuser.GetComponentInParent<BallDestroyer>().Pop();
	}

	public virtual void OnLaserExit(LaserDiffuser laserDiffuser)
	{

	}

	public void EnablePhysicalModifiers(BallController ball) {
		ball.GetComponent<Rigidbody> ().mass *= ballMassScale;
		ball.movePower *= ballMovePowerScale;
		ball.GetComponent<Rigidbody> ().maxAngularVelocity += ballMaxAngularVelocityModifier;
	}

	public void DisablePhysicalModifiers(BallController ball) {
		ball.GetComponent<Rigidbody> ().mass /= ballMassScale;
		ball.movePower /= ballMovePowerScale;
		ball.GetComponent<Rigidbody> ().maxAngularVelocity -= ballMaxAngularVelocityModifier;
	}
		
}
