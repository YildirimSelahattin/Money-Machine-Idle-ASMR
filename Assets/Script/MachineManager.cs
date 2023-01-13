using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using RengeGames.HealthBars;
using Unity.VisualScripting;

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
    public static Transform goPos;
    [SerializeField] GameObject parentOfMoney;
    [SerializeField] Transform stopPosForMachineMoneys;
    [SerializeField] Transform firstPosForMachineMoneys;
    public static  float x, y, z;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        x = -0.4f;
        y = 0.5f;
        z = 0.30f;

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
        GameObject moneyTemp = Instantiate(_moneyPrefab, myPos, _moneyPrefab.transform.rotation);
        Spawner.Instance.movingMoneyBaleList.Add(moneyTemp);
        if (gridIndexNumberOfObject % 2 == 0) // if money is on the left side
        {
            goPos = PickupManager.Instance.tempPickup.transform;
            firstPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0)
                .transform.position;
            firstPos.y += 0.6f;
            moneyTemp.transform.DOMove(firstPos, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    float dist = Vector3.Distance(firstPos, new Vector3(-7f, 1.1f, -20f));
                    float speed = GameDataManager.Instance.beltSpeed;
                    moneyTemp.transform.DOMove(new Vector3(-7.4f, 2f, -21f), (dist / -speed)*Time.deltaTime).SetEase(Ease.Linear)
                        .OnComplete(
                            () =>
                            {
                                LastMoveToTruck(new Vector3(x, y, z), moneyTemp);
                                x += 0.10f;
                                if (x > 0.1f && z != -0.3f)
                                {
                                    z -= 0.3f;
                                    x = -0.4f;
                                }
                                else if (x > 0.1f && z == -0.3f)
                                {
                                    y += 0.5f;
                                    x = -0.4f;
                                    z = 0.3f;
                                }
                            });
                });
        }
        else // if money is on the right side 
        {
            goPos = PickupManager.Instance.tempPickup.transform;
            firstPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0)
                .transform.position;
            firstPos.y += 0.6f;
            firstPos.x -= 1f;
            moneyTemp.transform.DOMove(firstPos, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    float dist = Vector3.Distance(firstPos, new Vector3(7f, 1.1f, -20f));
                    float speed = GameDataManager.Instance.beltSpeed;
                    //doKill
                    if (UIManager.Instance.isSell == true)
                        Debug.Log("Sell");
                    moneyTemp.transform.DOMove(new Vector3(6.5f, 2f, -20f), (dist / -speed) * Time.deltaTime).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            LastMoveToTruck(new Vector3(x, y, z),moneyTemp);
                            x += 0.1f;
                            if (x > 0.1f && z!= -0.3f) 
                            {
                                z -= 0.3f;
                                x = -0.4f;
                            }
                            else if (x>0.1f && z == -0.3f) 
                            {
                                y += 0.5f;
                                x = -0.4f;
                                z = 0.3f;
                            }
                        });
                });
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            isAtBank = true;
            Debug.Log("* * * < Geldim > * * *");
        }
    }
    public void LastMoveToTruck(Vector3 posToMoveForThisMoney, GameObject moneyTemp)
    {
        Spawner.Instance.movingMoneyBaleList.Remove(moneyTemp);
        moneyTemp.transform.SetParent(goPos.transform);
        moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.2f);
        moneyTemp.transform.DOLocalJump(posToMoveForThisMoney, 10,1,1.5f).SetEase(Ease.OutBounce).OnComplete(()=>
        {
            GameDataManager.Instance.moneyToBeCollected += GameDataManager.Instance.GetOnly1DigitAfterPoint(machineIncomeMoney);
            _tempText.text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.GetOnly1DigitAfterPoint(GameDataManager.Instance.moneyToBeCollected));
            GameDataManager.Instance.SaveData();
        });
    }
    public IEnumerator WaitAndPrint()
    {
        parentOfMoney.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            _moneySound.Play();
            if (levelIndexOfObject !=3)
            {
                MoveMoneyInRoundedMachine(parentOfMoney.transform.GetChild(i).gameObject, i,countWaitTime);
            }
            else
            {
                MoveMoneyInNormalMachine(parentOfMoney.transform.GetChild(i).gameObject, i,countWaitTime);
            }
            
            RadialSegmentNumber--;
            Debug.Log(RadialSegmentNumber);
            RadialSegment.SetRemovedSegments(RadialSegmentNumber);
            Debug.Log(RadialSegment.name);
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

    public void MoveMoneyInRoundedMachine(GameObject moneyToMove,float zOffset,float waitAmount)
    {
        moneyToMove.transform.DORotate(new Vector3(90, 0, 0),waitAmount*25/100);
        moneyToMove.transform.DOLocalMove(firstPosForMachineMoneys.localPosition, waitAmount * 25 / 100).OnComplete(() =>
        {
            
            moneyToMove.transform.DORotate(new Vector3(0,0,0),waitAmount*75/100);
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
        for(int i = 0; i < 10; i++)
        {
            parentOfMoney.transform.GetChild(i).transform.localPosition = new Vector3(0,0,(float)i/10f);
            parentOfMoney.SetActive(false);//disable money bale 
        }
    }
}