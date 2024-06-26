using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HittableObjectHolder : MonoBehaviour
{
    [SerializeField] private List<HittableObject> hittableObjects;
    [SerializeField] private int fallableObjectCount;
    [SerializeField] private FallTrigger fallTrigger;
    [SerializeField] private List<HittableObject> fallingList;
    public static Action CheckLevelFailed;

    private void Awake()
    {
        fallTrigger.MaxFallableObjectCount = fallableObjectCount = hittableObjects.Count;
    }

    #region Level Failed Checker

    private void OnEnable()
    {
        CheckLevelFailed += LevelFailedCoroutine;
    }

    private void OnDisable()
    {
        CheckLevelFailed -= LevelFailedCoroutine;
    }

    private IEnumerator LevelFailed()
    {
        //hittableObjectsten Falling Olanları Bul
        //Falling olanlar hareket etmeyene kadar bekle max 5 sn bekle 
        //eğer Level Kazanılmadıysa Level Failed
        //eğer Level Kazanıldıysa Level Win
        fallingList = hittableObjects.FindAll(x => x.IsFalling);
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(1);
            if (fallingList.Count == 0)
            {
                Debug.Log("fallable all items Falled");
                break;
            }
            
            List<HittableObject> removeList=new List<HittableObject>();
            foreach (var fall in fallingList)
            {
                if (!fall.IsFalling)
                {
                    //listeden sil

                    removeList.Add(fall);
                }
            }
            fallingList.RemoveAll(x => removeList.Contains(x));
        }

        yield return new WaitForSeconds(4);

        //hittableObjectsin bütün elemanları düştüymediyse değilse level failed
        hittableObjects.RemoveAll(x => !x.gameObject.activeInHierarchy);
        if (hittableObjects.Count != 0&&!GameManager.instance.LevelFinished)
        {
            Debug.Log("Level Failed");
            GameManager.levelFailed?.Invoke();
        }
    }

    public void LevelFailedCoroutine()
    {
        StartCoroutine(LevelFailed());
    }

    #endregion

    [Button]
    public void FindHittableObjects()
    {
        hittableObjects.Clear();
        hittableObjects.AddRange(transform.GetComponentsInChildren<HittableObject>().Where(hitableObj => hitableObj.GetType()!=typeof(ExplodeHittableObject)));
    }

}