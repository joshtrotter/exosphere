using UnityEditor;
using UnityEngine;
using System;

// Place this file inside Assets/Editor
[CustomPropertyDrawer (typeof(UniqueIdentifierAttribute))]
public class UniqueIdDrawer : PropertyDrawer {
	private bool generated = false;

	public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) {
		// Generate a unique ID, defaults to an empty string if nothing has been serialized yet
		if (!generated) {
			Guid guid = Guid.NewGuid ();
			prop.stringValue = guid.ToString ();
			generated = true;
		}
		
		// Place a label so it can't be edited by accident
		Rect textFieldPosition = position;
		textFieldPosition.height = 16;
		DrawLabelField (textFieldPosition, prop, label);
	}
	
	void DrawLabelField (Rect position, SerializedProperty prop, GUIContent label) {
		EditorGUI.LabelField(position, label, new GUIContent (prop.stringValue));
	} 
}