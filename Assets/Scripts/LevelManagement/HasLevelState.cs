using UnityEngine;
using System.Collections;
 
public class UniqueIdentifierAttribute : PropertyAttribute {}

/**
 * Objects that have level state will be registered with the LevelManager when the scene is first loaded. If the scene is reloaded (after player death)
 * then these objects will be informed of their current level state by the LevelManager so that they can reload in an appropriate way.
 */
public abstract class HasLevelState : MonoBehaviour {
	[UniqueIdentifier]
	public string uniqueId;

	public int currentState = 0;
	private static LevelManager levelManager;
	
	public abstract void ReloadState(int state);

	protected void RegisterStateChange(int state) {
		currentState = state;
		LevelManager.manager.RegisterObjectState (uniqueId, currentState);
	}
}
