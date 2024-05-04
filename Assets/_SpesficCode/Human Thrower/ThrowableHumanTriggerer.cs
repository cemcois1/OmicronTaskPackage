using System;
using UnityEngine;

namespace _SpesficCode.Human_Thrower
{
    public class ThrowableHumanTriggerer: MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        private bool isTriggered;
        private void OnCollisionEnter(Collision other)
        {
            if (!isTriggered && other.transform.TryGetComponent(out IHitableObject hitableObject))
            {
                isTriggered = true;
                particle.Play();
            }
        }
    }
}