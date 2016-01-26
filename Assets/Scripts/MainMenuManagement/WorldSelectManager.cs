using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class WorldSelectManager : MonoBehaviour {

	public CanvasRenderer background;
	private CanvasGroup canvas;
	private LevelSelectManager levelSelectManager;
	private WorldInfo worldInfo;

	//the distance between the different layers
	public float worldGap = 716;

	void Awake(){
		canvas = GetComponentInChildren<CanvasGroup> ();
		levelSelectManager = GetComponent<LevelSelectManager> ();
		worldInfo = GetComponentInChildren<WorldInfo> ();
	}

	public void Launch(){
		background.transform.DOLocalMove (new Vector3(0,-worldGap,0), 1).Play ();
		RectTransform rectTransform = worldInfo.GetComponent<RectTransform> ();
		rectTransform.offsetMax = new Vector2 (rectTransform.offsetMax.x, worldGap);
		rectTransform.offsetMin = new Vector2 (rectTransform.offsetMin.x, worldGap);
		worldInfo.DisplayWorldInfo (worldInfo.currentWorld);
	}

	public void BackToOpeningScreen(){
		background.transform.DOLocalMove (new Vector3(0,0,0),1).Play(); 
	}

	public void EnterWorld(WorldData world){
		levelSelectManager.StartWorldLevelsDisplay (world);
		canvas.DOFade (0, 1).Play ().OnComplete(Disable);
	}

	public void ExitWorld(){
		canvas.gameObject.SetActive (true);
		canvas.DOFade (1, 1).Play ();
	}

	private void Disable(){
		canvas.gameObject.SetActive (false);
	}
}
