using UnityEngine;
using System.Collections;

public class BeeAction : MonoBehaviour
{
    Transform hive;                 // Reference to the hive's position.
    public Vector3 destination;      
    beeState state;
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    public bool startExploring;
    public bool startExploiting;
    private Animation anim;

    void Start()
    {
        // Set up the references.
        hive = GameObject.FindGameObjectWithTag("Hive").transform;
        state = this.GetComponent<BeeState>().state;
        nav = this.GetComponent<NavMeshAgent>();
        startExploring = true;
        startExploiting = true;
        this.anim = this.GetComponent<Animation>();
    }


    void Update()
    {
        this.anim.Play("Fly");

        state = this.GetComponent<BeeState>().state;
        // Decide what to do depending on the bee state
        switch (state)
        {
            case beeState.Exploration:
                Exploration();
                break;
            case beeState.Exploitation:
                Exploitation();
                break;
            case beeState.CarryingFood:
                CarryingFood();
                break;
            case beeState.HeadHome:
                HeadHome();
                break;
            case beeState.StayAtHome:
                StayAtHome();
                break;
            case beeState.AtHome:
                AtHome();
                break;
            default:
                break;

        }
    }


    void Exploration()
    {
        if (startExploring)
        {
            int maxDistance = 10000;
            int not_blocked = 10;
            do
            {
                Vector3 randomDirection = Random.insideUnitCircle * maxDistance;
                randomDirection.z = randomDirection.y;
                randomDirection.y = 0;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 50, 1);
                destination = hit.position;
                maxDistance -= 500;
                not_blocked--;
            }
            while (!nav.SetDestination(destination) && not_blocked > 0);
            if (not_blocked == 0)
            {
                this.GetComponent<BeeState>().state = beeState.HeadHome;
                destination = hive.GetComponent<Transform>().position;
            }
            startExploring = false;
        }
        nav.SetDestination(destination);
    }

    void Exploitation()
    {
        if (startExploiting)
        {
            if (this.GetComponent<BeeState>().preferedFlower != null) // Pas tres naturel
                destination = this.GetComponent<BeeState>().preferedFlower.transform.position;
            startExploiting = false;
        }
        nav.SetDestination(destination);
    }

    void CarryingFood()
    {
        destination = hive.position;
        nav.SetDestination(destination);
    }

    void HeadHome()
    {
        destination = hive.position;
        nav.SetDestination(destination);
    }

    void StayAtHome()
    {
        startExploring = true;
        startExploiting = true;
    }

    void AtHome()
    {
        startExploring = true;
        startExploiting = true;
    }

}