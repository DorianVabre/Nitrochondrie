using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotMovement : MonoBehaviour
{
    
    public Rigidbody2D bacteriaRigidBody;
    public Sling sling;

    private Vector2 _currentMovementInputVector = Vector2.zero;

    public void OnMove (InputAction.CallbackContext context) {

        _currentMovementInputVector = context.ReadValue<Vector2>();
    }

    public void Update () {
        sling.Update(_currentMovementInputVector);

        if (sling.mustFire) {
            sling.Fire();            
            Propel();
        }
    }

    public void Propel () {
        bacteriaRigidBody.velocity = Vector3.zero;
        bacteriaRigidBody.angularVelocity = 0;
        bacteriaRigidBody.AddForce(sling.accessVector);
    }
}
