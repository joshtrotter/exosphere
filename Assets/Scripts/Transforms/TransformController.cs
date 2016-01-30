using UnityEngine;
using System.Collections;

public class TransformController : MonoBehaviour {

	//Store a reference to the standard transform. This is the state we start off in and that we return to whenever a transform is removed.
	public BallTransform standardTransform;
	public BallTransform currentTransform;

	public ParticleSystem morphEffect;

	//Control the force and duration of the atomizer explosion when shaking off a transform
	public float atomizerForce = 15f;
	public float atomizerDuration = 1f;

	private BallController ballController;

	void Awake() 
	{
		ballController = GetComponent<BallController> ();
		if (currentTransform == null) {
			currentTransform = standardTransform;
		}
	}

	public void ApplyTransform(BallTransform newTransform)
	{
		if (newTransform != currentTransform) {
			currentTransform.Remove (ballController);
			newTransform.Apply (ballController);
			currentTransform = newTransform;
			if (currentTransform != standardTransform) {
				morphEffect.Play();
				HUD.controller.SendMessage("MorphApplied", currentTransform);
			}
		}
	}

	public void RemoveCurrent()
	{
		if (currentTransform != standardTransform) {
			Atomize ();
			ApplyTransform(standardTransform);
			HUD.controller.SendMessage("MorphRemoved");
		}
	}

	//Simulates the apperance of the current material bursting off the ball during a transform
	private void Atomize() {
		AtomizerEffectGroup atomizerEffect = Atomizer.CreateEffectGroup();

		Pop pop = new Pop();
		pop.Force = atomizerForce;
		pop.Duration = atomizerDuration;

		atomizerEffect.Combine (pop);
		Atomizer.Atomize (gameObject, atomizerEffect, () => {});
	}
}
