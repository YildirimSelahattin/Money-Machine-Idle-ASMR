using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;
    [SerializeField] private GameObject ButtonPanel;
    public GameObject MoneyFromSellText;
    public GameObject TotalMoneyText;

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

        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("TotalMoney", 0).ToString();

        ButtonPanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level - " + GameDataManager.Instance.beltSpeedButtonLevel;
        ButtonPanel.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.beltSpeedButtonMoney) + " $";
        
        ButtonPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "Level - " + GameDataManager.Instance.incomeButtonLevel;
        ButtonPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.incomeButtonMoney) + " $";
        
        ButtonPanel.transform.GetChild(2).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
            "Level - " + GameDataManager.Instance.workerSpeedButtonLevel;
        ButtonPanel.transform.GetChild(2).transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.workerSpeedButtonMoney) + " $";
        
        ButtonPanel.transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "Level - " + GameDataManager.Instance.addMachineButtonLevel;
        ButtonPanel.transform.GetChild(3).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.addMachineButtonMoney) + " $";
    }

    public void OnSellButton()
    {
        Debug.Log("Sell");
        GameDataManager.Instance.totalMoney += GameDataManager.Instance.moneyToBeCollected;
        GameDataManager.Instance.moneyToBeCollected = 0;
        MoneyFromSellText.GetComponent<TextMeshProUGUI>().text = "0";
        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.totalMoney);
        //Truck Move
        PickupManager.Instance.SellMoneyWithTruck();
        GameDataManager.Instance.SaveData();
    }
    
    public void OnBeltSpeedUpgradeButton()
    {
        if (GameDataManager.Instance.totalMoney >= GameDataManager.Instance.beltSpeedButtonMoney)
        {
            GameDataManager.Instance.totalMoney -= GameDataManager.Instance.beltSpeedButtonMoney;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.totalMoney);
            
            GameDataManager.Instance.beltSpeedButtonMoney += GameDataManager.Instance.beltSpeedButtonMoney / 1.5f;
            GameDataManager.Instance.beltSpeedButtonLevel++;

            ButtonPanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level - " + GameDataManager.Instance.beltSpeedButtonLevel;
            ButtonPanel.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.beltSpeedButtonMoney) + " $";

            if(GameDataManager.Instance.beltSpeed > 0.1f)
                GameDataManager.Instance.beltSpeed -= (GameDataManager.Instance.beltSpeed * 0.03f);
        
            GameDataManager.Instance.SaveData();
        }
    }

    public void OnIncomeUpgradeButton()
    {
        if (GameDataManager.Instance.totalMoney >= GameDataManager.Instance.incomeButtonMoney)
        {
            GameDataManager.Instance.totalMoney -= GameDataManager.Instance.incomeButtonMoney;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.totalMoney);
            
            GameDataManager.Instance.incomeButtonMoney += GameDataManager.Instance.incomeButtonMoney / 2;
            GameDataManager.Instance.incomeButtonLevel++;

            ButtonPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.incomeButtonLevel;
            ButtonPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.incomeButtonMoney) + " $";

            MachineManager.Instance.machineIncomeMoney += MachineManager.Instance.machineIncomeMoney * 0.02f;

            GameDataManager.Instance.SaveData();
        }
    }

    public void OnWorkerUpgradeButton()
    {
        if (GameDataManager.Instance.totalMoney >= GameDataManager.Instance.workerSpeedButtonMoney)
        {
            GameDataManager.Instance.totalMoney -= GameDataManager.Instance.workerSpeedButtonMoney;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.totalMoney);
            
            GameDataManager.Instance.workerSpeedButtonMoney += GameDataManager.Instance.workerSpeedButtonMoney / 2;
            GameDataManager.Instance.workerSpeedButtonLevel++;

            GameDataManager.Instance.workerBaseSpeed += GameDataManager.Instance.workerBaseSpeed * 0.03f;

            ButtonPanel.transform.GetChild(2).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.workerSpeedButtonLevel;
            ButtonPanel.transform.GetChild(2).transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.workerSpeedButtonMoney) + " $";

            GameDataManager.Instance.SaveData();
        }
    }

    public void OnAddMachineButton()
    {
        if (GameDataManager.Instance.totalMoney >= GameDataManager.Instance.addMachineButtonMoney)
        {
            GameDataManager.Instance.totalMoney -= GameDataManager.Instance.addMachineButtonMoney;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.totalMoney);
            
            GameDataManager.Instance.addMachineButtonMoney += GameDataManager.Instance.addMachineButtonMoney / 2;
            GameDataManager.Instance.addMachineButtonLevel++;

            ButtonPanel.transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.addMachineButtonLevel;
            ButtonPanel.transform.GetChild(3).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.addMachineButtonMoney) + " $";

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
                            GameDataManager.Instance.moneyMachineArray[
                                GameDataManager.Instance.maxLevelMachineAmount + 1],
                            GameManager.Instance.gridParent.transform.GetChild(gridIndex).transform);
                    GameDataManager.Instance.gridArray[gridIndex] = 1;

                    //Instantiate worker and add to stack
                    StartCoroutine(Spawner.Instance.AddWorkerAfterDelay(gridIndex, 1));
                    break;
                }
            }
            GameDataManager.Instance.SaveData();
        }
    }
}