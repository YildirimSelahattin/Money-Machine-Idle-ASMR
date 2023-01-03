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
    public int indexThatWorkerGoing;
    public static WorkerManager Instance;
    public bool waitingForGridDecision =false;
    void Start()
    {
        
        StartCoroutine( StartDelayAndMoveToGrid());
    }
    private void Update()
    {
        Debug.Log(waitingForGridDecision);
    }
    public void MoveMachineAndComeBackByIndex()
    {
        GameObject machineObject = GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing).transform
            .GetChild(GameManager.Instance.MACHINE_CHILD_INDEX).gameObject;
        float firstPartLength =
            Vector3.Distance(transform.position, Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position);
        float secondPartLenght = Vector3.Distance(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
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

        transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
            (firstPartLength / GameDataManager.Instance.workerBaseSpeed) + (GameDataManager.Instance.workerBaseSpeed * 0.5f)).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            transform.DOLocalMove(
                machineObject.transform.parent.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).transform
                    .position, (secondPartLenght / GameDataManager.Instance.workerBaseSpeed) + (GameDataManager.Instance.workerBaseSpeed * 0.5f)).SetEase(Ease.Linear).OnComplete(
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
                        (thirdPartLength / GameDataManager.Instance.workerBaseSpeed) + (GameDataManager.Instance.workerBaseSpeed * 0.5f)).SetEase(Ease.Linear).OnComplete(() =>
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
        transform.DOKill();
        if (deployedSuccesfully == false)
        {
            Debug.Log("sui");
            transform.DOLocalRotate(new Vector3(0, -180, 0), 0.2f);
            transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
                    Vector3.Distance(transform.position,
                        Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / GameDataManager.Instance.workerBaseSpeed)
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
                    transform.DOLocalMove(Spawner.Instance._spwanPos, GameDataManager.Instance.workerBaseSpeed).SetEase(Ease.Linear).OnComplete(() =>
                    {

                        if (waitingForGridDecision == false)
                        {
                            Debug.Log("a");
                            MoveMachineAndComeBackByIndex();

                        }
                    });
                });
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
                                Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / GameDataManager.Instance.workerBaseSpeed)
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

                            transform.DOLocalMove(Spawner.Instance._spwanPos, GameDataManager.Instance.workerBaseSpeed).SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    if (waitingForGridDecision == false)
                                    {
                                        Debug.Log("a");
                                        MoveMachineAndComeBackByIndex();
                                    }
                                });
                        });
                });
        }
    }

    public IEnumerator StartDelayAndMoveToGrid()
    {
        yield return new WaitForSeconds(1);
        MoveMachineAndComeBackByIndex();
    }
}