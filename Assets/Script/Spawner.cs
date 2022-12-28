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
    public Stack<GameObject> workerList;
    public Stack<int> gridArrayStack;

    [SerializeField] private GameObject _endPosObject;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            LookForEmptyMachine();
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
        LookForEmptyMachine();
    }
 
    public void LookForEmptyMachine()
    {
        
        GameObject worker =  workerList.Pop();
        int emptyGridIndex = gridArrayStack.Pop();
        if (worker != null && emptyGridIndex != 0)
        {
            worker.GetComponent<WorkerManager>().MoveMachineAndComeBackByIndex(emptyGridIndex);    
        }
        
    }
}
