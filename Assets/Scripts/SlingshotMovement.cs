using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotMovement : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject arrowLine;
    GameObject arrowHead;

    public AnimationCurve curve;
    public float timeBetweenMovements = 0.5f;
    public float currentTimeSinceLastMove;

    public float forceMultiplier = 100;
    public float deadZone = 0.02f;

    private Vector2 currentMovementInput = Vector2.zero;
    private Vector2 lastMovementInput = Vector2.zero;
    public float minMagnitudeDeltaToTriggerPropel = 0.2f;

    // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
    private Vector3 initialPosition;
    private bool isDragging;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        foreach(Transform transform in transform) {
            if (transform.name == "ArrowHead") {
                arrowHead = transform.gameObject;
            } else if (transform.name == "LineContainer") {
                arrowLine = transform.gameObject;
            }
        }
    }

    void Start() {
        isDragging = false;
        arrowLine.SetActive(false);
        arrowHead.SetActive(false);
        currentTimeSinceLastMove = 0;
    }

    void Update()
    {
        currentTimeSinceLastMove += Time.deltaTime;

        // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
        if (!isDragging) {
            // do stuff during drag
        } else {
            // Updating initial position
            initialPosition = transform.position;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        if (currentTimeSinceLastMove < timeBetweenMovements) {
            return;
        }

        currentMovementInput = context.ReadValue<Vector2>();
        
        // Moving the direction arrowLine
        Vector2 vectorForDirectionArrow = currentMovementInput * -1;
        float angle = Mathf.Atan2(vectorForDirectionArrow.y, vectorForDirectionArrow.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Scaling the line
        Vector2 localScale = new Vector2(currentMovementInput.magnitude, 1);
        arrowLine.transform.localScale = localScale;

        // Showing the arrow
        arrowLine.SetActive(true);
        arrowHead.SetActive(true);

        Vector2 deltaInput = currentMovementInput - lastMovementInput;

        // Moving the bacteria
        if (Vector2.Dot(deltaInput, currentMovementInput) <= 0) {
            // Either the movement towards the idle joystick is enough OR the current movement is slow and back to idle
            if (deltaInput.magnitude >= minMagnitudeDeltaToTriggerPropel || currentMovementInput.magnitude <= deadZone) {
                arrowLine.SetActive(false);
                arrowHead.SetActive(false);
                if (lastMovementInput.x != 0 || lastMovementInput.y != 0) {
                    Propel(lastMovementInput * -1);
                    currentTimeSinceLastMove = 0;
                    lastMovementInput = Vector2.zero;
                }
            } else {
                lastMovementInput = currentMovementInput;
            }
        } else {
            lastMovementInput = currentMovementInput;
        }
    }

    // Applying the force to the rigidbody and resets the input
    private void Propel(Vector2 force) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        float xMovementUsingCurve = curve.Evaluate(force.x);
        float yMovementUsingCurve = curve.Evaluate(force.y);
        Vector2 movementUsingCurve = new Vector2(xMovementUsingCurve, yMovementUsingCurve);
        rb.AddForce(force * forceMultiplier);
    }






    // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
    // Starts of a drag
    private void OnMouseDown() {
        GetComponent<SpriteRenderer>().color = Color.red;
        isDragging = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }

    // On release, get the force and apply it with Propel()
    private void OnMouseUp() {
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
