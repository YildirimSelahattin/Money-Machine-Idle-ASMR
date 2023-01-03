using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;

    public int NumberOfDiamonds
    {
        get { return PlayerPrefs.GetInt("NumberOfDiamondsKey", 0); } // get method
        set
        {
            PlayerPrefs.SetInt("NumberOfDiamondsKey", value);
            //diamondNumberText.text = value.ToString();
        }
    } // set method

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //make a level array that contains every machine at the index of level number
    }

    public void OnSellButton()
    {
        GameDataManager.Instance.totalMoney += GameDataManager.Instance.moneyToBeCollected;
        GameDataManager.Instance.moneyToBeCollected = 0;
        //Truck Move
        
    }

   

    public void OnBeltSpeedUpgradeButton()
    {
        GameDataManager.Instance.beltSpeedButtonMoney += GameDataManager.Instance.beltSpeedButtonMoney / 1.5f;
        GameDataManager.Instance.beltSpeedButtonLevel++;

        if(GameDataManager.Instance.beltSpeed > 0.1f)
            GameDataManager.Instance.beltSpeed -= (GameDataManager.Instance.beltSpeed * 0.03f);
    }

    public void OnIncomeUpgradeButton()
    {
        GameDataManager.Instance.incomeButtonMoney += GameDataManager.Instance.incomeButtonMoney / 2;
        GameDataManager.Instance.incomeButtonLevel++;

        MachineManager.Instance.machineIncomeMoney += MachineManager.Instance.machineIncomeMoney * 0.02f;
    }

    public void OnWorkerUpgradeButton()
    {
        GameDataManager.Instance.workerSpeedButtonMoney += GameDataManager.Instance.workerSpeedButtonMoney / 2;
        GameDataManager.Instance.workerSpeedButtonLevel++;

        GameDataManager.Instance.workerBaseSpeed -= GameDataManager.Instance.workerBaseSpeed* 0.03f;
    }

    public void OnAddMachineButton()
    {
        GameDataManager.Instance.addMachineButtonMoney += GameDataManager.Instance.addMachineButtonMoney / 2;
        GameDataManager.Instance.addMachineButtonLevel++;

        for (int gridIndex = 0; gridIndex < GameDataManager.Instance.gridArray.Length; gridIndex++)
        {
            int valueOfGrid = GameDataManager.Instance.gridArray[gridIndex];
            if (valueOfGrid == 0) //found a position that has no machines
            {
                Debug.Log("qwe" + gridIndex);
                //level 1 şu an veriliyor !!sadece
                GameManager.Instance.gridParent.transform.GetChild(gridIndex).gameObject.GetComponent<BoxCollider>()
                    .enabled = false;
                GameObject newMachine =
                    Instantiate(
                        GameDataManager.Instance.moneyMachineArray[GameDataManager.Instance.maxLevelMachineAmount + 1],
                        GameManager.Instance.gridParent.transform.GetChild(gridIndex).transform);
                GameDataManager.Instance.gridArray[gridIndex] = 1;

                //Instantiate worker and add to stack
                StartCoroutine(Spawner.Instance.AddWorkerAfterDelay(gridIndex,1));
                break;
            }
        }
        
        GameDataManager.Instance.SaveData();
    }
}