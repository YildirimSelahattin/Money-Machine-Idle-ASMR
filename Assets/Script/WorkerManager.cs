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
    public bool waitingForGridDecision = false;
    [SerializeField] GameObject moneyPile;
    public int moveStage; // first part, long part , last part 
    void Start()
    {
        
        StartCoroutine( StartDelayAndMoveToGrid());
    }
  
    public void MoveMachineAndComeBackByIndex()
    {
        transform.DOKill();
        moneyPile.SetActive(true);
        GameObject gridThatWorkerGoing = GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing).gameObject;
        GameObject lastBreakPoint = GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing).transform.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).gameObject;
        GameObject finalPoint = GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing).transform.GetChild(GameManager.Instance.GRID_FINAL_POINT_INDEX).gameObject;
        if (moveStage == 1) {
            if (indexThatWorkerGoing % 2 == 0)
            {
                transform.DOLocalRotate(new Vector3(0, 40, 0), 0.4f);
            }
            else
            {
                transform.DOLocalRotate(new Vector3(0, -40, 0), 0.4f);
            }
            transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
            Vector3.Distance(transform.position, Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / (GameDataManager.Instance.workerBaseSpeed )).SetEase(Ease.Linear).OnComplete(() =>
                {
                    moveStage = 2;
                    transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f);
                    transform.DOLocalMove(
                    lastBreakPoint.transform.position, Vector3.Distance(transform.position,lastBreakPoint.transform.position) / (GameDataManager.Instance.workerBaseSpeed )).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        moveStage = 3;
                        if (indexThatWorkerGoing % 2 == 0)
                        {
                            transform.DOLocalRotate(new Vector3(0, 90, 0), 0.4f);
                        }
                        else
                        {
                            transform.DOLocalRotate(new Vector3(0, -90, 0), 0.4f);
                        }

                        transform.DOLocalMove(finalPoint.transform.position,Vector3.Distance(finalPoint.transform.position, transform.position) / (GameDataManager.Instance.workerBaseSpeed )).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                GoBackToPile();
                                Debug.Log(gridThatWorkerGoing.transform.GetChild(GameManager.Instance.GRID_MACHINE_INDEX).gameObject.GetComponent<MachineManager>());
                                StartCoroutine(gridThatWorkerGoing.transform.GetChild(GameManager.Instance.GRID_MACHINE_INDEX).gameObject.GetComponent<MachineManager>().WaitAndPrint());
                            });
                    });
                });
        }
        else if (moveStage == 2)
        {

            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f);
            transform.DOLocalMove(
            lastBreakPoint.transform.position, Vector3.Distance(transform.position, lastBreakPoint.transform.position)/ (GameDataManager.Instance.workerBaseSpeed)).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                moveStage = 3;
                if (indexThatWorkerGoing % 2 == 0)
                {
                    transform.DOLocalRotate(new Vector3(0, 90, 0), 0.4f);
                }
                else
                {
                    transform.DOLocalRotate(new Vector3(0, -90, 0), 0.4f);
                }
                transform.DOLocalMove(finalPoint.transform.position, Vector3.Distance(finalPoint.transform.position, transform.position) / (GameDataManager.Instance.workerBaseSpeed )).OnComplete(() =>
                    {
                        GoBackToPile();
                        Debug.Log(gridThatWorkerGoing.transform.GetChild(GameManager.Instance.GRID_MACHINE_INDEX).gameObject.GetComponent<MachineManager>());
                        StartCoroutine(gridThatWorkerGoing.transform.GetChild(GameManager.Instance.GRID_MACHINE_INDEX).gameObject.GetComponent<MachineManager>().WaitAndPrint());
                    });
            });
        }

        else if (moveStage == 3)
        {
            if (indexThatWorkerGoing % 2 == 0)
            {
                transform.DOLocalRotate(new Vector3(0, 90, 0), 0.4f);
            }
            else
            {
                transform.DOLocalRotate(new Vector3(0, -90, 0), 0.4f);
            }

            transform.DOLocalMove(finalPoint.transform.position, Vector3.Distance(finalPoint.transform.position, transform.position) / (GameDataManager.Instance.workerBaseSpeed )).SetEase(Ease.Linear).OnComplete(() =>
                {
                    GoBackToPile();
                    Debug.Log(gridThatWorkerGoing.transform.GetChild(GameManager.Instance.GRID_MACHINE_INDEX).gameObject.GetComponent<MachineManager>());
                    StartCoroutine(gridThatWorkerGoing.transform.GetChild(GameManager.Instance.GRID_MACHINE_INDEX).gameObject.GetComponent<MachineManager>().WaitAndPrint());
                });
        }

    }


    public void GoBackToPile()
    {

        GameObject lastBreakPoint = GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing).transform.GetChild(GameManager.Instance.GRID_LAST_BREAKPOINT_INDEX).gameObject;
        GameObject finalPoint = GameManager.Instance.gridParent.transform.GetChild(indexThatWorkerGoing).transform.GetChild(GameManager.Instance.GRID_FINAL_POINT_INDEX).gameObject;
        transform.DOKill();
        if (moveStage == 1)
        {
            if (indexThatWorkerGoing % 2 == 0)
            {
                transform.DOLocalRotate(new Vector3(0, 180 + 40, 0), 0.4f);
            }
            else
            {
                transform.DOLocalRotate(new Vector3(0, 180 - 40, 0), 0.4f);
            }

            transform.DOLocalMove(Spawner.Instance._spwanPos.position, Vector3.Distance(Spawner.Instance._spwanPos.position, transform.position) / (GameDataManager.Instance.workerBaseSpeed)).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (waitingForGridDecision == false)
                    {
                        Debug.Log("a");
                        MoveMachineAndComeBackByIndex();
                    }
                });
        }
        else if (moveStage == 2)
        {
            Debug.Log("sui");
            transform.DOLocalRotate(new Vector3(0, -180, 0), 0.4f);
            transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
                    Vector3.Distance(transform.position,
                        Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / (GameDataManager.Instance.workerBaseSpeed))
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    moveStage = 1;
                    if (indexThatWorkerGoing % 2 == 0)
                    {
                        transform.DOLocalRotate(new Vector3(0, 180 + 40, 0), 0.4f);
                    }
                    else
                    {
                        transform.DOLocalRotate(new Vector3(0, 180 - 40, 0), 0.4f);
                    }
                    transform.DOLocalMove(Spawner.Instance._spwanPos.position, Vector3.Distance(Spawner.Instance._spwanPos.position,transform.position)/ (GameDataManager.Instance.workerBaseSpeed)).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        if (waitingForGridDecision == false)
                        {
                            Debug.Log("a");
                            MoveMachineAndComeBackByIndex();

                        }
                    });
                });
        }
        else if(moveStage == 3)
        {
            Debug.Log("sui");
            moneyPile.SetActive(false);
            if (indexThatWorkerGoing % 2 == 0)
            {
                transform.DOLocalRotate(new Vector3(0, -90, 0), 0.4f);
            }
            else
            {
                transform.DOLocalRotate(new Vector3(0, 90, 0), 0.4f);
            }

            transform.DOLocalMove(
                    lastBreakPoint.transform.position,
                    Vector3.Distance(lastBreakPoint.transform.position, transform.position) / (GameDataManager.Instance.workerBaseSpeed))
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    moveStage = 2;
                    transform.DOLocalRotate(new Vector3(0, 180, 0), 0.4f);
                    transform.DOLocalMove(Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position,
                            Vector3.Distance(transform.position,
                                Spawner.Instance.firstRoadBreakdown[indexThatWorkerGoing % 2].position) / (GameDataManager.Instance.workerBaseSpeed))
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            moveStage=1;
                            if (indexThatWorkerGoing % 2 == 0)
                            {
                                transform.DOLocalRotate(new Vector3(0, 180 + 40, 0), 0.4f);
                            }
                            else
                            {
                                transform.DOLocalRotate(new Vector3(0, 180 - 40, 0), 0.4f);
                            }

                            transform.DOLocalMove(Spawner.Instance._spwanPos.position, Vector3.Distance(Spawner.Instance._spwanPos.position,transform.position) / (GameDataManager.Instance.workerBaseSpeed)).SetEase(Ease.Linear)
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