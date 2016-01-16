using UnityEngine;
using System.Collections;

public class TutorialDoorImageFrameSwitcher : MonoBehaviour {

	//store references to each piece of the side frame
	private Renderer[] side_pieces;

	private Color onColor;
	private Color offColor;

	void OnEnable(){
		side_pieces = GetComponentsInChildren<Renderer> ();
		onColor = side_pieces [0].material.GetColor ("_EmissionColor");
		offColor = onColor * Config.softDimIntensity;
		StartCoroutine (CycleColor ());
	}

	private IEnumerator CycleColor()
	{
		Color currentCol = offColor;
		while (true) {
			yield return new WaitForSeconds (2);
			SetEmissionColor (currentCol);
			if (currentCol == onColor) {
				currentCol = offColor;
			} else {
				currentCol = onColor;
			}
		}
	}


	//this function cycles through all pieces of the frame and changes their emission color
	public void SetEmissionColor(Color color){
		foreach (Renderer side in side_pieces) {
			side.material.SetColor ("_EmissionColor", color);
		}
	}
}
