using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NSPoop;

public class GameControl : MonoBehaviour
{

	public enum PowerType
	{
		RapidFire,
		StopTime}

	;

	public static GameControl instance;
	public GameObject gameOverText;
	public GameObject highscoreText;
	public Text scoreText;
	public bool gameOver = false;

	private int score = 0;
	private int oldHighscore;
	private float noOpVal = -1f;
	private float[] powerDurations;
	private float[] powerTimes;
	private GameLevel gameLevel;

	/*
	 * === API methods ===
	 */

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		oldHighscore = PlayerPrefs.GetInt ("highscore", -1);
		InitPowerDurations ();
		InitPowerTimes ();
		InitLevelStuff ();

		LosePower (PowerType.RapidFire);
		LosePower (PowerType.StopTime);
	}

	void Update ()
	{
		if (gameOver == true && Input.GetKeyDown ("space")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}

		UpdatePowers ();
		UpdateLevelCheck ();
	}

	/*
	 * === Init methods ===
	 */

	private void InitPowerTimes ()
	{
		powerTimes = new float[Enum.GetNames (typeof(PowerType)).Length];
		foreach (PowerType powerType in Enum.GetValues(typeof(PowerType))) {
			powerTimes [(int)powerType] = powerDurations [(int)powerType];
		}
	}

	private void InitPowerDurations ()
	{
		powerDurations = new float[Enum.GetNames (typeof(PowerType)).Length];
		powerDurations [(int)PowerType.RapidFire] = 10f;
		powerDurations [(int)PowerType.StopTime] = 5f;
	}

	private void InitLevelStuff ()
	{
		GameLevelMaker.InitLevels ();
		gameLevel = GameLevelMaker.GetLevel (score);

		UpdateLevelStuff(gameLevel);
	}

	/*
	 * === Update methods ===
	 */

	private void UpdatePowers ()
	{
		foreach (PowerType powerType in Enum.GetValues(typeof(PowerType))) {
			if (powerTimes [(int)powerType] == noOpVal) {
				continue;
			}

			powerTimes [(int)powerType] -= Time.deltaTime;

			if (powerTimes [(int)powerType] <= 0) {
				LosePower (powerType);
			}
		}
	}

	private void UpdateLevelCheck() {
		GameLevel newLevel = GameLevelMaker.GetLevel (score);
		if (!newLevel.Equals (gameLevel)) {
			UpdateLevelStuff (newLevel);
		}
	}

	private void UpdateLevelStuff (GameLevel newLevel)
	{
		gameLevel = newLevel;
		ScrollingObject.SetScrollSpeeds (newLevel);
		RandomObjectPool.SetSpawnTimes (newLevel);
	}

	/*
	 * === Misc crap ===
	 */

	public void PenguinDied ()
	{
		if (score > oldHighscore) {
			highscoreText.SetActive (true);
			PlayerPrefs.SetInt ("highscore", score);
		} else {
			gameOverText.SetActive (true);
		}
		gameOver = true;
	}

	public void PenguinScored ()
	{
		if (gameOver) {
			return;
		}
		score++;
		scoreText.text = "score: " + score.ToString ();
	}

	public void GainPower (PowerType powerType)
	{
		powerTimes [(int)powerType] = powerDurations [(int)powerType];

		switch (powerType) {
		case PowerType.RapidFire:
			FirePoop.GainPower ();
			break;
		case PowerType.StopTime:
			ScrollingObject.GainSlowTime ();
			break;
		default:
			break;
		}
	}

	private void LosePower (PowerType powerType)
	{
		powerTimes [(int)powerType] = noOpVal;

		switch (powerType) {
		case PowerType.RapidFire:
			FirePoop.LosePower ();
			break;
		case PowerType.StopTime:
			ScrollingObject.LoseSlowTime ();
			break;
		default:
			break;
		}
	}
}
