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
    public int[] gridCountedArray;
    public GameObject prefab;
    private ObjectPooling _pool;
    public Vector3 _spwanPos;
    private Vector3 _endPos;
    private int _moneyAmount = 0;
    public List<WorkerManager> workerArray;
    public Stack<GameObject> workerStack;
    public Stack<int> gridArrayStack;

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
        for (int i = 0; i < workerArray.Count; i++)
        {
            GameObject x = new GameObject();
            x.AddComponent<WorkerManager>();
            x.GetComponent<WorkerManager>().wheelBorrowCapacity = workerArray[i].wheelBorrowCapacity;
            x.GetComponent<WorkerManager>().addedTimeWhileGoing = workerArray[i].addedTimeWhileGoing;
            x.GetComponent<WorkerManager>().maxComeAndGoCounter = workerArray[i].maxComeAndGoCounter;
            x.GetComponent<WorkerManager>()._baseSpeed = workerArray[i]._baseSpeed;
            workerStack.Push(x);
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
