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
    public Stack<GameObject> workerStack = new Stack<GameObject>();
    public Stack<int> gridArrayStack = new Stack<int>();
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
            //worker.GetComponent<WorkerManager>()._workerData.wheelBorrowCapacity = GameDataManager.Instance.workerArray[i].wheelBorrowCapacity;
            WorkerManager a = worker.GetComponent<WorkerManager>();
            Debug.Log(GameDataManager.Instance.workerArray[i].wheelBorrowCapacity);
            a.addedTimeWhileGoing = GameDataManager.Instance.workerArray[i].addedTimeWhileGoing;
            a.maxComeAndGoCounter = GameDataManager.Instance.workerArray[i].maxComeAndGoCounter;
            a._baseSpeed = GameDataManager.Instance.workerArray[i]._baseSpeed;
            workerStack.Push(worker);
        }
        LookForEmptyMachine();
    }
 
    public void LookForEmptyMachine()
    {
        if (workerStack.Count != 0 && gridArrayStack.Count!= 0)
        {
            GameObject worker =  workerStack.Pop();
            while (true)
            {
                int availableGridIndex = gridArrayStack.Pop();
                if(availableGridIndex == null)
                {
                    break;
                }
                if (GameDataManager.Instance.gridArray[availableGridIndex] > 0)// this is the case when 
                {
                    worker.GetComponent<WorkerManager>().MoveMachineAndComeBackByIndex(availableGridIndex);
                    break;
                }
            }
        }
        
    }
}
