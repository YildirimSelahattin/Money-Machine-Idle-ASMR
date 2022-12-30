using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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
    public Vector3 _firstStep;
    public float waitTime ;
    public GameObject comingWorkerObject;
    public Vector3 myPos;
    public Vector3 firstPos;
   
    public float x,y,z;

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
        for (int i = 0; i < 5; i++)
        {
            _moneySound.Play();
            _counterText.text = (i+1) + "/5";
            yield return new WaitForSeconds(countWaitTime);
            if (i == 4)
            {
               myPos =GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(1).transform.position;
               myPos.y += 0.6f;

               
                GameObject moneyTemp = Instantiate(_moneyPrefab, myPos,_moneyPrefab.transform.rotation);
                
                

                if (gridIndexNumberOfObject % 2 == 0)
                {
                   firstPos =GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0).transform.position;
               firstPos.y += 0.6f;
               
                  moneyTemp.transform.DOMove(firstPos,1f).SetEase(Ease.Linear)
                .OnComplete(()=>{
                 moneyTemp.transform.DOMove(new Vector3(-7f,1.1f,-23f),1f).SetEase(Ease.Linear).OnComplete(
                    ()=>{ 
                        
                        if (x<=0.4&&z>=-0.25)
                        {
                            moneyTemp.transform.SetParent(GameManager.Instance.money.transform);
                            
                            moneyTemp.transform.DOLocalMove(new Vector3(x,y,z),1f).OnComplete(()=>{
                                moneyTemp.transform.DORotate(new Vector3(-90f,0,0),0.5f).OnComplete(()=>{
                                x+=0.1f;
                                });
                            });
                            
                        }
                        if (x>0.4)
                        {
                            x=-0.4f;
                            z-=0.25f;
                            
                            moneyTemp.transform.SetParent(GameManager.Instance.money.transform);
                            moneyTemp.transform.DOLocalMove(new Vector3(x,y,z),1f).OnComplete(()=>{
                                moneyTemp.transform.DORotate(new Vector3(-90f,0,0),0.5f).OnComplete(()=>{
                                x+=0.1f;
                                });
                            });
                            
                        }
                        else if(z<-0.25)
                        {
                            x=-0.4f;
                            z=0.25f;
                            y+=1f;
                            moneyTemp.transform.SetParent(GameManager.Instance.money.transform);
                            moneyTemp.transform.DOLocalMove(new Vector3(x,y,z),1f).OnComplete(()=>{
                                moneyTemp.transform.DORotate(new Vector3(-90f,0,0),0.5f).OnComplete(()=>{
                                x+=0.1f;
                                });
                            });
                        }
                        
                        
                });});
                
                }
                else{
                     firstPos =GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0).transform.position;
               firstPos.y += 0.6f;
                     moneyTemp.transform.DOMove(firstPos,3f).SetEase(Ease.Linear)
                .OnComplete(()=>{
                    moneyTemp.transform.DOMove(new Vector3(7f,1.1f,-20f),5f).SetEase(Ease.Linear).OnComplete(()=>{
                        moneyTemp.transform.DOMove(new Vector3(0.18f,1.1f,-23f),3f).SetEase(Ease.Linear);
                    });;
                });
                }
                
                isFinishedCount = true;
                Spawner.Instance.gridArrayStack.Push(gridIndexNumberOfObject);
                Spawner.Instance.LookForEmptyMachine();
            }
        }
        Debug.Log(waitTime);
    }
}