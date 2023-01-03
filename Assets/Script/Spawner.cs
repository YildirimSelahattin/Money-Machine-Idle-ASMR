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
    public GameObject workerPrefab;

    public GameObject[] gridWorkerArray = new GameObject[6];
    [SerializeField] private GameObject _endPosObject;
    public Transform[] firstRoadBreakdown;
    Hashtable workerGrid = new Hashtable();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // go for every grid 
        for (int i = 0; i < GameDataManager.Instance.gridArray.Length; i++)
        {
            // if that have machine, ýnstantiate a worker for this machine 
            if (GameDataManager.Instance.gridArray[i] > 0)
            {
                AddWorker(i);
            }
        }
    }
    public void AddWorker(int index)
    {
        GameObject worker = Instantiate(workerPrefab, _spwanPos, Quaternion.identity);
        WorkerManager a = worker.GetComponent<WorkerManager>();
        a.indexThatWorkerGoing = index;
        gridWorkerArray[index] = worker;
    }
}