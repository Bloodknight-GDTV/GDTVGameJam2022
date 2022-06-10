using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConnectedPatrol : MonoBehaviour
{
    // To wait or not to wait, that is the question
    [SerializeField] bool patrolWaiting;

    // this should be a range?
    [SerializeField] float totalWaitTime = 3.0f;

    //[SerializeField] float switchProbability = 0.2f;

    //[SerializeField] List<Waypoint> patrolPoints;
    ConnectedWaypoint currentWaypoint;
    ConnectedWaypoint previousWaypoint;
    int waypointsVisited;

    NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool travelling;
    bool waiting;
    //bool patrolForward;
    float waitTimer;


    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogWarning($"no navmesh agent on {gameObject.name}");
        }
        else
        {
            if (currentWaypoint == null)
            {
                // Get a list of all waypoints
                // TODO Make this work for room only
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int randomWaypoint = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[randomWaypoint].GetComponent<ConnectedWaypoint>();

                        if (startingWaypoint != null)
                        {
                            currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("No waypoints found");
                }
            }

            SetDestination();
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            travelling = false;
            waypointsVisited++;

            //to wait, apparently 
            if (patrolWaiting)
            {
                waiting = true;
                waitTimer = 0.0f;
            }
            else
            {
                SetDestination();
            }
        }

        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= totalWaitTime)
            {
                waiting = false;
                SetDestination();
            }
        }

    }


    private void SetDestination()
    {
        if (waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
        }

        Vector3 targetVector = currentWaypoint.transform.position;
        navMeshAgent.SetDestination(targetVector);
        travelling = true;
    }

    // private void ChangePatrolPoint()
    // {
    //     if (UnityEngine.Random.Range(0.0f, 1.0f) <= switchProbability)
    //     {
    //         // switch 
    //         patrolForward = !patrolForward;
    //     }

    //     if (patrolForward)
    //     {
    //         //currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;

    //         currentPatrolIndex++;
    //         if (currentPatrolIndex >= patrolPoints.Count)
    //         {
    //             currentPatrolIndex = 0;
    //         }
    //     }
    //     else
    //     {
    //         if (--currentPatrolIndex < 0)
    //         {
    //             currentPatrolIndex = patrolPoints.Count - 1;
    //         }
    //     }
}

