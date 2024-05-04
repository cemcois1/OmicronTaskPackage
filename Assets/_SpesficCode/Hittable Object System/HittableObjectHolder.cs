using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HittableObjectHolder : MonoBehaviour
{
    [SerializeField] private List<HittableObject> hittableObjects;
    [SerializeField] private int fallableObjectCount;
    
    private void Awake()
    {
        fallableObjectCount = hittableObjects.Count;
    }

    [Button]
    public void FindHittableObjects()
    {
        hittableObjects.AddRange(transform.GetComponentsInChildren<HittableObject>());
    }
    
}