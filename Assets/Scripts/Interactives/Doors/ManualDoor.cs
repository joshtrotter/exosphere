using UnityEngine;
using System.Collections;

public class ManualDoor : Door {
	
	//store the initial 'On' emission color
	public Color onColor = new Color(0xD3F,0x1CF,0xE2F);
	public Color offColor;

	//the speed at which the door opens and closes
	public float doorSpeed = 3f;

	private bool IsOpening = false;
	private bool IsClosing = false;

	private float openY;
	private float closedY;
	
	void Awake()
	{
		//set up base positions so door knows where to move between.
		closedY = transform.position.y;
		openY = closedY + 4.5f;
		offColor = onColor * Config.softDimIntensity;
		
		IsClosed = !IsClosed;
		SwapState ();

		//set color correctly,. If door is locked, then offColor will be set by LockController
		if (!IsLocked) SetEmissionColor (onColor);
	}
	
	public override void Close ()
	{
		IsOpening = false;
		IsClosing = true;
		StartCoroutine (MoveDown ());
		IsClosed = true;
	}
	
	public override void Open ()
	{
		IsOpening = true;
		IsClosing = false;
		StartCoroutine (MoveUp());
		IsClosed = false;
	}

	private IEnumerator MoveUp(){
		while (IsOpening) {
			yield return new WaitForEndOfFrame();
			Vector3 newPos = transform.position;
			newPos.y = Mathf.Lerp (transform.position.y, openY, doorSpeed * Time.deltaTime);
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
			newPos.y = Mathf.Lerp (transform.position.y, closedY, doorSpeed * Time.deltaTime);
			transform.position = newPos;
			if (transform.position.y <= closedY){
				IsClosing = false;
			}
		}
	}

	public override void Lock(){
		IsLocked = true;
		SetEmissionColor (offColor);
	}

	public override void Unlock(){
		IsLocked = false;
		SetEmissionColor (onColor);
	}

	//this function cycles through all pieces of the frame and changes their emission color
	public void SetEmissionColor(Color color){
		this.GetComponent<Renderer>().material.SetColor ("_EmissionColor", color);
	}
}

