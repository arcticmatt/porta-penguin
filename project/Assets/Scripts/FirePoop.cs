using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoop : MonoBehaviour {

	public PoopPool poopPool;
	public GameObject poopPoint;
	// Shots per second
	public LayerMask notToHit;
	public float poopSpeed = -5f;
	public AudioSource audioSource;

	private Rigidbody2D rb2d;
	private float timeToFire = 0f;
	private Vector2 defaultPoopSpeedVector;

	private static float rapidFireRate = 8f;  // TODO: adjust
	private static float baseFireRate = 1f;
	private static float fireRate = baseFireRate;

	// Use this for initialization
	void Awake () {
		defaultPoopSpeedVector = new Vector2 (0, poopSpeed);
		rb2d = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.DownArrow) && Time.time > timeToFire) {
			timeToFire = Time.time + 1f / fireRate;
			Shoot ();
		}
	}

	void Shoot () {
		audioSource.Play ();

		GameObject poop = poopPool.getPoop ();
		poop.SetActive (true);
		poop.transform.position = poopPoint.transform.position;

		Vector2 poopVelocity;
		// If penguin is falling faster than the default poop velocity, give the
		// poop the penguin's velocity instead
		if (rb2d.velocity.y < poopSpeed) {
			Debug.Assert (poopSpeed < 0);
			poopVelocity = rb2d.velocity;
		} else {
			poopVelocity = defaultPoopSpeedVector;
		}

		poop.GetComponent<Rigidbody2D> ().velocity = poopVelocity;
	}

	public static void GainPower() {
		fireRate = rapidFireRate;
	}

	public static void LosePower() {
		fireRate = baseFireRate;
	}
}
