using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using UnityEngine;

public class HittableObject : MonoBehaviour,IHitableObject
{
    [FindInParent][SerializeField] private Rigidbody rigidbody;
    [SerializeField] private bool fallable=true;
    [SerializeField] private float MinimumVelocityForFalling = 7;
    [SerializeField] private ParticleSystem particle;
    
    public void Hit(Vector3 hitPos)
    {
        Debug.Log("Hit");
    }

    public void Droped(Vector3 hitPos)
    {
        Debug.Log("Droped");
        fallable = false;
        particle.transform.position = hitPos;
        particle.Play();
        this.MakeAction(() => { gameObject.SetActive(false); }, 1f);
    }

    public bool IsFalling
    {
        get
        {
            if (fallable == false)
            {
                return false;
            }
            Debug.Log(rigidbody.velocity.magnitude);
            return rigidbody.velocity.magnitude >MinimumVelocityForFalling;
        } 
    }
}