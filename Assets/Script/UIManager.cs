using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using RengeGames.HealthBars;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;

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
    [SerializeField] GameObject vibrationOff;
    [SerializeField] GameObject vibrationOn;
    [SerializeField] private GameObject OptionsButton;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject InfoButton;
    [SerializeField] private GameObject InfoPanel;
    
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = GameDataManager.Instance.TotalMoney.ToString();
        beltSpeedButton = ButtonPanel.transform.GetChild(0).gameObject;
        incomeButton = ButtonPanel.transform.GetChild(1).gameObject;
        workerSpeedButton = ButtonPanel.transform.GetChild(2).gameObject;
        addMachineButton = ButtonPanel.transform.GetChild(3).gameObject;
        ButtonPanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level - " + GameDataManager.Instance.beltSpeedButtonLevel;
        
        ButtonPanel.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.BeltSpeedButtonMoney) + " $";
        Debug.Log("belt"+ GameDataManager.Instance.BeltSpeedButtonMoney);
        
        ButtonPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "Level - " + GameDataManager.Instance.incomeButtonLevel;
        ButtonPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.IncomeButtonMoney) + " $";
        
        ButtonPanel.transform.GetChild(2).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
            "Level - " + GameDataManager.Instance.workerSpeedButtonLevel;
        ButtonPanel.transform.GetChild(2).transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.WorkerSpeedButtonMoney) + " $";
        
        ButtonPanel.transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "Level - " + GameDataManager.Instance.addMachineButtonLevel;
        ButtonPanel.transform.GetChild(3).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.AddMachineButtonMoney) + " $";
    }

    public void OnSellButton()
    {
        Debug.Log("Sell");
        isSell = true;
        GameDataManager.Instance.TotalMoney += GameDataManager.Instance.moneyToBeCollected;
        GameDataManager.Instance.moneyToBeCollected = 0;
        MoneyFromSellText.GetComponent<TextMeshProUGUI>().text = "0";
        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);
        //pauseMoneyBales
        foreach (GameObject moneyBale in Spawner.Instance.movingMoneyBaleList)
        {
            moneyBale.transform.DOPause();
        }
        MachineManager.x = -0.4f;
        MachineManager.y = 0.5f;
        MachineManager.z = 0.25f;
        //Truck Move
        PickupManager.Instance.SellMoneyWithTruck();
        GameDataManager.Instance.SaveData();
    }
    
    public void OnBeltSpeedUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.BeltSpeedButtonMoney)
        {
            float moneyToDecrease = GameDataManager.Instance.BeltSpeedButtonMoney;

            GameDataManager.Instance.BeltSpeedButtonMoney += GameDataManager.Instance.BeltSpeedButtonMoney / 1.5f;
            GameDataManager.Instance.beltSpeedButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            ButtonPanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level - " + GameDataManager.Instance.beltSpeedButtonLevel;
            ButtonPanel.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.BeltSpeedButtonMoney) + " $";
            
            GameDataManager.Instance.beltSpeed += (GameDataManager.Instance.beltSpeed * 0.03f);
        
            GameDataManager.Instance.SaveData();
        }
    }

    public void OnIncomeUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.IncomeButtonMoney)
        {
            float moneyToDecrrease = GameDataManager.Instance.IncomeButtonMoney;
            GameDataManager.Instance.IncomeButtonMoney += GameDataManager.Instance.IncomeButtonMoney / 2;
            GameDataManager.Instance.offlineProgressNum += GameDataManager.Instance.offlineProgressNum / 5;
            GameDataManager.Instance.incomeButtonLevel++;
            GameDataManager.Instance.IncomePerTap++;
            GameDataManager.Instance.TotalMoney -=moneyToDecrrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            ButtonPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.incomeButtonLevel;
            ButtonPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.IncomeButtonMoney) + " $";

            MachineManager.Instance.machineIncomeMoney += MachineManager.Instance.machineIncomeMoney * 0.02f;

            GameDataManager.Instance.SaveData();
        }
    }

    public void OnWorkerUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.WorkerSpeedButtonMoney)
        {
            float moneyToDecrease = GameDataManager.Instance.WorkerSpeedButtonMoney;
            GameDataManager.Instance.WorkerSpeedButtonMoney += GameDataManager.Instance.WorkerSpeedButtonMoney / 2;
            GameDataManager.Instance.workerSpeedButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            GameDataManager.Instance.workerBaseSpeed += GameDataManager.Instance.workerBaseSpeed * 0.03f;

            ButtonPanel.transform.GetChild(2).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.workerSpeedButtonLevel;
            ButtonPanel.transform.GetChild(2).transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.WorkerSpeedButtonMoney) + " $";

            GameDataManager.Instance.SaveData();
        }
    }

    public void OnAddMachineButton()
    {
        
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.AddMachineButtonMoney)
        {
            bool controllForButtonInteract = false;
            bool closeInteractibility = true;
            float moneyToDecrease = GameDataManager.Instance.AddMachineButtonMoney;
            GameDataManager.Instance.AddMachineButtonMoney += GameDataManager.Instance.AddMachineButtonMoney / 2;
            GameDataManager.Instance.addMachineButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            ButtonPanel.transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "Level - " + GameDataManager.Instance.addMachineButtonLevel;
            ButtonPanel.transform.GetChild(3).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.AddMachineButtonMoney) + " $";

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
                addMachineButton.GetComponent<Button>().interactable = false;
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