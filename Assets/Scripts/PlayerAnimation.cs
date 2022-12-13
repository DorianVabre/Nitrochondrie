using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Rigidbody[] list;
    [SerializeField] Transform[] movableList;
    [SerializeField] Transform centerDummy;
    [SerializeField] Transform center;

    Vector2 currentMovementInput;
    Vector3 footForce;
    [SerializeField] int firstIntsToMove;

    [SerializeField] float length = 5f;

    [SerializeField] float stretchPower = 2f;
    [SerializeField] float stretchSpeed = 2f;

    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    Animator anim;
    bool canTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentMovementInput.magnitude < 0.2f && canTrigger)
        {
            canTrigger = false;
            TriggerAnim();
        }
        else
        {
            if (currentMovementInput.magnitude > 0.2f)
            {
                canTrigger = true;
            }
        }

        foreach (Rigidbody item in list)
        {
            item.AddForce(footForce * Time.deltaTime * length);
        }
        foreach (Transform item in movableList)
        {
            Vector3 desiredLegPosition = new Vector3(0, 1f, 0) * Time.deltaTime * stretchPower * currentMovementInput.magnitude;
            item.localPosition = Vector3.Lerp(item.localPosition, desiredLegPosition, Time.deltaTime * stretchSpeed);

        }

        skinnedMesh.SetBlendShapeWeight(1, currentMovementInput.magnitude * 100f);
    }

    public void TriggerAnim()
    {
        anim.SetTrigger("Choc");
    }


    public void MoveBody(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        footForce = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);
    }
}
