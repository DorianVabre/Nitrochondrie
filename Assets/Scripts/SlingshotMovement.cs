using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotMovement : MonoBehaviour
{
    public GameObject arrowLine;
    public GameObject arrowHead;
    
    public Rigidbody2D bacteriaRigidBody;
    public Sling sling;

    public float forceMultiplier = 100;
    public float deadZone = 0.02f;

    private Vector2 currentMovementInputVector = Vector2.zero;

    private void Awake() {
        //InputSystem.pollingFrequency = 120;
        bacteriaRigidBody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        arrowLine.SetActive(false);
        arrowHead.SetActive(false);
    }

    void Update()
    {
        sling.Update(currentMovementInputVector);

        if (sling.mustFire) {
            arrowLine.SetActive(false);
            arrowHead.SetActive(false);
            sling.Fire();
            Propel();
        } else if (sling.accessValue != 0) {
            UpdateArrow();
            arrowLine.SetActive(true);
            arrowHead.SetActive(true);
        }
    }

    void UpdateArrow() {
        // Moving the direction arrowLine
        Vector2 vectorForDirectionArrow = currentMovementInputVector * -1;
        float angle = Mathf.Atan2(vectorForDirectionArrow.y, vectorForDirectionArrow.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Scaling the line
        Vector2 localScale = new Vector2(sling.accessValue, 1);
        arrowLine.transform.localScale = localScale;
    }

    public void OnMove(InputAction.CallbackContext context) {
        currentMovementInputVector = context.ReadValue<Vector2>();
    }

    public void Propel () {
        bacteriaRigidBody.velocity = Vector3.zero;
        bacteriaRigidBody.angularVelocity = 0;
        bacteriaRigidBody.AddForce(sling.accessVector);
    }
}