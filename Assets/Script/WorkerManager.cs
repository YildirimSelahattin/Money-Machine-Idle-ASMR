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
    public float countedUntilSleep =0;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    public void MoveMachineAndComeBackByIndex(int index)
    {
        transform.DOLocalMove(GameManager.Instance.gridParent.transform.GetChild(index).position, _baseSpeed+addedTimeWhileGoing).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            StartCoroutine(GameManager.Instance.gridParent.transform.GetChild(index).transform.GetChild(3).gameObject.GetComponent<MachineManager>().WaitAndPrint(MachineManager.Instance.countWaitTime));
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
        }) ;
    }
    
}
