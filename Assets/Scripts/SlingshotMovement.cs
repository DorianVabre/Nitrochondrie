using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotMovement : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject arrowLine;
    GameObject arrowHead;

    public float forceMultiplier = 50;
    public float deadZone = 0.02f;

    private Vector2 currentMovementInput = Vector2.zero;
    private Vector2 lastMovementInput = Vector2.zero; 

    // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
    private Vector3 initialPosition;
    private bool isDragging;   

    private void Awake() {
        InputSystem.pollingFrequency = 120;
        rb = GetComponent<Rigidbody2D>();

        foreach (Transform child in transform){
            if (child.name == "LineContainer"){
                arrowLine = child.gameObject;
            }
            if (child.name == "ArrowHead"){
                arrowHead = child.gameObject;
            }
        }
    }

    void Start() {
        isDragging = false;
        arrowLine.SetActive(false);
        arrowHead.SetActive(false);
    }

    void Update()
    {
        // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
        if (!isDragging) {
            initialPosition = transform.position;
        }

        Cursor.visible = false;

        if (Input.GetButtonDown("Fire1")) {mouseDown();}
        if (Input.GetButtonUp("Fire1")) {mouseUp();}
    }

    public void OnMove(InputAction.CallbackContext context) {
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

        // Moving the bacteria
        if (Mathf.Abs(currentMovementInput.x) <= deadZone && Mathf.Abs(currentMovementInput.y) <= deadZone) {
            arrowLine.SetActive(false);
            arrowHead.SetActive(false);
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
    private void mouseDown() {
        GetComponent<SpriteRenderer>().color = Color.red;
        isDragging = true;
    }

    // FOR MOUSE ONLY, CAN BE DELETED AFTER CONTROLLER DONE
    // On release, get the force and apply it with Propel()
    private void mouseUp() {
        GetComponent<SpriteRenderer>().color = Color.white;
        Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 forceDirection = initialPosition - draggedPosition;
        Vector2 forceDirection2d = new Vector2(forceDirection.x, forceDirection.y);
        Propel(forceDirection);
        isDragging = false;
    }
}
