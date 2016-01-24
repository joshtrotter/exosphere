using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldLevels : MonoBehaviour {

	public Text title;
	private LevelSelectManager levelSelectManager;
	private LevelIcon[] levelIcons;

	public Sprite collectedStar;
	public Sprite uncollectedStar;
	public Sprite lockedLevel;
	public Sprite uncompletedLevel;
	public Sprite completedLevel;

	void Start(){
		GetComponentInChildren<Canvas> ().renderMode = RenderMode.WorldSpace;
	}

	public void DisplayWorldLevels (WorldData world)
	{
		RenderSettings.skybox = world.skybox;
		title.text = world.worldName;
		levelSelectManager = GetComponentInParent<LevelSelectManager> ();
		levelIcons = GetComponentsInChildren<LevelIcon> ();
		for (int i = 0; i < levelIcons.Length; i++) {
			levelIcons [i].DisplayLevelInfo (world.GetXthChildData (i));
		}
	}

	public void LevelSelectedButton(int levelID){
		levelSelectManager.StartLevelInfoDisplay (levelID);
	}
}
