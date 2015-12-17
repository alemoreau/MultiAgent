﻿using UnityEngine;
using System.Collections;

public class FlowerManager : MonoBehaviour {

    public GameObject[] flowers;                // The flower prefab to be spawned.
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this flower can spawn from.

	// Use this for initialization
	void Start () {
	    // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Spawn () {

        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        Vector3 randomPosition = spawnPoints[spawnPointIndex].position;
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation
        randomPosition[0] += ((float) Random.Range(-10, 10)) / 10;
        randomPosition[2] += ((float)Random.Range(-10, 10)) / 10;
        Instantiate(flowers[spawnPointIndex], randomPosition, spawnPoints[spawnPointIndex].rotation);
    }
}
