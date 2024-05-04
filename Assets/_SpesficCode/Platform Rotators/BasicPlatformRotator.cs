using DG.Tweening;
using UnityEngine;

namespace _SpesficCode.Platform_Rotators
{
    public class BasicPlatformRotator : MonoBehaviour
    {
        [SerializeField] private float turnDuration=1f;
        
        private void OnEnable()
        {
            transform.DORotate(new Vector3(0, 360,0 ), turnDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            transform.DOKill(true);
        }
    }
}
