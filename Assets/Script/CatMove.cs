using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CatMove : MonoBehaviour
{
    public GameObject CatPrefab;
    public GameObject tempCatPrefab;
    public GameObject StartPos;
    public GameObject EndPos;
    
    void Start()
    {
        tempCatPrefab = Instantiate(CatPrefab, StartPos.transform);
        CatDoMove();
    }
    
    void CatDoMove()
    {
        tempCatPrefab.transform.DOMove(EndPos.transform.position, 60f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(tempCatPrefab);
                tempCatPrefab = Instantiate(CatPrefab, StartPos.transform);
                CatDoMove();
            });
    }
}
