using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header(" Elementos")]
    [SerializeField] private Animator animator;
    

    [Header(" Configuraciones")]
    [SerializeField] private float moveSpeedMultiplier;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

}
