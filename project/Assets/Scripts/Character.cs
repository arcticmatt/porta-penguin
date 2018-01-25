using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSPoop;

public class Character : MonoBehaviour, IReset {

	private Animator anim;
	private bool isDead = false;
	private AudioSource audioSource;
	private float audioVolume = 0.75f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load("squish") as AudioClip;
		audioSource.volume = audioVolume;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Projectile") {
			audioSource.Play ();

			if (!isDead) {
				anim.SetTrigger ("Die");
				isDead = true;
			}
			GameControl.instance.PenguinScored ();
		}
	}

	public void Reset(float x, float y) {
		if (anim != null) {
			anim.SetTrigger ("Reset");
			transform.position = new Vector2 (x, y);
			isDead = false;
		}
	}
}
