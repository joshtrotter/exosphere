using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CameraModifier : MonoBehaviour {

	public float zoomOutDistance = -10f;
	public float zoomTime = 2f;


	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			Camera.main.transform.DOBlendableLocalMoveBy(new Vector3(0f,0f,zoomOutDistance), zoomTime).Play ();
		}
	}
}
