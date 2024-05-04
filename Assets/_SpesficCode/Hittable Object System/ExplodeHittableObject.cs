using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ExplodeHittableObject : HittableObject
{
    [SerializeField] private float explotionRadius = 2f;
    [SerializeField] private float triggerOtherRadius = 0.5f;
    [SerializeField] private float force = 100;
    [SerializeField] private bool exploded=false;
    [SerializeField] private ParticleSystem explotionParticle;
    
    
    [FindInParent][SerializeField] private MeshRenderer meshRenderer;
    [FindInParent][SerializeField] private Collider collider;
    

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
                rb.isKinematic = false;
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

        transform.DOKill(false);
        transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.OutBack).SetDelay(.8f).OnComplete(() =>
        {
            Destroy(transform.gameObject);
        });

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explotionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerOtherRadius);
    }
}