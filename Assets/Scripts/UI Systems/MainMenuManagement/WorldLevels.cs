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
		levelSelectManager = GetComponentInParent<LevelSelectManager> ();
		levelIcons = GetComponentsInChildren<LevelIcon> ();
	}

	public void DisplayWorldLevels (WorldData world)
	{
		RenderSettings.skybox = world.skybox;
		title.text = world.worldName;
		for (int i = 0; i < levelIcons.Length; i++) {
			levelIcons [i].DisplayLevelInfo (world.GetXthChildData (i));
		}
	}

	public void LevelSelectedButton(int levelID){
		if (levelSelectManager.IsSafeToPress ()) {
			levelSelectManager.StartLevelInfoDisplay (levelID);
		}
	}
}
