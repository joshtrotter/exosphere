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
		portal.open = false;
	}
	
	public override void Open ()
	{
		base.Open ();
		portal.open = true;
	}
}
