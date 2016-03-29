using UnityEngine;
using System.Collections;

public class TransformController : MonoBehaviour {

	//Store a reference to the standard transform. This is the state we start off in and that we return to whenever a transform is removed.
	public BallTransform standardTransform;
	public BallTransform currentTransform;

	public ParticleSystem shatter;

	private BallController ballController;
	private LightsController lightsController;

	void Awake() 
	{
		ballController = GetComponent<BallController> ();
		lightsController = GetComponent<LightsController> ();
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
			lightsController.SetLightColor(currentTransform.morphColor);
			if (currentTransform != standardTransform) {
				Transform morphEffect = transform.FindChild("ParticleSystems/" + currentTransform.morphEffectName);
				if (morphEffect != null) {
					morphEffect.GetComponent<ParticleSystem>().Play();
				}
				HUD.controller.SendMessage("MorphApplied", currentTransform);
			}
		}
	}

	public void RemoveCurrent()
	{
		if (currentTransform != standardTransform) {
			shatter.Play();
			ApplyTransform(standardTransform);
			HUD.controller.SendMessage("MorphRemoved");
		}
	}
}
