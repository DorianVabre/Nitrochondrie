using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float zPos;
    public Transform player1;
    public Transform player2;
    public float coefBetweenPlayerDistanceAndDezoom = 0.15f;
    public float moveSpeed = 1f;

    void Start() {
        zPos = transform.position.z;
    }

    void Update() {
        Vector3 p1Pos = new Vector3(player1.position.x, player1.position.y, player1.position.z);
        Vector3 p2Pos = new Vector3(player2.position.x, player2.position.y, player2.position.z);
        Vector3 averagePosition = (p1Pos + p2Pos) / 2;

        float distance = Vector3.Distance(p1Pos, p2Pos);

        averagePosition.z = zPos - distance * coefBetweenPlayerDistanceAndDezoom;
        transform.position = Vector3.Lerp(transform.position, averagePosition, Time.deltaTime * moveSpeed);
    }
}
