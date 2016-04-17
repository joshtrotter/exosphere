using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SpinningLaserAtTarget : MoveBetweenTargets
{
	public float maxRotationSpeed = 1.5f;
	public float spinUpTime = 2.5f;
	private AxisRotator rotator;
	public LaserAutoGun[] lasers;

	protected override void Awake ()
	{
		base.Awake ();
		DisableLasers ();
		rotator = GetComponent<AxisRotator> ();
	}

	protected override Tween DoTargetAction ()
	{
		return DOTween.Sequence ().OnComplete(DisableLasers).OnStart (EnableLasers)
			.Append (DOTween.To (() => rotator.rotationsPerSecond, x => rotator.rotationsPerSecond = x, maxRotationSpeed, spinUpTime))
			.AppendInterval (interval)
			.Append (DOTween.To (() => rotator.rotationsPerSecond, x => rotator.rotationsPerSecond = x, 0f, spinUpTime));
	}

	private void DisableLasers(){
		foreach (LaserAutoGun laser in lasers) {
			laser.gameObject.SetActive(false);
		}
	}

	private void EnableLasers(){
		foreach (LaserAutoGun laser in lasers) {
			laser.gameObject.SetActive(true);
		}
	}
}
