using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class WorkerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public  int maxComeAndGoCounter = 8;
    private float _baseSpeed;
    public  float addedTimeWhileGoing;
    public float wheelBorrowCapacity = 10;
    public int countedUntilSleep=0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
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
                        Spawner.Instance.workerList.Push(gameObject);
                        Spawner.Instance.LookForEmptyMachine();
                    }
                });
        }) ;
    }
    
}
