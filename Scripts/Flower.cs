using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

    public int quality;  // pollen quality
    public int growth; // pollen created each unit of time
    public int quantity; // pollen quantity
    public int popularity; // How much bees like her
	// Use this for initialization
	void Start () {
        quality = Random.Range(0, 10);
        growth = Random.Range(0, 10);
        quantity = 0;
        popularity = 0;
	}
	
	// Update is called once per frame
	void Update () {
        quantity += growth;
	}
    public int getHarvested(int beeCapacity)
    {
        int pollen;
        if (quantity < beeCapacity)
            pollen = quantity;
        else
            pollen = beeCapacity;
        quantity -= pollen;
        return pollen;
    }
}
