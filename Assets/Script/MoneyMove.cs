using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MoneyMove : MonoBehaviour
{
    public static MoneyMove Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void MoneyMoveTruck()
    {
        Debug.LogError("Move Position: " + MachineManager.Instance._firstStep);
        transform.DOMove(MachineManager.Instance._firstStep, GameDataManager.Instance.beltSpeed).OnComplete(
            () => { transform.DOMove(new Vector3(0, 0, 0), GameDataManager.Instance.beltSpeed); });
    }
}