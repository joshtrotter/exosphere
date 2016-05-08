using UnityEngine;
using System.Collections;

public class ZoomPadTunnelPiece : ObstacleFilledTunnelPiece {

	protected override void applyConfig(ObstacleConfig config) {
		Debug.Log ("Setting up " + config.name);
		foreach (LayerOfObstacles layer in config.configLayers) {
			layer.chosenLevel = layer.allowableLevels[Random.Range(0, layer.allowableLevels.Length)];
			foreach (GameObject obstacle in layer.obstacles){
				obstacle.gameObject.SetActive(true);
				obstacle.transform.Translate(0,0,layer.chosenLevel, Space.World);
			}
		}
		rotationInUse = allowableRotations [Random.Range (0, allowableRotations.Length)];
		transform.GetChild(0).transform.Rotate (Vector3.up * rotationInUse);
	}
	
	protected override void removeConfig(ObstacleConfig config) {
		foreach (LayerOfObstacles layer in config.configLayers) {
			foreach (GameObject obstacle in layer.obstacles) {
				obstacle.gameObject.SetActive (false);
				obstacle.transform.Translate (0, 0, -layer.chosenLevel, Space.World);
			}
		}
	}


}
