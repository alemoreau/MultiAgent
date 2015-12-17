using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hive : MonoBehaviour
{

    public Transform spawnLocation;
    public GameObject hiveObject;
    public GameObject queenBee;
    public GameObject[] bees;
    public int food;
    public int beeCount;
    public List<GameObject> swarm;
    public List<GameObject> beeOutside;

    public List<GameObject> flowersFound;
    public List<int> flowersQuality;

    // Use this for initialization
    void Start()
    {
        food = 1000;
        spawnLocation = GetComponent<Transform>();
        hiveObject = GameObject.FindGameObjectWithTag("Hive");
        queenBee = GameObject.FindGameObjectWithTag("Bee");
        spawnLocation = hiveObject.transform;
        swarm = new List<GameObject>();
        beeOutside = new List<GameObject>(); // Not implemented yet
        beeCount = 100;
        //InvokeRepeating("Spawn", 1, 1);
        for (int i = 0; i < beeCount; i++)
            swarm.Add(Instantiate(queenBee, spawnLocation.position, spawnLocation.rotation) as GameObject);

    }

    // Update is called once per frame
    void Update()
    {
        food--;
        while (queenBee == null)
        {
            queenBee =  swarm[swarm.Count - 1];
        }
        //Update the number of bees in the swarm
        this.beeCount = this.swarm.Count;
    }

    void Spawn()
    {
    
        GameObject bee = Instantiate(queenBee, spawnLocation.position, spawnLocation.rotation) as GameObject;
        bee.transform.parent = GameObject.Find("Swarm").transform;
        swarm.Add(bee);
    }

}