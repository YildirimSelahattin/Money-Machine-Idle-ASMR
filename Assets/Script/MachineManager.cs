using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using RengeGames.HealthBars;

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
    public Vector3 myPos;
    public Vector3 firstPos;
    private float RadialSegmentNumber = 10;
    public RadialSegmentedHealthBar RadialSegment;
    public Transform goPos;
    
    public float x, y, z;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        x = -0.4f;
        y = 0.5f;
        z = 0.25f;
        
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

    public void MoneyMove(){
        myPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(1)
                    .transform.position;
                myPos.y += 0.6f;
                firstPos.x -= 1f;

                GameObject moneyTemp = Instantiate(_moneyPrefab, myPos, _moneyPrefab.transform.rotation);
                
                if (gridIndexNumberOfObject % 2 == 0)
                {
                    goPos = PickupManager.Instance.tempPickup.transform;
                    firstPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0)
                        .transform.position;
                    firstPos.y += 0.6f;

                    moneyTemp.transform.DOMove(firstPos, 1f).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            float dist = Vector3.Distance (firstPos,  new Vector3(-7f, 1.1f, -20f));
                            float speed = GameDataManager.Instance.beltSpeed;
                            moneyTemp.transform.DOMove(new Vector3(-7.4f, 2f, -21f), (dist/speed)*Time.deltaTime).SetEase(Ease.Linear)
                                .OnComplete(
                                    () =>
                                    {
                                        if (x <= 0.4 && z >= -0.25)
                                        {
                                            moneyTemp.transform.SetParent(goPos.transform);

                                            moneyTemp.transform.DOLocalMove(new Vector3(x, y, z), 2f).OnComplete(() =>
                                            {
                                                moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.5f)
                                                    .OnComplete(() => { Debug.Log("x ++"); x += 0.1f; });
                                            });
                                        }

                                        else if (x > 0.4)
                                        {
                                            x = -0.4f;
                                            z -= 0.25f;

                                            moneyTemp.transform.SetParent(goPos.transform);
                                            moneyTemp.transform.DOLocalMove(new Vector3(x, y, z), 2f).OnComplete(() =>
                                            {
                                                moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.5f)
                                                    .OnComplete(() => { x += 0.1f; });
                                            });
                                        }
                                        else if (z < -0.25 && x >0.4)
                                        {
                                            x = -0.4f;
                                            z = 0.25f;
                                            y += 1f;
                                            moneyTemp.transform.SetParent(goPos.transform);
                                            moneyTemp.transform.DOLocalMove(new Vector3(x, y, z), 2f).OnComplete(() =>
                                            {
                                                moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.5f)
                                                    .OnComplete(() => { x += 0.1f; });
                                            });
                                        }
                                    });
                        });
                }
                else
                {
                    goPos = PickupManager.Instance.tempPickup.transform;
                    firstPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0)
                        .transform.position;
                    firstPos.y += 0.6f;
                    firstPos.x -= 1f;
                    moneyTemp.transform.DOMove(firstPos, 1f).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            float dist = Vector3.Distance (firstPos,  new Vector3(7f, 1.1f, -20f));
                            float speed = GameDataManager.Instance.beltSpeed;
                            //doKill
                            if (UIManager.Instance.isSell == true) 
                                Debug.Log("Sell");
                            moneyTemp.transform.DOMove(new Vector3(6.5f, 2f, -20f), (dist/speed)*Time.deltaTime).SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    if (x <= 0.4 && z >= -0.25)
                                        {
                                            moneyTemp.transform.SetParent(goPos.transform);

                                            moneyTemp.transform.DOLocalMove(new Vector3(x, y, z), 2f).OnComplete(() =>
                                            {
                                                moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.5f)
                                                    .OnComplete(() => { x += 0.1f; });
                                            });
                                        }

                                        if (x > 0.4)
                                        {
                                            x = -0.4f;
                                            z -= 0.25f;

                                            moneyTemp.transform.SetParent(goPos.transform);
                                            moneyTemp.transform.DOLocalMove(new Vector3(x, y, z), 2f).OnComplete(() =>
                                            {
                                                moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.5f)
                                                    .OnComplete(() => { x += 0.1f; });
                                            });
                                        }
                                        else if (z < -0.25)
                                        {
                                            x = -0.4f;
                                            z = 0.25f;
                                            y += 1f;
                                            moneyTemp.transform.SetParent(goPos.transform);
                                            moneyTemp.transform.DOLocalMove(new Vector3(x, y, z), 2f).OnComplete(() =>
                                            {
                                                moneyTemp.transform.DORotate(new Vector3(-90f, 0, 0), 0.5f)
                                                    .OnComplete(() => { x += 0.1f; });
                                            });
                                        }
                                });
                            ;
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

    public IEnumerator WaitAndPrint()
    {
        Debug.Log("aui");
        for (int i = 0; i < 10; i++)
        {
            _moneySound.Play();
            _counterText.text = (i + 1) + "/5";
            RadialSegmentNumber--;
            Debug.Log(RadialSegmentNumber);
            RadialSegment.SetRemovedSegments(RadialSegmentNumber);
            Debug.Log(RadialSegment.name);
            yield return new WaitForSeconds(countWaitTime);
            if (i == 9)
            {
                RadialSegmentNumber = 10;
                GameDataManager.Instance.moneyToBeCollected += machineIncomeMoney;
                _tempText.text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.moneyToBeCollected);
                GameDataManager.Instance.SaveData();
                MoneyMove();

                isFinishedCount = true;
            }
        }
        _counterText.text = "waiting";
    }
}