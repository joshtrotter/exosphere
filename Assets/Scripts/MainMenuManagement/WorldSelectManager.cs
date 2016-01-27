using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class WorldSelectManager : MonoBehaviour {

	public CanvasRenderer background;
	private CanvasGroup canvas;
	private LevelSelectManager levelSelectManager;
	private WorldInfo worldInfo;
	private RectTransform rectTransform;
	private int worldNumber = 0;

	//the distance between the different layers
	public float worldGap = 716;

	void Awake(){
		canvas = GetComponentInChildren<CanvasGroup> ();
		levelSelectManager = GetComponent<LevelSelectManager> ();
		worldInfo = GetComponentInChildren<WorldInfo> ();
		rectTransform = worldInfo.GetComponent<RectTransform> ();
	}

	void Update(){
		int newWorldNumber = (int)(-0.5 * Mathf.Floor (background.transform.localPosition.y / (worldGap / 2)));
		if (worldNumber != newWorldNumber) {
			//Debug.Log (newWorldNumber + ", " + worldNumber);
			worldNumber = newWorldNumber;
			UpdateWorldInfo ();
		}
	}

	//changes position and display of the world info screen to display correct data
	private void UpdateWorldInfo ()
	{
		if (worldNumber > 0) {
			rectTransform.offsetMax = new Vector2 (rectTransform.offsetMax.x, (worldNumber * worldGap));
			rectTransform.offsetMin = new Vector2 (rectTransform.offsetMin.x, (worldNumber * worldGap));
			worldInfo.DisplayWorldInfo (LevelDataManager.manager.GetWorldData (worldNumber));
		}
	}

	public void MoveScreenToWorld (int worldChange)
	{
		background.transform.DOLocalMove (new Vector3 (0, -1 * ((worldNumber + worldChange) * worldGap), 0), 1).Play ();
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
