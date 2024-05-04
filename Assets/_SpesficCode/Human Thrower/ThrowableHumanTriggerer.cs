using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _SpesficCode.Human_Thrower
{
    public class ThrowableHumanTriggerer: MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        private bool isTriggered;
        [SerializeField] private float forceRadius=1;
        [SerializeField] private float forcePower=50;

        [SerializeField] private float explosionTriggerRadius=2;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!isTriggered && other.transform.TryGetComponent(out IHitableObject hitableObject))
            {
                hitableObject.Hit(other.transform.position);
                isTriggered = true;
                particle.Play();
                //yakındaki rigidbodylere force uygula
                Collider[] colliders = Physics.OverlapSphere(transform.position, forceRadius);
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddExplosionForce(forcePower, transform.position, forceRadius);
                    }
                }
            }

            //yakındaki patlayabilir objeleri patlat
            Collider[] explodeable = Physics.OverlapSphere(transform.position, explosionTriggerRadius);
            foreach (var collider in explodeable)
            {
                if (collider.TryGetComponent(out ExplodeHittableObject hit))
                {
                    hit.Hit(hit.transform.position);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, forceRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionTriggerRadius);
        }
    }
}