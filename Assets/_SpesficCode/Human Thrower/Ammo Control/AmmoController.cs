using System.Collections;
using System.Collections.Generic;
using _SpesficCode.Human_Thrower;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmoController : MonoBehaviour
{
    [SerializeField] private List<ThrowableHuman> throwableHumans;
    [SerializeField] private Transform throwTransform;
    
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private float startDelay=1f;
    [SerializeField] private float jumpPower=3f;
    
    [Button]
    public void FindInChilds()
    {
        throwableHumans.Clear();
        throwableHumans.AddRange(transform.GetComponentsInChildren<ThrowableHuman>());
    }


    public Sequence LoadAmmo(out GameObject throwableObject)
    {
        var reloadSequence = DOTween.Sequence(this);
        if (throwableHumans.Count == 0)
        {
            Debug.LogError("There is no ammo to throw");
            throwableObject = null;
            return null;
        }
        reloadSequence.AppendInterval(startDelay);
        reloadSequence.AppendCallback(() => throwableHumans[0].JumpAnim());
        reloadSequence.Append(throwableHumans[0].transform.DOJump(throwTransform.position, jumpPower,1, reloadTime));
        reloadSequence.AppendCallback(() => throwableHumans.RemoveAt(0));
        throwableObject = throwableHumans[0].gameObject;
        return reloadSequence;

    }

}