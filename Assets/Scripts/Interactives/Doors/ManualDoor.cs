using UnityEngine;
using System.Collections;

public class ManualDoor : Door {
	
	//store the initial 'On' emission color
	public Color onColor = new Color(0xD3F,0x1CF,0xE2F);
	private Color offColor;

	private bool IsOpening;
	private bool IsClosing;

	private float openY;
	private float closedY;
	
	void Awake(){
		//set up base positions so door knows where to move between.
		closedY = transform.position.y;
		openY = closedY + 4.5f;

		offColor = onColor * Config.dimIntensity;
		IsClosed = !IsClosed;
		SwapState ();
	}

	
	public override void Close ()
	{
		IsOpening = false;
		IsClosing = true;
		SetEmissionColor (onColor);
		IsClosed = true;
	}
	
	public override void Open ()
	{
		IsOpening = true;
		IsClosing = false;
		SetEmissionColor (offColor);
		IsClosed = false;
	}

	private IEnumerator MoveUp(){
		while (IsOpening) {
			yield return new WaitForEndOfFrame();
			Vector3 newPos = transform.position;
			newPos.y = Mathf.Lerp (transform.position.y, openY, Time.deltaTime);
			transform.position = newPos;
			if (transform.position.y >= openY){
				IsOpening = false;
			}
		}
	}

	private IEnumerator MoveDown(){
		while (IsClosing) {
			yield return new WaitForEndOfFrame();
			Vector3 newPos = transform.position;
			newPos.y = Mathf.Lerp (transform.position.y, closedY, Time.deltaTime);
			transform.position = newPos;
			if (transform.position.y <= closedY){
				IsClosing = false;
			}
		}
	}

	//this function cycles through all pieces of the frame and changes their emission color
	public void SetEmissionColor(Color color){
		this.GetComponent<Renderer>().material.SetColor ("_EmissionColor", color);
	}
}

