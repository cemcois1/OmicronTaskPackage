using System;
using System.Collections;
using System.Collections.Generic;
using _SpesficCode.UI;
using Sirenix.OdinInspector;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    public int MaxFallableObjectCount;
    [SerializeField][ReadOnly]private float currentFallableObjectCount;
    private void OnTriggerEnter(Collider other)
    {
        IHitableObject hittableObject = other.GetComponent<IHitableObject>();
        if (hittableObject != null)
        {
            currentFallableObjectCount += 1;
            UIManager.Instance.UpdateProgressbar(currentFallableObjectCount / MaxFallableObjectCount);
            other.enabled = false;
            hittableObject.Hit(transform.position);
        }
    }
}