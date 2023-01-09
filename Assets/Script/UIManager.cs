using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using RengeGames.HealthBars;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;
    public bool isSell = false;
    [SerializeField] private GameObject ButtonPanel;
    public GameObject MoneyFromSellText;
    public GameObject TotalMoneyText;
    public GameObject Grid;
    public GameObject beltSpeedButton;
    public GameObject incomeButton;
    public GameObject workerSpeedButton;
    public GameObject addMachineButton;
    
    [SerializeField] GameObject soundOn;
    [SerializeField] GameObject soundOff;
    [SerializeField] GameObject musicOn;
    [SerializeField] GameObject musicOff;
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
        beltSpeedButton = ButtonPanel.transform.GetChild(0).gameObject;
        incomeButton = ButtonPanel.transform.GetChild(1).gameObject;
        workerSpeedButton = ButtonPanel.transform.GetChild(2).gameObject;
        addMachineButton = ButtonPanel.transform.GetChild(3).gameObject;
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
        isSell = true;
        GameDataManager.Instance.TotalMoney += GameDataManager.Instance.moneyToBeCollected;
        GameDataManager.Instance.moneyToBeCollected = 0;
        MoneyFromSellText.GetComponent<TextMeshProUGUI>().text = "0";
        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);
        //Truck Move
        PickupManager.Instance.SellMoneyWithTruck();
        GameDataManager.Instance.SaveData();
        MachineManager.Instance.x = -0.4f;
        MachineManager.Instance.y = 0.5f;
        MachineManager.Instance.z = 0.25f;

       
    }
    
    public void OnBeltSpeedUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.beltSpeedButtonMoney)
        {
            float moneyToDecrease = GameDataManager.Instance.beltSpeedButtonMoney;

            GameDataManager.Instance.beltSpeedButtonMoney += GameDataManager.Instance.beltSpeedButtonMoney / 1.5f;
            GameDataManager.Instance.beltSpeedButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            ButtonPanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level - " + GameDataManager.Instance.beltSpeedButtonLevel;
            ButtonPanel.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.beltSpeedButtonMoney) + " $";
            
            GameDataManager.Instance.beltSpeed += (GameDataManager.Instance.beltSpeed * 0.03f);
        
            GameDataManager.Instance.SaveData();
        }
    }

    public void OnIncomeUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.incomeButtonMoney)
        {
            float moneyToDecrrease = GameDataManager.Instance.incomeButtonMoney;
            GameDataManager.Instance.incomeButtonMoney += GameDataManager.Instance.incomeButtonMoney / 2;
            GameDataManager.Instance.offlineProgressNum += GameDataManager.Instance.offlineProgressNum / 5;
            GameDataManager.Instance.incomeButtonLevel++;
            GameDataManager.Instance.incomePerTap++;
            GameDataManager.Instance.TotalMoney -=moneyToDecrrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

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
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.workerSpeedButtonMoney)
        {
            float moneyToDecrease = GameDataManager.Instance.workerSpeedButtonMoney;
            GameDataManager.Instance.workerSpeedButtonMoney += GameDataManager.Instance.workerSpeedButtonMoney / 2;
            GameDataManager.Instance.workerSpeedButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

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
        
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.addMachineButtonMoney)
        {
            bool controllForButtonInteract = false;
            bool closeInteractibility = true;
            float moneyToDecrease = GameDataManager.Instance.addMachineButtonMoney;
            GameDataManager.Instance.addMachineButtonMoney += GameDataManager.Instance.addMachineButtonMoney / 2;
            GameDataManager.Instance.addMachineButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            ButtonPanel.transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.addMachineButtonLevel;
            ButtonPanel.transform.GetChild(3).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.addMachineButtonMoney) + " $";

            for (int gridIndex = 0; gridIndex < GameDataManager.Instance.gridArray.Length; gridIndex++)
            {
                int valueOfGrid = GameDataManager.Instance.gridArray[gridIndex];
                if (valueOfGrid == 0) //found a position that has no machines
                {
                    if (controllForButtonInteract == false)
                    {
                        Debug.Log("qwe" + gridIndex);
                        //level 1 şu an veriliyor !!sadece
                        GameManager.Instance.gridParent.transform.GetChild(gridIndex).gameObject.GetComponent<BoxCollider>()
                            .enabled = false;
                            Instantiate(
                                GameDataManager.Instance.moneyMachineArray[
                                    GameDataManager.Instance.maxLevelMachineAmount + 1],
                                GameManager.Instance.gridParent.transform.GetChild(gridIndex).transform);
                        GameDataManager.Instance.gridArray[gridIndex] = 1;

                        //Instantiate worker and add to stack
                        StartCoroutine(Spawner.Instance.AddWorkerAfterDelay(gridIndex, 1));
                        controllForButtonInteract = true;
                    }
                    else
                    {
                        closeInteractibility = false;
                    }
                }
            }

            if(closeInteractibility == true)//CLOSEINTERACT
            {

            }
            GameDataManager.Instance.SaveData();
        }
    }

    public IEnumerator DeactivateForSeconds(Button button,float waitTime)
    {
        button.interactable = false;
        yield return new WaitForSeconds(waitTime);
        button.interactable = true;
    }
}