using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSPoop;

public class ScrollingObject : MonoBehaviour, IReset {

	public ObjectType objectType;
	public static int offScreen = -12;

	private float scrollSpeed;

	private static float[] baseScrollSpeeds;
	private static float[] scrollSpeeds;
	private static float slowRate = 1.5f;
	private static bool isSlow = false;

	private Rigidbody2D rb2d;

	/*
	 * === API methods ===
	 */

	void Awake () {
		if (scrollSpeeds == null) {
			InitScrollSpeeds ();
		}
	}

	void Start () {
		scrollSpeed = scrollSpeeds [(int) objectType];

		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = GetVelocity ();

		RandomRotate ();
	}

	void Update () {
		UpdateScrollSpeed ();

		// GameControl.instance will be null for the starting screen
		if ((GameControl.instance != null && GameControl.instance.gameOver == true)
		  || (objectType != ObjectType.Background && transform.position.x < offScreen)) {
			rb2d.velocity = Vector2.zero;
		}

		if (ShouldRotate()) {
			gameObject.transform.Rotate (0, 0, Time.deltaTime * 20);
		}
	}

	/*
	 * === Init methods ===
	 */

	private static void InitScrollSpeeds() {
		// Unlike baseScrollSpeeds, we need to initialize it because we're not
		// just using the array declared in a GameLevel. This is b/c we need
		// to account for powerups.
		int length = System.Enum.GetNames (typeof(ObjectType)).Length;
		scrollSpeeds = new float[length];
	}

	/*
	 * === Update methods ===
	 */

	private void UpdateScrollSpeed() {
		if (scrollSpeed != scrollSpeeds [(int) objectType]) {
			rb2d.velocity = GetVelocity ();
			scrollSpeed = scrollSpeeds [(int) objectType];
		}
	}

	/*
	 * === Power methods ===
	 */

	public static void GainSlowTime() {
		if (scrollSpeeds != null && !isSlow) {
			isSlow = true;
			scrollSpeeds [(int)ObjectType.Character] /= slowRate;
			scrollSpeeds [(int)ObjectType.Obstacle] /= slowRate;
			scrollSpeeds [(int)ObjectType.Power] /= slowRate;
		}
	}

	public static void LoseSlowTime() {
		if (scrollSpeeds != null) {
			isSlow = false;
			System.Array.Copy (baseScrollSpeeds, 0, scrollSpeeds, 0, scrollSpeeds.Length);
		}
	}

	/*
	 * === Rotation methods ===
	 */

	private void RandomRotate() {
		if (ShouldRotate()) {
			int rr = Random.Range (0, 359);
			gameObject.transform.Rotate (0, 0, rr);
		}
	}

	private bool ShouldRotate() {
		return objectType == ObjectType.Obstacle || objectType == ObjectType.Power;
	}

	/*
	 * === Level methods ===
	 */

	public static void SetScrollSpeeds(GameLevel gameLevel) {
		baseScrollSpeeds = gameLevel.scrollSpeeds;
		if (isSlow == false) {
			System.Array.Copy (baseScrollSpeeds, 0, scrollSpeeds, 0, scrollSpeeds.Length);
		}
	}

	/*
	 * === Misc methods ===
	 */

	public void Reset (float x, float y) {
		if (rb2d != null) {
			rb2d.velocity = GetVelocity();
			RandomRotate ();
		}
		transform.position = new Vector2 (x, y);
	}

	private Vector2 GetVelocity() {
		return new Vector2 (scrollSpeeds [(int) objectType], 0);
	}

}
