using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPlaying : MonoBehaviour {
	
	public static KeepPlaying instance;
	public AudioSource audioSource;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}
	}
}
