using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using RengeGames.HealthBars;
using Unity.VisualScripting;
using static System.Net.Mime.MediaTypeNames;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;
    public int levelIndexOfObject;
    public int gridIndexNumberOfObject;
    public bool dropped = true;
    public bool inMergeArea = false;
    public bool isWorking = false;
    public float countWaitTime;
    public float machineIncomeMoney;
    private AudioSource _moneySound;
    private TMP_Text _counterText;
    private TMP_Text _tempText;
    public bool inSnapArea = false;
    public bool isAtBank = false;
    [SerializeField] private GameObject _moneyPrefab;
    public bool isFinishedCount = false;
    public Vector3 _firstStep;
    public float waitTime;
    public GameObject comingWorkerObject;
    public Vector3 firstPos;
    private float RadialSegmentNumber = 10;
    public RadialSegmentedHealthBar RadialSegment;
    [SerializeField] GameObject parentOfMoney;
    [SerializeField] Transform stopPosForMachineMoneys;
    [SerializeField] Transform firstPosForMachineMoneys;
    public static float x, y, z;
    public Material moneyMatAfterCount;
    public Material baseMoneyMat;
    public int moneyPileNumber;
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
        _firstStep = gameObject.transform.parent.GetChild(0).position;
        _tempText = UIManager.Instance.MoneyFromSellText.GetComponent<TextMeshProUGUI>();
    }

    public void MoneyMove()
    {

        Vector3 myPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(1)
        .transform.position;
        myPos.y += 0.6f;
        firstPos.x -= 1f;
        GameObject movingMoneyParent = Instantiate(_moneyPrefab, myPos, _moneyPrefab.transform.rotation);

        movingMoneyParent.GetComponent<MoneyMove>().MoveMoney(gridIndexNumberOfObject,firstPos,machineIncomeMoney,_tempText,movingMoneyParent);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            isAtBank = true;
            Debug.Log("* * * < Geldim > * * *");
        }
    }
    
    public IEnumerator WaitAndPrint()
    {
        parentOfMoney.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            if (GameDataManager.Instance.playSound == 1)
            {
                _moneySound.Play();

            }

            if (levelIndexOfObject != 3)
            {
                MoveMoneyInRoundedMachine(parentOfMoney.transform.GetChild(i).gameObject, i, countWaitTime);
            }
            else
            {
                MoveMoneyInNormalMachine(parentOfMoney.transform.GetChild(i).gameObject, i, countWaitTime);
            }

            RadialSegmentNumber--;
            RadialSegment.SetRemovedSegments(RadialSegmentNumber);
            yield return new WaitForSeconds(countWaitTime);
            if (i == 9)
            {
                RadialSegmentNumber = 10;
                GameDataManager.Instance.SaveData();
                MoneyMove();
                isFinishedCount = true;
            }
        }

    }

    public void MoveMoneyInRoundedMachine(GameObject moneyToMove, float zOffset, float waitAmount)
    {
        moneyToMove.transform.DORotate(new Vector3(90, 180, 90), waitAmount * 25 / 100);
        moneyToMove.transform.DOLocalMove(firstPosForMachineMoneys.localPosition, waitAmount * 25 / 100).OnComplete(() =>
        {
            moneyToMove.GetComponent<MeshRenderer>().material = moneyMatAfterCount;
            moneyToMove.transform.DORotate(new Vector3(0, 180, 90), waitAmount * 75 / 100);
            moneyToMove.transform.DOLocalMove(new Vector3(stopPosForMachineMoneys.localPosition.x, stopPosForMachineMoneys.localPosition.y, stopPosForMachineMoneys.localPosition.z + (0.2f * zOffset)), waitAmount * 75 / 100).OnComplete(() =>
            {
                if (zOffset == 9)
                {
                    ResetMachineMoneyPositions();
                }
            });
        });

    }
    public void MoveMoneyInNormalMachine(GameObject moneyToMove, float zOffset, float waitAmount)
    {
        moneyToMove.transform.DOLocalMove(firstPosForMachineMoneys.localPosition, waitAmount * 25 / 100).OnComplete(() =>
        {
            moneyToMove.GetComponent<MeshRenderer>().material = moneyMatAfterCount;
            moneyToMove.transform.DOLocalMove(new Vector3(stopPosForMachineMoneys.localPosition.x, stopPosForMachineMoneys.localPosition.y, stopPosForMachineMoneys.localPosition.z + (0.2f * zOffset)), waitAmount * 75 / 100).OnComplete(() =>
            {
                if (zOffset == 9)
                {
                    ResetMachineMoneyPositions();
                }
            });
        });
    }
    public void ResetMachineMoneyPositions()
    {
        for (int i = 0; i < 10; i++)
        {
            parentOfMoney.transform.GetChild(i).transform.localPosition = new Vector3(0, 0, (float)i / 10f);
            parentOfMoney.SetActive(false);//disable money bale 
            parentOfMoney.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = baseMoneyMat;
        }
    }
}