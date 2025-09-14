using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header(" Elementos ")]
    [SerializeField] private ParticleSystem seedParticles;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlaySeedParticles()
    {
        seedParticles.Play();
    }
}
