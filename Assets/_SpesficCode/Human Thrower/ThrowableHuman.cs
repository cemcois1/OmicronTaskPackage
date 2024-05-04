using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using _GenericPackageStart.Core.CustomAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _SpesficCode.Human_Thrower
{
    public class ThrowableHuman:MonoBehaviour
    {
        [FindInChildren][SerializeField] private Rigidbody rigidbody;
        [FindInParent][SerializeField] private Animator animator; 
        [FindInParent][SerializeField] private Collider collider;

        
        //ıdle ,fırlatma ıdle ve fırlatma için animasyon keyleri
        [SerializeField] private string idleKey;
        [SerializeField] private string throwIdleKey;
        [SerializeField] private string throwKey;
        [SerializeField] private string jumpKey;
        public void Throw(Vector3 ThrowForce)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(ThrowForce, ForceMode.Force);
            animator.SetTrigger(throwKey);
            collider.enabled = true;
        }
        public void ThrowableIdleAnim()
        {
            animator.SetTrigger(throwIdleKey);
        }
        public void IdleAnim()
        {
            animator.SetTrigger(idleKey);
        }
        public void JumpAnim()
        {
            animator.SetTrigger(jumpKey);
        }
    }
}