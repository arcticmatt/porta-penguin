using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using NSPoop;

public class StartListener : MonoBehaviour {

	void Awake () {
		GameLevelMaker.InitLevels ();
		GameLevel gameLevel = GameLevelMaker.GetLevel (0);
		ScrollingObject.SetScrollSpeeds (gameLevel);
		RandomObjectPool.SetSpawnTimes (gameLevel);
	}

	// Update is called once per frame
	void Update () {
			if (Input.GetKeyDown ("space")) {
				SceneManager.LoadScene ("scene");
			}
	}
}
