using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header(" Elementos")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem waterParticles;
    

    [Header(" Configuraciones")]
    [SerializeField] private float moveSpeedMultiplier;

    public void ManageAnimations(Vector3 moveVector)
    {
        if(moveVector.magnitude > 0)
        {
            animator.SetFloat("moveSpeed", moveVector.magnitude * moveSpeedMultiplier);
            PlayRunTimeAnimation();

            animator.transform.forward = moveVector.normalized;
        }
        else
        {
            PlayIdleTimeAnimation();
        }
    }
    private void PlayRunTimeAnimation()
    {
        animator.Play("Run");
    }
    private void PlayIdleTimeAnimation()
    {
        animator.Play("Idle");            
    }
    public void PlaySowAnimation()
    {
        animator.SetLayerWeight(1,1);
    }
    public void StopSowAnimation()
    {
        animator.SetLayerWeight(1, 0);
    }

    public void PlayWaterAnimation()
    {        
        animator.SetLayerWeight(2, 1); 
    }
    public void StopWaterAnimation()
    {
        animator.SetLayerWeight(2, 0);
        waterParticles.Stop();
    }
    public void PlayHarvestAnimation()
    {
        animator.SetLayerWeight(3, 1);
    }
    public void StopHarvestAnimation()
    {
        animator.SetLayerWeight(3, 0);
    }
}
