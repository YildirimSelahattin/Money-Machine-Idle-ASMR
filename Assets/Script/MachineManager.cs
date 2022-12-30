using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Mono.CompilerServices.SymbolWriter;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;
    public int levelIndexOfObject;
    public int gridIndexNumberOfObject;
    public bool dropped = true;
    public bool inMergeArea = false;
    public bool isWorking = false;
    public float countWaitTime;
    private AudioSource _moneySound;
    private TMP_Text _counterText;
    public bool inSnapArea = false;
    public bool isAtBank = false;
    [SerializeField] private GameObject _moneyPrefab;
    public bool isFinishedCount = false;
    public Vector3 _firstStep = new Vector3(3,3,3);
    public float waitTime ;
    public GameObject comingWorkerObject;
    public GameObject lastBreakdownBeforeMachine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        levelIndexOfObject = transform.tag[transform.tag.Length - 1] - '0';
        gridIndexNumberOfObject = transform.parent.tag[transform.parent.tag.Length - 1] - '0';
        _moneySound = gameObject.GetComponent<AudioSource>();
        _counterText = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            
            isAtBank = true;
            _firstStep = gameObject.transform.parent.GetChild(0).position;
            Debug.Log("Massage: " + gameObject.transform.parent.GetChild(0).name);
            Debug.Log(("FirstStep Position: " + gameObject.transform.parent.GetChild(0).position));
        }
    }
    
    public IEnumerator WaitAndPrint()
    {
        for (int i = 0; i < 5; i++)
        {
            _moneySound.Play();
            _counterText.text = (i+1) + "/5";
            yield return new WaitForSeconds(countWaitTime);
            if (i == 4)
            {
                GameObject moneyTemp = Instantiate(_moneyPrefab, gameObject.transform);
                isFinishedCount = true;
                Spawner.Instance.gridArrayStack.Push(gridIndexNumberOfObject);
                Spawner.Instance.LookForEmptyMachine();
                MoneyMove.Instance.MoneyMoveTruck();
            }
        }
        Debug.Log(waitTime);
    }
}