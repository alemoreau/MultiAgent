using UnityEngine;
using System.Collections;
using System;

public enum beeState
{
    AtHome,
    StayAtHome,
    Exploitation,
    Exploration,
    HeadHome,
    CarryingFood
};
public enum beePreference
{
    StayAtHome,
    Exploration,
};


public class BeeState : MonoBehaviour
{

    public beeState state;
    public int age;
    public bool hasFood;
    public int pollenCapacity;
    public int pollenCarried;
    public bool startExploring;
    public bool startExploiting;
    public beePreference preference;
    public GameObject preferedFlower;
    public GameObject hive;

    // Use this for initialization
    void Start()
    {
        age = 0;
        pollenCapacity = 100;
        pollenCarried = 0;
        preferedFlower = null;
        hasFood = false;
        state = beeState.AtHome;
        startExploring = true;
        startExploiting = true;
        int r = UnityEngine.Random.Range(0, 2);
        if (r == 0)
            preference = beePreference.StayAtHome;
        else
            preference = beePreference.Exploration;
        hive = GameObject.FindGameObjectWithTag("Hive");
    }

    // Update is called once per frame
    void Update()
    {
        age++;
        liveOrDie();
        lookAtFlowersNearby();

        // If the bee is staying at home, no state change possible
        if (state == beeState.StayAtHome)
            return;
        // If the bee is at home, it is waiting for being recruted
        if (state == beeState.AtHome)
        {
            // If it prefers to stay at home, it waits for pollen sources to be found by others
            if (preference == beePreference.StayAtHome)
            {
                if (UnityEngine.Random.Range(0, 100) < 10) // 10% probability to go exploiting
                {
                    // decide which flower to exploit :
                    // each flower has a score, the probability of a flower to be chosen is "score(f) / sum_flower(score(flower))
                    int totalScore = 0;
                    int currentScore = 0;
                    // Compute total score
                    foreach (GameObject flower in hive.GetComponent<Hive>().flowersFound)
                    {
                        totalScore += flower.GetComponent<Flower>().popularity;
                    }
                    int r = UnityEngine.Random.Range(0, totalScore);
                    foreach (GameObject flower in hive.GetComponent<Hive>().flowersFound)
                    {
                        currentScore += flower.GetComponent<Flower>().popularity;
                        if (r < currentScore)
                        {
                            preferedFlower = flower;
                            state = beeState.Exploitation;
                            break;
                        }
                    }
                }
            }
            // If it prefers to explore, it can spontaneously decide to go exploring
            if (preference == beePreference.Exploration)
            {
                if (UnityEngine.Random.Range(0, 100) < 10) // 10% probability to go exploring
                    state = beeState.Exploration;
            }
            return;
        }
        if (state == beeState.Exploration)
        {
            if (reachedDestination()) // If reached the destination
            {
                if (UnityEngine.Random.Range(0, 4) == 0) // Either go back to the hive, or explore another area
                    state = beeState.HeadHome;
                else
                    this.GetComponent<BeeAction>().startExploring = true;
            }
            else
            {
                 if (UnityEngine.Random.Range(0, 2000) == 0) // To prevent the bee from behing locked somewhere for too long
                    state = beeState.HeadHome;
            }

            return;
        }
        if (state == beeState.CarryingFood)
        {
            if (reachedDestination()) // If reached the hive
            {
                // Leave food in the hive
                hive.GetComponent<Hive>().food += pollenCarried;
                // Update information on the flower
                preferedFlower.GetComponent<Flower>().popularity = (1000 * pollenCarried) / (int)(Vector3.Distance(hive.GetComponent<Transform>().position, preferedFlower.GetComponent<Transform>().position));
                bool hasAlreadyBeenFound = false;
                foreach (GameObject flower in hive.GetComponent<Hive>().flowersFound)
                {
                    if (flower.GetComponent<Transform>().position.Equals(preferedFlower.GetComponent<Transform>().position))
                        hasAlreadyBeenFound = true;
                }
                if (!hasAlreadyBeenFound)
                    hive.GetComponent<Hive>().flowersFound.Add(preferedFlower);

                if (UnityEngine.Random.Range(0, pollenCarried) == 0) // Add a small chance to wait in the hive and to change prefered flower
                {
                    state = beeState.AtHome;
                    preferedFlower = null;
                }
                pollenCarried = 0;
            }
            else
                return;
        }

        if (state == beeState.Exploitation)
        {
            if (reachedDestination()) // If reached the flower
            {
                if (preferedFlower != null)
                {
                    int pollen = preferedFlower.GetComponent<Flower>().getHarvested(pollenCapacity);
                    pollenCarried = pollen;
                    state = beeState.CarryingFood;
                }
                else
                    state = beeState.HeadHome;
            }
               
            return;
        }

        if (state == beeState.HeadHome)
            if (reachedDestination()) // If reached the hive
                state = beeState.AtHome;
                return;

    }

    private void lookAtFlowersNearby()
    { //Only look at x and z
        foreach (GameObject flower in GameObject.FindGameObjectsWithTag("Flower"))
        {
            Vector3 beePosition = this.gameObject.GetComponent<Transform>().position;
            Vector3 flowerPosition = flower.GetComponent<Transform>().position;
            beePosition[1] = 0;
            flowerPosition[1] = 0;
            if (Vector3.Distance(beePosition, flowerPosition ) < 500)
            {
                state = beeState.Exploitation;
                preferedFlower = flower;
                break;
            }
        }
    }

    private bool reachedDestination()
    {
        Vector3 beePosition = this.gameObject.GetComponent<Transform>().position;
        Vector3 destinationPosition = this.GetComponent<BeeAction>().destination;
        beePosition[1] = 0;
        destinationPosition[1] = 0;
        return Vector3.Distance(beePosition, destinationPosition) < 20;
    }

    private void liveOrDie()
    {
        //int r = Random.Range(0, 8000 - age);
        int r = 1;
        if (r == 0)
        {
            GameObject.FindGameObjectWithTag("Hive").GetComponent<Hive>().swarm.Remove(this.gameObject);
            Destroy(this.gameObject);
            return;
        }
    }
}