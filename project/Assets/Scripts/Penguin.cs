using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour {

	public float upForce = 5f;

	private bool isDead = false;
	private Rigidbody2D rb2d;
	private Animator anim;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (isDead == false) {
			if (Input.GetKeyDown ("space")) {
				rb2d.velocity = Vector2.zero;
				rb2d.AddForce (new Vector2 (0, upForce));
				anim.SetTrigger ("Flap");
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D other) {
		switch (other.gameObject.tag) {
		case "PowerRapid":
			GameControl.instance.GainPower (GameControl.PowerType.RapidFire);
			return;
		case "PowerSlow":
			GameControl.instance.GainPower (GameControl.PowerType.StopTime);
			return;
		default:
			return;
		}
	}

	private void OnCollisionEnter2D () {
		rb2d.velocity = Vector2.zero;
		isDead = true;
		anim.SetTrigger ("Die");
		GameControl.instance.PenguinDied ();
	}
}
