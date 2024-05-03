using _GenericPackageStart.Core.CustomAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _SpesficCode.Human_Thrower
{
    public class ThrowableHuman:MonoBehaviour
    {
        [FindInChildren][SerializeField] private Rigidbody rigidbody;
        
        public void Throw(Vector3 ThrowForce)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(ThrowForce, ForceMode.Force);
            
        }
    }
}