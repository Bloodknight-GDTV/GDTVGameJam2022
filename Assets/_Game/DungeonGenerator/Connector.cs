using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Connector : MonoBehaviour
{
    public Vector2 size = Vector2.one * 4.0f;
    public bool isConnected = false;

    void OnDrawGizmos()
    {

        Vector2 halfsize = size * 0.5f;
        Vector3 offset = transform.position + transform.up * halfsize.y;

        // top and side
        Vector3 top = transform.up * size.x;
        Vector3 side = transform.right * halfsize.y;


        // define corner vectors
        Vector3 topRight = transform.position + top + side;
        Vector3 topleft = transform.position + top - side;
        Vector3 bottomRight = transform.position + side;
        Vector3 bottomLeft = transform.position - side;


        // draw lines to define the 'object'
        Gizmos.color = Color.green;
        Gizmos.DrawLine(offset, offset + transform.forward);

        Gizmos.color = Color.cyan * 0.8f;
        Gizmos.DrawLine(topRight, topleft);
        Gizmos.DrawLine(topleft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);

        Gizmos.color = Color.cyan * 0.4f;
        Gizmos.DrawLine(topRight, offset);
        Gizmos.DrawLine(topleft, offset);
        Gizmos.DrawLine(bottomRight, offset);
        Gizmos.DrawLine(bottomLeft, offset);

    }
}