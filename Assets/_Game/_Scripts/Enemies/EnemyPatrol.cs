using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    // To wait or not to wait, that is the question
    [SerializeField] bool patrolWaiting;
    // this should be a range?
    [SerializeField] float totalWaitTime = 3.0f;
    [SerializeField] float switchProbability = 0.2f;
    [SerializeField] List<Waypoint> patrolPoints;

    NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool travelling;
    bool waiting;
    bool patrolForward;
    float waitTimer;





    // Start is called before the first frame update
    void Start()
    {
        // TODO fill the patrol waypoint list **is there a better way??**
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        for (int i = 0; i < allWaypoints.Length; i++)
        {
            patrolPoints.Add(allWaypoints[i].GetComponent<Waypoint>());
        }

        navMeshAgent = this.GetComponent<NavMeshAgent>();

        //
        if (navMeshAgent == null)
        {
            // Bleargh
            Debug.Log("no navmesh agent on");
        }
        else
        {
            // Patrol needs at least 2 waypoints
            if (patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                // Bleargh
                Debug.Log("Who you gonna call!");
            }
        }
    }


    void Update()
    {

        if (travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            travelling = false;

            //to wait, apparently 
            if (patrolWaiting)
            {
                waiting = true;
                waitTimer = 0.0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= totalWaitTime)
            {
                waiting = false;
                ChangePatrolPoint();
                SetDestination();
            }
        }

    }


    private void SetDestination()
    {

        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;

            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }

    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) <= switchProbability)
        {
            // switch 
            patrolForward = !patrolForward;
        }

        if (patrolForward)
        {
            //currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;

            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Count)
            {
                currentPatrolIndex = 0;
            }
        }
        else
        {
            if (--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }
}
