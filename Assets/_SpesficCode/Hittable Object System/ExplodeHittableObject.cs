using Sirenix.OdinInspector;
using UnityEngine;

public class ExplodeHittableObject : HittableObject
{
    [SerializeField] private float explotionRadius = 2f;
    [SerializeField] private float triggerOtherRadius = 0.5f;
    [SerializeField] private float force = 100;
    [SerializeField] private bool exploded=false;
    [SerializeField] private ParticleSystem explotionParticle;
    

    public override void Hit(Vector3 hitPos)
    {
        if (exploded)
        {
            return;
        }

        base.Hit(hitPos);
        Explode();
        exploded = true;
    }
    [Button]
    private void Explode()
    {
        explotionParticle.Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explotionRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(force, transform.position, explotionRadius);
            }
        }        
        Collider[] explodeable = Physics.OverlapSphere(transform.position, explotionRadius);
        foreach (var collider in explodeable)
        {
            if (collider.TryGetComponent(out ExplodeHittableObject hit))
            {
                this.MakeAction(() =>
                {
                    hit.Hit(hit.transform.position);
                }, Random.Range(0f, .4f));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explotionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerOtherRadius);
    }
}