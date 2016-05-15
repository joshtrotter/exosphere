using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LevelLaunchSequence : MonoBehaviour {

	public Rigidbody ball;
	public Camera cam;
	
	public void PlayLevel(){
		StartCoroutine (Launch());
	}

	private IEnumerator Launch(){
		ball.isKinematic = false;
		cam.enabled = true;
		cam.transform.DOLocalRotate (new Vector3 (10f, 0f, 0f), 1f).Play ();
		yield return new WaitForSeconds (3.5f);
		LevelManager.manager.FirstLoadLevel ();
	}
	
}
