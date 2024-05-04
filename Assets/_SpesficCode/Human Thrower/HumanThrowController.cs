using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using _SpesficCode.Human_Thrower;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanThrowController : MonoBehaviour
{
    [FindInParent][SerializeField] private InputControlller inputController;
    [SerializeField] private CustomLineRenderer customLineRenderer;
    [SerializeField] private AmmoController ammoController;
    
    [SerializeField] private GameObject ThrowableObject;
    [SerializeField] private Transform SlingBelt;
    
    [SerializeField] private float ForceMultiplyer=1;

    [FoldoutGroup("Belt Movement Settings")] 
    [SerializeField] private float DragVectorScale=1f;
    [FoldoutGroup("Belt Movement Settings")] [SerializeField] private float BeltMovementSpeed=2f;
    [FoldoutGroup("Belt Movement Settings")] [SerializeField] private Vector2 minimumPosition;
    [FoldoutGroup("Belt Movement Settings")] [SerializeField] private Vector2 maxPosition;

    [FoldoutGroup("Throw Settings")] [SerializeField, ReadOnly]
    private Vector3 dragStartPos;
    [FoldoutGroup("Throw Settings")]
    [SerializeField] private float MaxReturnDistance;

    [FoldoutGroup("Throw Settings")] [SerializeField]
    private bool objectThrowing;
    [FoldoutGroup("Throw Settings")]
    [SerializeField] private float BeltThrowAnimationDuration=1;
    Ease ThrowanimationEase = Ease.OutElastic;

    [FoldoutGroup("Editor")] [SerializeField]
    private Transform maxTransform;
    [FoldoutGroup("Editor")] [SerializeField]
    private Transform minTransform;  
    
    [FoldoutGroup("Bullet")]
    [SerializeField] private Transform hitPoint;
    private Vector3 hitPointStartPos;

    [FoldoutGroup("Bullet")] [SerializeField] private float maxYValue;
    [FoldoutGroup("Bullet")] [ReadOnly][SerializeField]private  Vector3 ForceVector;
    [FoldoutGroup("Bullet")] [SerializeField]private  float SensitivityX=1.5f;
    [SerializeField] private float additionalZLimit=5;
    [SerializeField] private bool objectLoaded;


    [FoldoutGroup("Editor")] 
    [Button]
    public void SetBounds()
    {
        maxPosition = new Vector2(maxTransform.position.x, maxTransform.position.z);
        minimumPosition = new Vector2(minTransform.position.x, minTransform.position.z);
    }

    
    private void OnEnable()
    {
        dragStartPos = SlingBelt.position;
        hitPointStartPos = hitPoint.position;
        
        var loadseq=ammoController.LoadAmmo(out ThrowableObject);
        if (loadseq != null)
        {
            loadseq.AppendCallback(() => BindHumanToBelt());
        }
    }
    

    private void Update()
    {
        if (objectThrowing||EventSystem.current.IsPointerOverGameObject()|| !objectLoaded)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            BindHumanToBelt();
            customLineRenderer.ShowLine();
        }
        if (inputController.IsInputTakeable)
        {
            //Drag Belt
            DragBelt();
            
            //Calculate Power and Draw Line
            UpdateHitPoint();
            //Draw Line
            customLineRenderer.DrawLine(dragStartPos,hitPoint.position);
            
        }

        if (Input.GetMouseButtonUp(0))
        { 
            customLineRenderer.HideLine();
            ThrowObject(ForceVector);
        }
    }

    private void UpdateHitPoint()
    {
        ForceVector = (hitPoint.position - dragStartPos).normalized;
        var distanceZ = Mathf.Abs(SlingBelt.position.z - dragStartPos.z);
        var powerRatio = (distanceZ > MaxReturnDistance ? MaxReturnDistance : distanceZ) / MaxReturnDistance;

        //0 ile 1 arasında olan değeri -1 ile 1 arasına çevir
        var xPosRatio = StaticMethods.GetLerpedValue(minimumPosition.x, maxPosition.x, SlingBelt.position.x, 0, 1);
        var hitXposition = Mathf.Lerp(minimumPosition.x, maxPosition.x, 1 - xPosRatio) * SensitivityX;
        var hitYPos = Mathf.Lerp(hitPointStartPos.y, maxYValue, powerRatio);

        hitPoint.position = new Vector3(hitXposition, hitYPos,
            Mathf.Lerp(hitPointStartPos.z, hitPointStartPos.z + additionalZLimit, powerRatio));
    }

    private void DragBelt()
    {
        var additionalPosition =
            new Vector3(inputController.DistanceX, 0, inputController.DistanceY) * DragVectorScale;
        var targetPos = dragStartPos + additionalPosition;
        var clampedTargetPos = new Vector3(Mathf.Clamp(targetPos.x, minimumPosition.x, maxPosition.x),
            targetPos.y, Mathf.Clamp(targetPos.z, minimumPosition.y, maxPosition.y));
        SlingBelt.position = Vector3.Lerp(SlingBelt.position, clampedTargetPos, BeltMovementSpeed * Time.deltaTime);
    }


    private void BindHumanToBelt()
    {
        objectLoaded = true;
        ThrowableObject.transform.SetParent(SlingBelt, true);
        var human = ThrowableObject.GetComponent<ThrowableHuman>();
        human.ThrowableIdleAnim();
    }
    private void ThrowObject(Vector3 forceVector)
    {
        
        var ThrowSequence = DOTween.Sequence();
        ThrowSequence.PrependCallback(() =>
        {
            objectThrowing = true;
            objectLoaded = false;
        });
        //calculate distance
        var distance = Vector3.Distance(SlingBelt.position, dragStartPos);
        var powerRatio = (distance > MaxReturnDistance ? MaxReturnDistance : distance) / MaxReturnDistance;
        
        var ThrowAnimationTimeMultiplyer = 1-powerRatio;
        var ThrowDuration = ThrowAnimationTimeMultiplyer*BeltThrowAnimationDuration;

        ThrowSequence.Append(SlingBelt.DOMove(dragStartPos, ThrowDuration).SetEase(ThrowanimationEase));
        ThrowSequence.AppendCallback(() =>
        {
            objectThrowing = false;
        });

        //Logic
        DOTween.Sequence().AppendInterval(ThrowDuration  / 50).AppendCallback(() =>
        {
            ThrowableObject.transform.SetParent(transform, true);
            var human = ThrowableObject.GetComponent<ThrowableHuman>();
            human.Throw(forceVector * ForceMultiplyer);
            
            ThrowableObject = null;

            var loadseq = ammoController.LoadAmmo(out ThrowableObject);
            if (loadseq != null)
            {
                loadseq.AppendCallback(() => BindHumanToBelt());
            }
            else
            {
                HittableObjectHolder.CheckLevelFailed?.Invoke();
            }
            
        });

    }
}