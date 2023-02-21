using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class MoneyMove : MonoBehaviour
{
    public static MoneyMove Instance;

    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void MoveMoney(int gridIndexNumberOfObject, Vector3 firstPos,float machineIncomeMoney, TMP_Text _tempText,GameObject movingMoneyParent)
    {


        Spawner.Instance.movingMoneyBaleList.Add(movingMoneyParent);
        if (gridIndexNumberOfObject % 2 == 0) // if money is on the left side
        {
            firstPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0)
                .transform.position;
            firstPos.y += 0.6f;
            movingMoneyParent.transform.DOMove(firstPos, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    float dist = Vector3.Distance(firstPos, new Vector3(-7f, 1.1f, -20f));
                    float speed = GameDataManager.Instance.beltSpeed;
                    movingMoneyParent.transform.DOMove(new Vector3(-7.4f, 2f, -20f), (dist / -speed) * Time.deltaTime)
                        .SetEase(Ease.Linear)
                        .OnComplete(
                            () =>
                            {
                                StartCoroutine(movingMoneyParent.GetComponent<MoneyMove>()
                                    .LastMoveToTruck(movingMoneyParent, machineIncomeMoney, _tempText));
                            });
                });
        }
        else // if money is on the right side 
        {
            firstPos = GameManager.Instance.gridParent.transform.GetChild(gridIndexNumberOfObject).GetChild(0)
                .transform.position;
            firstPos.y += 0.6f;
            firstPos.x -= 1f;
            movingMoneyParent.transform.DOMove(firstPos, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    float dist = Vector3.Distance(firstPos, new Vector3(7f, 1.1f, -20f));
                    float speed = GameDataManager.Instance.beltSpeed;
                    //doKill
                    if (UIManager.Instance.isSell == true)
                        Debug.Log("Sell");
                    movingMoneyParent.transform.DOMove(new Vector3(6.5f, 2f, -20f), (dist / -speed) * Time.deltaTime)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            Debug.Log("sdasdasdasdsdasdsdasdsdasdsdasdsdasdd");
                            StartCoroutine(movingMoneyParent.GetComponent<MoneyMove>()
                                .LastMoveToTruck(movingMoneyParent, machineIncomeMoney, _tempText));
                        });
                });
        }
    }

    public IEnumerator LastMoveToTruck(GameObject moneyParent, float machineIncomeMoney, TMP_Text _tempText)
    {
        
        Debug.Log(("Girdimmmmmmm"));
        int _incomeLevel = PlayerPrefs.GetInt("IncomeButtonLevel", 0);
        Transform goPos = PickupManager.Instance.tempPickup.transform;
        Spawner.Instance.movingMoneyBaleList.Remove(moneyParent);
        
        for (int i = moneyParent.transform.childCount-1;i>=0;i--)
        {
            yield return new WaitForSeconds(0.2F);
            Vector3 posToMoveForThisMoney = new Vector3(MachineManager.x, MachineManager.y, MachineManager.z);
            Debug.Log("sa" + posToMoveForThisMoney);
            GameObject temp = moneyParent.transform.GetChild(i).gameObject;
            temp.transform.parent = goPos.transform;
            temp.transform.DORotate(new Vector3(-90f, 0, 180), 0.2f);
            temp.transform.DOLocalJump(posToMoveForThisMoney, 10, 1, 1.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                GameDataManager.Instance.moneyToBeCollected += AbbrevationUtility.RoundNumberLikeText((long)(machineIncomeMoney * GameDataManager.Instance.IncomePercantage));
                
                _tempText.text = AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.moneyToBeCollected);
                GameDataManager.Instance.SaveData();
            });
            MachineManager.x += 0.1f;
            if (MachineManager.x > 0.1f && MachineManager.z != -0.3f)
            {
                MachineManager.z -= 0.3f;
                MachineManager.x = -0.4f;
            }
            else if (MachineManager.x > 0.1f && MachineManager.z == -0.3f)
            {
                MachineManager.y += 0.5f;
                MachineManager.x = -0.4f;
                MachineManager.z = 0.3f;
            }
        }
        Destroy(moneyParent);
    }
}