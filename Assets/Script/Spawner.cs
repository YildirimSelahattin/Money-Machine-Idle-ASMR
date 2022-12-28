using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    public GameObject prefab;
    public Vector3 _spwanPos;
    private int _moneyAmount = 0;
    public Stack<GameObject> workerStack;
    public Stack<int> gridArrayStack;
    public GameObject workerPrefab;
    [SerializeField] private GameObject _endPosObject;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < GameDataManager.Instance.gridArray.Length; i++)
        {
            if (GameDataManager.Instance.gridArray[i] > 0)
            {
                gridArrayStack.Push(i);
            }
        }
        for (int i = 0; i < GameDataManager.Instance.workerArray.Length; i++)
        {
            GameObject worker = Instantiate(workerPrefab,_spwanPos,Quaternion.identity);
            worker.AddComponent<WorkerManager>();
            worker.GetComponent<WorkerManager>().wheelBorrowCapacity = GameDataManager.Instance.workerArray[i].wheelBorrowCapacity;
            worker.GetComponent<WorkerManager>().addedTimeWhileGoing = GameDataManager.Instance.workerArray[i].addedTimeWhileGoing;
            worker.GetComponent<WorkerManager>().maxComeAndGoCounter = GameDataManager.Instance.workerArray[i].maxComeAndGoCounter;
            worker.GetComponent<WorkerManager>()._baseSpeed = GameDataManager.Instance.workerArray[i]._baseSpeed;
            workerStack.Push(worker);
        }
        LookForEmptyMachine();
    }
 
    public void LookForEmptyMachine()
    {
        GameObject worker =  workerStack.Pop();
        int emptyGridIndex = gridArrayStack.Pop();
        if (worker != null && emptyGridIndex != 0)
        {
            worker.GetComponent<WorkerManager>().MoveMachineAndComeBackByIndex(emptyGridIndex);    
        }
        
    }
}
