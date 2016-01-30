using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MorphPickup : MonoBehaviour {

	public float maxFloatHeight = 6f;

	void OnEnable () {
		transform.position = transform.position + Vector3.up * maxFloatHeight;
		transform.DOMoveY (transform.position.y - maxFloatHeight, 2f).Play();
	}
}
