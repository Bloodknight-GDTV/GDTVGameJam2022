using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConnectedWaypoint : Waypoint
{
    // This is the radius used to find 'nearby' waypoints to create a hub of sorts
    [SerializeField] protected float connectivityRadius = 50f;

    // List of waypoints connected to *this* waypoint
    List<ConnectedWaypoint> connections;

    // Start is called before the first frame update
    void Start()
    {
        // get all waypoints // find a way to limit this to the one room
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        // loop through all waypoints checking for any within the connectivityRadius
        // and then add them to the connections list
        connections = new List<ConnectedWaypoint>();

        for (int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

            if (nextWaypoint != null)
            {
                Vector3 myPos = this.transform.position;
                Vector3 waypointPos = nextWaypoint.transform.position;

                if (Vector3.Distance(myPos, waypointPos) <= connectivityRadius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if (connections.Count == 0)
        {
            // It was at this point, he knew....
            Debug.LogError("Waypoints? What waypoints?");
            return null;
        }
        if (connections.Count == 1 && connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }
        else
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}


