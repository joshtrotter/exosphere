using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldLevels : MonoBehaviour {

	public Text title;
	private LevelSelectManager levelSelectManager;
	private WorldData currentWorld;
	private LevelIcon[] levelIcons;

	public Sprite collectedStar;
	public Sprite uncollectedStar;
	public Sprite lockedLevel;
	public Sprite uncompletedLevel;
	public Sprite completedLevel;

	void Start(){
		GetComponentInChildren<Canvas> ().renderMode = RenderMode.WorldSpace;
		currentWorld = LevelDataManager.manager.GetCurrentWorldData ();
		title.text = currentWorld.worldName;
		levelSelectManager = GetComponentInParent<LevelSelectManager> ();
		levelIcons = GetComponentsInChildren<LevelIcon> ();
		for (int i = 0; i < levelIcons.Length; i++) {
			levelIcons[i].DisplayLevelInfo(currentWorld.GetXthChildData (i));
		}
	}

	public void LevelSelectedButton(int levelID){
		levelSelectManager.StartLevelInfoDisplay (levelID);
	}
}
