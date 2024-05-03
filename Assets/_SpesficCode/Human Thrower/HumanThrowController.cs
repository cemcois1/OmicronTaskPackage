using System;
using System.Collections;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HumanThrowController : MonoBehaviour
{
    [FindInParent][SerializeField] private InputControlller inputController;
    [SerializeField] private GameObject ThrowableObject;
    [SerializeField] private Transform SlingBelt;

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
    }

    private void Update()
    {
        if (objectThrowing)
        {
            return;
        }

        if (inputController.IsInputTakeable)
        {
            DragBelt();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ThrowObject();
        }
    }

    private void DragBelt()
    {
        var additionalPosition = new Vector3(inputController.DistanceX, 0, inputController.DistanceY) * DragVectorScale;
        var targetPos = dragStartPos + additionalPosition;
        var clampedTargetPos = new Vector3(Mathf.Clamp(targetPos.x, minimumPosition.x, maxPosition.x),
            targetPos.y, Mathf.Clamp(targetPos.z, minimumPosition.y, maxPosition.y));

        SlingBelt.position = Vector3.Lerp(SlingBelt.position, clampedTargetPos, BeltMovementSpeed * Time.deltaTime);
    }

    private void ThrowObject()
    {
        
        var ThrowSequence = DOTween.Sequence();
        ThrowSequence.PrependCallback(() =>
        {
            objectThrowing = true;
        });
        //calculate distance
        var distance = Vector3.Distance(SlingBelt.position, dragStartPos);
        var ThrowAnimationTimeMultiplyer =
            1-((distance > MaxReturnDistance ? MaxReturnDistance : distance) / MaxReturnDistance);
        var ThrowDuration = ThrowAnimationTimeMultiplyer*BeltThrowAnimationDuration;

        ThrowSequence.Append(SlingBelt.DOMove(dragStartPos, ThrowDuration).SetEase(ThrowanimationEase));
        ThrowSequence.AppendCallback(() =>
        {
            objectThrowing = false;
        });

        //Logic
        DOTween.Sequence().AppendInterval(ThrowDuration  / 20).AppendCallback(() =>
        {
            var component = ThrowableObject.GetComponent<Rigidbody>();
            component.isKinematic = false;
            component.AddForce(SlingBelt.forward * inputController.DistanceY, ForceMode.Force);
        });

    }
}