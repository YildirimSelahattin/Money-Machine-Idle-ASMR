using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Runtime.ExceptionServices;

public class WorkerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static WorkerManager Instance;
    public float maxComeAndGoCounter = 8;
    public float _baseSpeed;
    public float addedTimeWhileGoing;
    public float wheelBorrowCapacity = 10;
    public int countedUntilSleep = 0;
    public int indexThatWorkerGoing;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void MoveMachineAndComeBackByIndex(int index)
    {
        indexThatWorkerGoing = index;
        GameObject machineObject = GameManager.Instance.gridParent.transform.GetChild(index).transform
            .GetChild(GameManager.Instance.MACHINE_CHILD_INDEX).gameObject;
        float firstPartLength =
            Vector3.Distance(transform.position, Spawner.Instance.firstRoadBreakdown[index % 2].position);
        float secondPartLenght = Vector3.Distance(Spawner.Instance.firstRoadBreakdown[index % 2].position,
            machineObject.transform.parent.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).transform
                .position);
        float thirdPartLength =
            Vector3.Distance(
                machineObject.transform.parent.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).transform
                    .position, machineObject.transform.position);
        float fullLenght = secondPartLenght + firstPartLength + thirdPartLength;

        if (indexThatWorkerGoing % 2 == 0)
        {
            transform.DOLocalRotate(new Vector3(0, 25, 0), 0.1f);
        }
        else
        {
            transform.DOLocalRotate(new Vector3(0, -25, 0), 0.1f);
        }

        /*transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[index%2].position,(firstPartLength/ _baseSpeed)+addedTimeWhileGoing).SetEase(Ease.Linear).// go to first breakdown after taking money
            OnComplete(() => transform.DOMove(machineObject.transform.parent.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).transform.position,(secondPartLenght/ _baseSpeed)+ addedTimeWhileGoing).SetEase(Ease.Linear).
            OnComplete(() =>transform.DOLocalMove(machineObject.transform.position,( thirdPartLength / _baseSpeed) + addedTimeWhileGoing).SetEase(Ease.Linear).
            OnComplete(() =>
                {
                    Debug.Log("asdf");
                    GoBackToPile(true);
                    Debug.Log(machineObject.GetComponent<MachineManager>());
                    StartCoroutine(machineObject.GetComponent<MachineManager>().WaitAndPrint());
                })));*/
        transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[index % 2].position,
            (firstPartLength / _baseSpeed) + addedTimeWhileGoing).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            transform.DOLocalMove(
                machineObject.transform.parent.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).transform
                    .position, (secondPartLenght / _baseSpeed) + addedTimeWhileGoing).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    if (indexThatWorkerGoing % 2 == 0)
                    {
                        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
                    }
                    else
                    {
                        transform.DOLocalRotate(new Vector3(0, -90, 0), 0.1f);
                    }

                    transform.DOLocalMove(machineObject.transform.position,
                        (thirdPartLength / _baseSpeed) + addedTimeWhileGoing).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        GoBackToPile(true);
                        Debug.Log(machineObject.GetComponent<MachineManager>());
                        StartCoroutine(machineObject.GetComponent<MachineManager>().WaitAndPrint());
                    });
                });
        });
    }


    public void GoBackToPile(bool deployedSuccesfully)
    {
        if (deployedSuccesfully == false)
        {
            Debug.Log("sui");

            transform.DOLocalRotate(new Vector3(0, -180, 0), 0.2f);
            transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
                    Vector3.Distance(transform.position,
                        Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / _baseSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (indexThatWorkerGoing % 2 == 0)
                    {
                        transform.DOLocalRotate(new Vector3(0, 25, 0), 0.1f);
                    }
                    else
                    {
                        transform.DOLocalRotate(new Vector3(0, -25, 0), 0.1f);
                    }

                    transform.DOLocalMove(Spawner.Instance._spwanPos, _baseSpeed).SetEase(Ease.Linear).OnComplete(() =>
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
                });
            /*transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,Vector3.Distance(transform.position, Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position)/_baseSpeed).SetEase(Ease.Linear)
              .OnComplete(()=>
            transform.DOLocalMove(Spawner.Instance._spwanPos, _baseSpeed).SetEase(Ease.Linear).OnComplete(
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
               }));*/
        }
        else
        {
            Debug.Log("sui");
            if (indexThatWorkerGoing % 2 == 0)
            {
                transform.DOLocalRotate(new Vector3(0, -90, 0), 0.1f);
            }
            else
            {
                transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
            }

            transform.DOLocalMove(
                    GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing)
                        .GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).position,
                    Vector3.Distance(
                        GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing)
                            .GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).position, transform.position))
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.DOLocalRotate(new Vector3(0, 180, 0), 0.1f);

                    transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
                            Vector3.Distance(transform.position,
                                Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / _baseSpeed)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            if (indexThatWorkerGoing % 2 == 0)
                            {
                                transform.DOLocalRotate(new Vector3(0, 180 + 25, 0), 0.2f);
                            }
                            else
                            {
                                transform.DOLocalRotate(new Vector3(0, 180 - 25, 0), 0.2f);
                            }

                            transform.DOLocalMove(Spawner.Instance._spwanPos, _baseSpeed).SetEase(Ease.Linear)
                                .OnComplete(() =>
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
                        });
                });
        }
    }
}