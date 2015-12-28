using UnityEngine;
using System.Collections;

public class OcclusionDoor : Door {

	private OcclusionPortal portal;

	protected void Awake() 
	{
		portal = GetComponent<OcclusionPortal> ();

	}

	public override void Close ()
	{
		base.Close ();
		//TODO this is temporary until we have a proper door
		portal.gameObject.GetComponent<Renderer>().enabled = true;
		portal.open = false;
	}
	
	public override void Open ()
	{
		base.Open ();
		portal.open = true;
		//TODO this is temporary until we have a proper door
		portal.gameObject.GetComponent<Renderer>().enabled = false;
	}
}
