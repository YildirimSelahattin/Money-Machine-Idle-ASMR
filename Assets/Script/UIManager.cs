using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using RengeGames.HealthBars;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;
using GoogleMobileAds.Api;

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
    public GameObject adBeltSpeedButton;
    public GameObject adIncomeButton;
    public GameObject adWorkerSpeedButton;
    public GameObject adAddMachineButton;

    public int isSoundOn;
    public int isMusicOn;
    public int isVibrateOn;
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
    bool openedOptionsPanel = false;
    public int buttonIndex = 0;
    public GameObject[] gridMoneyOpenInteractableArray;
    public GameObject[] gridMoneyOpenNotInteractableArray;
    public GameObject[] gridAddArray;
    public GameObject tappingHand;
    public GameObject MergeHand;
    public int addMachineTapAmount;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        addMachineTapAmount = PlayerPrefs.GetInt("addMachineAmount", 0);
        if (addMachineTapAmount == 2)
        {
            MergeHand.SetActive(true);
        }
        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = GameDataManager.Instance.TotalMoney.ToString();
        beltSpeedButton = ButtonPanel.transform.GetChild(0).transform.GetChild(0).gameObject;
        incomeButton = ButtonPanel.transform.GetChild(0).transform.GetChild(1).gameObject;
        workerSpeedButton = ButtonPanel.transform.GetChild(0).transform.GetChild(2).gameObject;
        addMachineButton = ButtonPanel.transform.GetChild(0).transform.GetChild(3).gameObject;

        StartCoroutine(AdButtonsDelay());

        beltSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
        beltSpeedButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.BeltSpeedButtonMoney) + " $";

        incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
        incomeButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.IncomeButtonMoney) + " $";

        workerSpeedButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
        workerSpeedButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.WorkerSpeedButtonMoney) + " $";

        addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
        addMachineButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.AddMachineButtonMoney) + " $";

        GameDataManager.Instance.ControlButtons();
        StartCoroutine(OpeningAdButtonsAfterDelay(60));
    }

    IEnumerator AdButtonsDelay()
    {
        yield return new WaitForEndOfFrame();
        adAddMachineButton.SetActive(false);
        adIncomeButton.SetActive(false);
        adBeltSpeedButton.SetActive(false);
        adWorkerSpeedButton.SetActive(false);
        ButtonPanel.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = false;
    }

    public void OnSellButton()
    {
        Debug.Log("Sell");
        isSell = true;
        GameDataManager.Instance.TotalMoney += GameDataManager.Instance.moneyToBeCollected;
        GameDataManager.Instance.moneyToBeCollected = 0;
        MoneyFromSellText.GetComponent<TextMeshProUGUI>().text = "0";
        TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);
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

            GameDataManager.Instance.BeltSpeedButtonMoney += GameDataManager.Instance.GetOnly1DigitAfterPoint( GameDataManager.Instance.BeltSpeedButtonMoney / 1.5f);
            GameDataManager.Instance.beltSpeedButtonLevel++;

            /*if (GameDataManager.Instance.beltSpeedButtonLevel % 3 == 0)
            {
                adBeltSpeedButton.SetActive(true);
                beltSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
                StartCoroutine(AdBeltButtonsDelay(10));
            }*/

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            beltSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
            beltSpeedButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.BeltSpeedButtonMoney) + " $";

            GameDataManager.Instance.beltSpeed += (GameDataManager.Instance.beltSpeed * 0.04f);

            GameDataManager.Instance.SaveData();
        }
    }

    public void AdOnBeltSpeedUpgradeButton()
    {
        buttonIndex = 1;
        RewardedAdManager.Instance.UpgradeButtonRewardAd();
    }

    public void RewardedBeltSpeedUpgradeButton()
    {
        GameDataManager.Instance.BeltSpeedButtonMoney += GameDataManager.Instance.GetOnly1DigitAfterPoint( GameDataManager.Instance.BeltSpeedButtonMoney / 1.5f);
        GameDataManager.Instance.beltSpeedButtonLevel++;
        GameDataManager.Instance.beltSpeed += (GameDataManager.Instance.beltSpeed * 0.04f);
        GameDataManager.Instance.SaveData();
        
        adBeltSpeedButton.SetActive(false);
    }

    public void OnIncomeUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.IncomeButtonMoney)
        {
            float moneyToDecrrease = GameDataManager.Instance.IncomeButtonMoney;
            GameDataManager.Instance.IncomeButtonMoney += GameDataManager.Instance.GetOnly1DigitAfterPoint(GameDataManager.Instance.IncomeButtonMoney / 2);
            GameDataManager.Instance.offlineProgressNum += GameDataManager.Instance.offlineProgressNum / 5;
            GameDataManager.Instance.incomeButtonLevel++;
            
            /*if (GameDataManager.Instance.incomeButtonLevel % 3 == 0)
            {
                adIncomeButton.SetActive(true);
                incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
                StartCoroutine(AdIncomeButtonsDelay(10));
            }*/
            
            GameDataManager.Instance.IncomePerTap++;
            GameDataManager.Instance.TotalMoney -= moneyToDecrrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
            incomeButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.IncomeButtonMoney) + " $";

            MachineManager.Instance.machineIncomeMoney += MachineManager.Instance.machineIncomeMoney * 0.2f;

            GameDataManager.Instance.SaveData();
        }
    }

    public void AdOnIncomeUpgradeButton()
    {
        buttonIndex = 2;
        RewardedAdManager.Instance.UpgradeButtonRewardAd();
    }

    public void RewardedIncomeUpgradeButton()
    {
        GameDataManager.Instance.IncomeButtonMoney += GameDataManager.Instance.GetOnly1DigitAfterPoint(GameDataManager.Instance.IncomeButtonMoney / 2);
        GameDataManager.Instance.offlineProgressNum += GameDataManager.Instance.offlineProgressNum / 5;
        GameDataManager.Instance.incomeButtonLevel++;
        GameDataManager.Instance.IncomePerTap++;

        MachineManager.Instance.machineIncomeMoney += MachineManager.Instance.machineIncomeMoney * 0.2f;

        GameDataManager.Instance.SaveData();
        
        adIncomeButton.SetActive(false);
    }
    public IEnumerator OpeningAdButtonsAfterDelay(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        //for belt
        adBeltSpeedButton.SetActive(true);
        beltSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
        StartCoroutine(AdBeltButtonsDelay(10));

        //income
        adIncomeButton.SetActive(true);
        incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
        StartCoroutine(AdIncomeButtonsDelay(10));

        //worker
        adWorkerSpeedButton.SetActive(true);
        workerSpeedButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
        StartCoroutine(AdWorkerButtonsDelay(10));

        //machine button
        foreach (int gridValue in GameDataManager.Instance.gridArray)
        {
            if(gridValue == 0)
            {
                adAddMachineButton.SetActive(true);
                addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
                StartCoroutine(AdMachineButtonsDelay(10));
            }
        }

    }
    public void OnWorkerUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.WorkerSpeedButtonMoney)
        {
            float moneyToDecrease = GameDataManager.Instance.WorkerSpeedButtonMoney;
            GameDataManager.Instance.WorkerSpeedButtonMoney += GameDataManager.Instance.WorkerSpeedButtonMoney / 2;
            GameDataManager.Instance.workerSpeedButtonLevel++;

            /*if (GameDataManager.Instance.workerSpeedButtonLevel % 3 == 0)
            {
                adWorkerSpeedButton.SetActive(true);
                workerSpeedButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                    "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
                StartCoroutine(AdWorkerButtonsDelay(10));
            }*/
            
            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            GameDataManager.Instance.workerBaseSpeed += GameDataManager.Instance.workerBaseSpeed * 0.03f;

            workerSpeedButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
            workerSpeedButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.WorkerSpeedButtonMoney) + " $";

            GameDataManager.Instance.SaveData();
        }
    }

    public void AdOnWorkerUpgradeButton()
    {
        buttonIndex = 3;
        RewardedAdManager.Instance.UpgradeButtonRewardAd();
    }

    public void RewardedAdWorkerUpgradeButton()
    {
        GameDataManager.Instance.WorkerSpeedButtonMoney += GameDataManager.Instance.WorkerSpeedButtonMoney / 2;
        GameDataManager.Instance.workerSpeedButtonLevel++;
        
        GameDataManager.Instance.workerBaseSpeed += GameDataManager.Instance.workerBaseSpeed * 0.03f;

        GameDataManager.Instance.SaveData();
        
        adWorkerSpeedButton.SetActive(false);
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
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);

            addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
            addMachineButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.AddMachineButtonMoney) + " $";

            for (int gridIndex = 0; gridIndex < GameDataManager.Instance.gridArray.Length; gridIndex++)
            {
                int valueOfGrid = GameDataManager.Instance.gridArray[gridIndex];
                if (valueOfGrid == 0) //found a position that has no machines
                {
                    if (controllForButtonInteract == false)
                    {
                        if (addMachineTapAmount == 2)
                        {
                            MergeHand.SetActive(true);
                            addMachineTapAmount++;
                        }
                        PlayerPrefs.SetInt("addMachineAmount", addMachineTapAmount);
                        //level 1 şu an veriliyor !!sadece
                        GameManager.Instance.gridParent.transform.GetChild(gridIndex).gameObject
                            .GetComponent<BoxCollider>()
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

            if (closeInteractibility == true) //CLOSEINTERACT
            {
                addMachineButton.GetComponent<Button>().interactable = false;
            }
            
            /*if (GameDataManager.Instance.addMachineButtonLevel % 3 == 0 && closeInteractibility == false)
            {
                adAddMachineButton.SetActive(true);
                addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
                StartCoroutine(AdMachineButtonsDelay(10));
            }*/
            GameDataManager.Instance.SaveData();
        }
    }

    public void AdOnAddMachineButton()
    {
        buttonIndex = 4;
        RewardedAdManager.Instance.UpgradeButtonRewardAd();
    }

    public void RewardedAddMachineButton()
    {
        bool controllForButtonInteract = false;
        bool closeInteractibility = true;

        GameDataManager.Instance.AddMachineButtonMoney += GameDataManager.Instance.GetOnly1DigitAfterPoint(GameDataManager.Instance.AddMachineButtonMoney / 2);
        GameDataManager.Instance.addMachineButtonLevel++;

        for (int gridIndex = 0; gridIndex < GameDataManager.Instance.gridArray.Length; gridIndex++)
        {
            int valueOfGrid = GameDataManager.Instance.gridArray[gridIndex];
            if (valueOfGrid == 0) //found a position that has no machines
            {
                if (controllForButtonInteract == false)
                {
                    //level 1 şu an veriliyor !!sadece
                    GameManager.Instance.gridParent.transform.GetChild(gridIndex).gameObject
                        .GetComponent<BoxCollider>()
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

        if (closeInteractibility == true) //CLOSEINTERACT
        {
            addMachineButton.GetComponent<Button>().interactable = false;
        }

        GameDataManager.Instance.SaveData();
        
        adAddMachineButton.SetActive(false);
    }
    public void OpenGridAdButtons()
    {
        foreach (GameObject adButton in gridAddArray)
        {
            adButton.SetActive(true);
        }
    }

    public void CloseGridAdButtons()
    {
        foreach (GameObject adButton in gridAddArray)
        {
            adButton.SetActive(false);
        }
    }
    public IEnumerator DeactivateForSeconds(Button button, float waitTime)
    {
        button.interactable = false;
        yield return new WaitForSeconds(waitTime);
        button.interactable = true;
    }
    
    public IEnumerator AdBeltButtonsDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        adBeltSpeedButton.SetActive(false);
    }
    public IEnumerator AdIncomeButtonsDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        adIncomeButton.SetActive(false);
    }
    public IEnumerator AdWorkerButtonsDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        adWorkerSpeedButton.SetActive(false);
    }
    public IEnumerator AdMachineButtonsDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        adAddMachineButton.SetActive(false);
    }

    public void UpdateSound()
    {
        isSoundOn = GameDataManager.Instance.playSound;
        if (isSoundOn == 0)
        {
            soundOff.gameObject.SetActive(true);
            SoundsOff();
        }

        if (isSoundOn == 1)
        {
            soundOn.gameObject.SetActive(true);
            SoundsOn();
        }
    }

    public void UpdateMusic()
    {
        isMusicOn = GameDataManager.Instance.playMusic;
        if (isMusicOn == 0)
        {
            musicOff.gameObject.SetActive(true);
            MusicOff();
        }

        if (isMusicOn == 1)
        {
            musicOn.gameObject.SetActive(true);
            MusicOn();
        }
    }

    public void UpdateVibrate()
    {
        isSoundOn = GameDataManager.Instance.playSound;
        if (isVibrateOn == 0)
        {
            vibrationOff.gameObject.SetActive(true);
            SoundsOff();
        }

        if (isVibrateOn == 1)
        {
            vibrationOn.gameObject.SetActive(true);
            SoundsOn();
        }
    }

    public void MusicOff()
    {
        GameDataManager.Instance.playMusic = 0;
        musicOn.gameObject.SetActive(false);
        musicOff.gameObject.SetActive(true);
    }

    public void MusicOn()
    {
        GameDataManager.Instance.playMusic = 1;
        musicOff.gameObject.SetActive(false);
        musicOn.gameObject.SetActive(true);
    }

    public void SoundsOff()
    {
        GameDataManager.Instance.playSound = 0;
        soundOn.gameObject.SetActive(false);
        soundOff.gameObject.SetActive(true);
    }

    public void SoundsOn()
    {
        GameDataManager.Instance.playSound = 1;
        soundOff.gameObject.SetActive(false);
        soundOn.gameObject.SetActive(true);
    }

    public void VibrationOff()
    {
        GameDataManager.Instance.playSound = 0;
        vibrationOn.gameObject.SetActive(false);
        vibrationOff.gameObject.SetActive(true);
    }

    public void VibrationOn()
    {
        GameDataManager.Instance.playSound = 1;
        vibrationOff.gameObject.SetActive(false);
        vibrationOn.gameObject.SetActive(true);
    }

    public void OnOpenOptionsPanel()
    {
        if(openedOptionsPanel == false)
        {
            OptionsPanel.SetActive(true);
            openedOptionsPanel = true;
        }
        else
        {
            OptionsPanel.SetActive(false);
            openedOptionsPanel = false;
        }
    }

    public void OnOpenInfoPanel()
    {
        InfoPanel.SetActive(true);
    }

    public void OnSpace()
    {
        OptionsPanel.SetActive(false);
        InfoPanel.SetActive(false);
    }
}