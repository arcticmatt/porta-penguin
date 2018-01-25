using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopPool : MonoBehaviour {

	public int poopPoolSize = 5;
	public GameObject poopPrefab;
	public int poopSpeed;

	private GameObject[] poops;
	private int currentPoop = 0;

	// Use this for initialization
	void Start () {
		poops = new GameObject[poopPoolSize];
		for (int i = 0; i < poopPoolSize; i++) {
			poops [i] = (GameObject)Instantiate (poopPrefab, transform.position, Quaternion.identity);
			poops [i].SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject getPoop() {
		GameObject poopToReturn = poops [currentPoop];

		currentPoop++;
		if (currentPoop >= poopPoolSize) {
			currentPoop = 0;
		}
			
		return poopToReturn;
	}
}
