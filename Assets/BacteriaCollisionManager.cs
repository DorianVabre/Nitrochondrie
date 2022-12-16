using UnityEngine;

public class BacteriaCollisionManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        SlingshotMovement sm = GetComponent<SlingshotMovement>();
        SlingshotMovement otherSm = other.transform.GetComponent<SlingshotMovement>();

        if (!otherSm) {
            return;
        }

        float selfVelocity = sm.velocityBeforePhysicsUpdate.magnitude;
        float otherVelocity = otherSm.velocityBeforePhysicsUpdate.magnitude;

        if (selfVelocity < otherVelocity) {
            PortraitAnimatorManager pam = GetComponent<PortraitAnimatorManager>();
            if (pam) pam.TriggerHurt();
            sm.sfxManager.PlayRandomHitPlayer();
            sm.sfxManager.PlayRandomHitLine();
        }
    }
}
