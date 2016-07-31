using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Tips : ScriptableObject {

	[TextArea(1,10)]
	public string[] tips;

}
