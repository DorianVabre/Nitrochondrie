using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float forceMultiplier = 50;
    public float deadZone = 0.02f;

    private Vector2 currentMovementInput = Vector2.zero;
    private Vector2 lastMovementInput = Vector2.zero; 

    // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
    private Vector3 initialPosition;
    private bool isDragging;   

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        isDragging = false;
    }

    void Update()
    {
        // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
        if (!isDragging) {
            initialPosition = transform.position;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        currentMovementInput = context.ReadValue<Vector2>();
        Debug.Log(currentMovementInput);
        if (Mathf.Abs(currentMovementInput.x) <= deadZone && Mathf.Abs(currentMovementInput.y) <= deadZone) {
            if (lastMovementInput.x != 0 && lastMovementInput.y != 0) {
                Propel(lastMovementInput * -1);
                lastMovementInput = Vector2.zero;
            }
        } else {
            lastMovementInput = currentMovementInput;
        }

    }

    // Applying the force to the rigidbody and resets the input
    private void Propel(Vector2 force) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
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

    // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
    // On release, get the force and apply it with Propel()
    private void OnMouseUp() {
        GetComponent<SpriteRenderer>().color = Color.white;
        Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 forceDirection = initialPosition - draggedPosition;
        Vector2 forceDirection2d = new Vector2(forceDirection.x, forceDirection.y);
        Propel(forceDirection);
        isDragging = false;
    }
}
