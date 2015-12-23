using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SpinAtTarget : MoveBetweenTargets
{
	public float maxRotationSpeed = 1.5f;
	public float spinUpTime = 2.5f;
	private AxisRotator rotator;

	protected void Awake ()
	{
		base.Awake ();
		rotator = GetComponent<AxisRotator> ();
	}

	protected override Tween DoTargetAction ()
	{
		return DOTween.Sequence ()
			.Append (DOTween.To (() => rotator.rotationsPerSecond, x => rotator.rotationsPerSecond = x, maxRotationSpeed, spinUpTime))
			.AppendInterval (interval)
			.Append (DOTween.To (() => rotator.rotationsPerSecond, x => rotator.rotationsPerSecond = x, 0f, spinUpTime));
	}
}
