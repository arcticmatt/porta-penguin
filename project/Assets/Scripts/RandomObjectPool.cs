using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSPoop;

/**
 * Use this when you have a set of objects you want to be spawned
 * in random order.
 */
public class RandomObjectPool : MonoBehaviour {

	public int howManyOfEach = 2;
	public float yMin = -3f;
	public float yMax = 3f;
	public float spawnXPosition = 10;
	public string filePath; //
	public ObjectType objectType;

	private GameObject[] objects;
	private float timeSinceLastSpawned;
	private float spawnRate;
	private int howManyObjects;
	private int objectPoolSize;
	private Vector2 initialPosition;
	private SpawnTime spawnTime;

	private static SpawnTime[] spawnTimes;

	/*
	 * === API methods ===
	 */

	void Awake () {
		initialPosition = new Vector2 (-20, 0);
		InitObjects ();
		if (objectType == ObjectType.Character) {
			SpawnObject();
		}
	}

	void Start() {
		spawnTime = spawnTimes [(int) objectType];
		UpdateSpawnRate ();
	}

	void Update () {
		timeSinceLastSpawned += Time.deltaTime;

		UpdateSpawnTime ();

		// GameControl.instance will be null for the starting screen
		if ((GameControl.instance == null || GameControl.instance.gameOver == false)
		  && timeSinceLastSpawned >= spawnRate) {
			timeSinceLastSpawned = 0;
			SpawnObject ();
			UpdateSpawnRate ();
		}
	}

	/*
	 * === Init methods ===
	 */

	// private static void InitSpawnTimes() {
	// 	int length = System.Enum.GetNames (typeof(ObjectType)).Length;
	// 	spawnTimes = new SpawnTime[length];
  //
	// 	spawnTimes [(int) ObjectType.Character] = new SpawnTime(2f, 3f);
	// 	spawnTimes [(int) ObjectType.Obstacle] = new SpawnTime(2f, 4.5f);
	// 	spawnTimes [(int) ObjectType.Power] = new SpawnTime(5f, 15f);
	// }

	private void InitObjects() {
		int index = 0;
		GameObject[] originalObjects = Resources.LoadAll<GameObject>(filePath);

		howManyObjects = originalObjects.Length;
		objectPoolSize = howManyOfEach * howManyObjects;
		objects = new GameObject[objectPoolSize];

		for (int i = 0; i < originalObjects.Length; i++) {
			index = AddObject (originalObjects [i], index);
		}
	}

	/*
	 * === Update methods ===
	 */

	private void UpdateSpawnRate() {
		spawnRate = Random.Range (spawnTime.min, spawnTime.max);
	}

	private void UpdateSpawnTime() {
		if (spawnTime != spawnTimes [(int) objectType]) {
			spawnTime = spawnTimes [(int) objectType];
			UpdateSpawnRate();
			// TODO: only do this when going from level 0 -> level 1
			if (timeSinceLastSpawned > 7) {
				timeSinceLastSpawned = 0;
			}
		}
	}

	/*
	 * === Spawn methods ===
	 */

	/*
	 * Spawns random object I.E. moves object to spawn position. Only spawns
	 * object if it is off screen.
	 *
	 * Does this by picking a random index, and then iterating from there until it finds
	 * an offscreen object
	 */
	private void SpawnObject() {
		int incDec = 1;
		if (Random.Range(0, 1) == 0) {
			incDec = -1;
		}
		int randomIndex = Random.Range (0, objectPoolSize - 1);
		int startIndex = randomIndex;  // use this to avoid infinite loops
		while (true) {
			if (objects [randomIndex].transform.position.x < ScrollingObject.offScreen) {
				ConfigureSpawnedObject (objects [randomIndex]);
				return;
			}

			randomIndex += incDec;
			// B/c mod is remainder in c# (and probably slower?)
			if (randomIndex < 0) {
				randomIndex = objectPoolSize - 1;
			}
			if (randomIndex > objectPoolSize) {
				randomIndex = 0;
			}
			if (randomIndex == startIndex) {
				return;  // avoid infinite loops
			}
			Debug.Assert (randomIndex >= 0);
			Debug.Assert (randomIndex < objectPoolSize);
		}
	}

	private void ConfigureSpawnedObject(GameObject obj) {
		foreach (var component in obj.GetComponents<MonoBehaviour>()) {
			if (component is IReset) {
				float yPos = Random.Range (yMin, yMax);
				((IReset) component).Reset (spawnXPosition, yPos);
			}
		}
	}

	/*
	 * Adds objects in an interlaced manner.
	 * E.g. 3 objects, with 2 of each, will look like:
	 * o1 o2 o3 o1 o2 o3
	 */
	private int AddObject(GameObject prefab, int startIndex) {
		int nextIndex = startIndex + 1;
		while (startIndex < objectPoolSize) {
			if (startIndex >= objectPoolSize) {
				throw new System.IndexOutOfRangeException ("Index " + startIndex.ToString () + " is out of range");
			}
			objects [startIndex] = (GameObject)Instantiate (prefab, initialPosition, Quaternion.identity);
			// Nice for debugging
			objects [startIndex].SetActive (true);

			startIndex += howManyObjects;
		}
		return nextIndex;
	}

	/*
	 * Level methods
	 */

	public static void SetSpawnTimes (GameLevel gameLevel)
	{
		spawnTimes = gameLevel.spawnTimes;
	}
}
