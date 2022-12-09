using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotMovement : MonoBehaviour
{
    public float forceMultiplier = 50;
    private Vector3 initialPosition;
    private bool isDragging;

    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging) {
            // do stuff during drag
        } else {
            // Updating initial position
            initialPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Cursor.visible = false;

        if (Input.GetButtonDown("Fire1")) {mouseDown();}
        if (Input.GetButtonUp("Fire1")) {mouseUp();}
    }

    // Starts of a drag
    private void mouseDown() {
        GetComponent<SpriteRenderer>().color = Color.red;
        isDragging = true;
    }

    // On release, get the force and apply it with Propel()
    private void mouseUp() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        GetComponent<SpriteRenderer>().color = Color.white;
        Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 forceDirection = initialPosition - draggedPosition;
        Propel(forceDirection);
        isDragging = false;
    }

    // Applying the force to the rigidbody
    private void Propel(Vector3 force) {
        rb.AddForce(force * forceMultiplier);
    }
}