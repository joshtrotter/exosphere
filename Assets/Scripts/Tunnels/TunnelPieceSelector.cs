﻿using UnityEngine;
using System.Collections;

public class TunnelPieceSelector : MonoBehaviour {

	public TunnelPiece spawnChildTunnelPiece(TunnelPiece parent, TunnelSelectionPreferences prefs) {
		return TunnelPiecePool.INSTANCE.spawnNewInstance(selectFromPool(parent, prefs));
	}

	protected virtual TunnelPiece selectFromPool(TunnelPiece parent, TunnelSelectionPreferences prefs) {
		bool validPieceFound = false;
		TunnelPiece candidate = null;
		while (!validPieceFound) {
			candidate = TunnelPiecePool.INSTANCE.tunnelPieces [Random.Range (0, (TunnelPiecePool.INSTANCE.tunnelPieces.Length))];
			validPieceFound = validatePiece(candidate, prefs);
		}

		return candidate;
	}

	protected virtual bool validatePiece(TunnelPiece candidate, TunnelSelectionPreferences prefs) {
		bool isValid = candidate.bucketLevel <= prefs.maxBucketLevel;
		isValid = isValid && candidate.difficultyLevel <= prefs.maxDifficulty;
		isValid = isValid && candidate.rarity <= prefs.maxRarity;
		isValid = isValid && validateClearRuns (candidate, prefs);
		return isValid;
	}

	private bool validateClearRuns(TunnelPiece candidate, TunnelSelectionPreferences prefs) {
		bool isValid = true;

		if (prefs.requireCleanRun) {
			isValid = candidate.clearRun;
		}
		
		if (isValid && candidate.minClearSequenceBefore > 0f) {
			isValid = TunnelSpawnController.INSTANCE.getCurrentClearRun() >= candidate.minClearSequenceBefore;
		}
		
		if (isValid && candidate.maxClearSequenceBefore > 0f) {
			isValid = TunnelSpawnController.INSTANCE.getCurrentClearRun() <= candidate.maxClearSequenceBefore;
		}

		return isValid;
	}
}