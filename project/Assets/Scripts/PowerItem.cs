using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSPoop;

public class PowerItem : MonoBehaviour, IReset {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void Reset(float x, float y) {
		gameObject.GetComponent<Renderer>().enabled = true;
	}

	private void OnTriggerEnter2D (Collider2D other) {
		gameObject.GetComponent<Renderer>().enabled = false;
	}
}
