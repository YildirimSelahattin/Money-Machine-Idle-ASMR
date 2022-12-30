using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class WorkerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public  float maxComeAndGoCounter = 8;
    public float _baseSpeed;
    public  float addedTimeWhileGoing;
    public float wheelBorrowCapacity = 10;
    public int countedUntilSleep=0;
    [SerializeField] private GameManager _upgradeButton;

    public void MoveMachineAndComeBackByIndex(int index)
    {
        transform.DOLocalMove(GameManager.Instance.gridParent.transform.GetChild(index).position, _baseSpeed+addedTimeWhileGoing).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            StartCoroutine(GameManager.Instance.gridParent.transform.GetChild(index).transform.GetChild(GameManager.Instance.MACHINE_CHILD_INDEX).gameObject.GetComponent<MachineManager>().WaitAndPrint());
            GoBackToPile();
        }) ;
    }

    public void GoBackToPile()
    {
        transform.DOLocalMove(Spawner.Instance._spwanPos, _baseSpeed).OnComplete(
               () =>
               {
                   countedUntilSleep++;
                   if (countedUntilSleep > maxComeAndGoCounter)
                   {
                       //GoSleep();
                   }
                   else
                   {
                       Spawner.Instance.workerStack.Push(gameObject);
                       Spawner.Instance.LookForEmptyMachine();
                   }
               });
    }
}
