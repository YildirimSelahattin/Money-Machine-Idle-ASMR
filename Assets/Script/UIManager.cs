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
using JetBrains.Annotations;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;
    public bool isSell = false;
    [SerializeField] private GameObject ButtonPanel;
    public GameObject MoneyFromSellText;
    public GameObject TotalMoneyText;
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
    public GameObject addMachineHand;
    public int addMachineTapAmount;
    public GameObject gameMusic;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateSound();
        UpdateMusic();
        UpdateVibrate();
        
        addMachineTapAmount = PlayerPrefs.GetInt("addMachineAmount", 0);
        if (addMachineTapAmount == 2)
        {
            MergeHand.SetActive(true);
        }
        if (addMachineTapAmount == 0 && GameDataManager.Instance.AddMachineButtonMoney<GameDataManager.Instance.TotalMoney)
        {
            addMachineHand.SetActive(true);
        }
        if (PlayerPrefs.GetInt("isFirstMerge", 1) == -1)
        {
            MergeHand.SetActive(false);
        }
        TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.TotalMoney); 
        beltSpeedButton = ButtonPanel.transform.GetChild(0).transform.GetChild(0).gameObject;
        incomeButton = ButtonPanel.transform.GetChild(0).transform.GetChild(1).gameObject;
        workerSpeedButton = ButtonPanel.transform.GetChild(0).transform.GetChild(2).gameObject;
        addMachineButton = ButtonPanel.transform.GetChild(0).transform.GetChild(3).gameObject;
        

        StartCoroutine(AdButtonsDelay());

        adBeltSpeedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
        beltSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
        beltSpeedButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.BeltSpeedButtonMoney) + " $";
        
        adIncomeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
        incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
        incomeButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.IncomeButtonMoney) + " $";

        adWorkerSpeedButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
        workerSpeedButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
        workerSpeedButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.WorkerSpeedButtonMoney) + " $";

        adAddMachineButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
        addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
        addMachineButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.AddMachineButtonMoney) + " $";

        GameDataManager.Instance.ControlButtons();
        StartCoroutine(OpeningAdButtonsAfterDelay(30));
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
            AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.TotalMoney);
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
            long moneyToDecrease = GameDataManager.Instance.BeltSpeedButtonMoney;
            GameDataManager.Instance.BeltSpeedButtonMoney += (long)(GameDataManager.Instance.BeltSpeedButtonMoney / 1.5f);
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
                AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.TotalMoney);

            adBeltSpeedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
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
        
        adBeltSpeedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.beltSpeedButtonLevel;
        beltSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + (GameDataManager.Instance.beltSpeedButtonLevel + 1);

        GameDataManager.Instance.SaveData();
    }

    public IEnumerator RewardedBeltSpeedUpgradeButton()
    {
        yield return new WaitForEndOfFrame();
        GameDataManager.Instance.BeltSpeedButtonMoney += (long)(GameDataManager.Instance.BeltSpeedButtonMoney / 1.5f);
        GameDataManager.Instance.beltSpeedButtonLevel++;
        GameDataManager.Instance.beltSpeed += (GameDataManager.Instance.beltSpeed * 0.04f);
        GameDataManager.Instance.SaveData();
        
        adBeltSpeedButton.SetActive(false);
    }

    public void OnIncomeUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.IncomeButtonMoney)
        {
            long moneyToDecrrease = GameDataManager.Instance.IncomeButtonMoney;
            GameDataManager.Instance.IncomeButtonMoney += GameDataManager.Instance.IncomeButtonMoney / 2;
            GameDataManager.Instance.offlineProgressNum += GameDataManager.Instance.offlineProgressNum / 5;
            GameDataManager.Instance.incomeButtonLevel++;
            
            /*if (GameDataManager.Instance.incomeButtonLevel % 3 == 0)
            {
                adIncomeButton.SetActive(true);
                incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
                StartCoroutine(AdIncomeButtonsDelay(10));
            }*/
            
            GameDataManager.Instance.TotalMoney -= moneyToDecrrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.TotalMoney);

            adIncomeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
            incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
            incomeButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.IncomeButtonMoney) + " $";

            GameDataManager.Instance.IncomePerTap += GameDataManager.Instance.IncomePerTap * .5f;

            GameDataManager.Instance.SaveData();
        }
    }

    public void AdOnIncomeUpgradeButton()
    {
        buttonIndex = 2;
        RewardedAdManager.Instance.UpgradeButtonRewardAd();
        adIncomeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.incomeButtonLevel;
        incomeButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + (GameDataManager.Instance.incomeButtonLevel + 1);
        
        GameDataManager.Instance.SaveData();
    }

    public IEnumerator RewardedIncomeUpgradeButton()
    {
        yield return new WaitForEndOfFrame();
        GameDataManager.Instance.IncomeButtonMoney += GameDataManager.Instance.IncomeButtonMoney / 2;
        GameDataManager.Instance.offlineProgressNum += GameDataManager.Instance.offlineProgressNum / 5;
        GameDataManager.Instance.incomeButtonLevel++;

        GameDataManager.Instance.IncomePercantage += GameDataManager.Instance.IncomePercantage * 0.04f;
        GameDataManager.Instance.IncomePerTap += GameDataManager.Instance.IncomePerTap * .5f;

        GameDataManager.Instance.SaveData();
        
        adIncomeButton.SetActive(false);
    }

    public void OnWorkerUpgradeButton()
    {
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.WorkerSpeedButtonMoney)
        {
            long moneyToDecrease = GameDataManager.Instance.WorkerSpeedButtonMoney;
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
                AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.TotalMoney);

            GameDataManager.Instance.workerBaseSpeed += GameDataManager.Instance.workerBaseSpeed * 0.03f;

            adWorkerSpeedButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
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
        adWorkerSpeedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.workerSpeedButtonLevel;
        workerSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + (GameDataManager.Instance.workerSpeedButtonLevel + 1);
        
        GameDataManager.Instance.SaveData();
    }

    public IEnumerator RewardedAdWorkerUpgradeButton()
    {
        yield return new WaitForEndOfFrame();
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
            int emptySpotsLeft = 0;
            int minLevelOnGrid = 6;
            bool allGridsOpened = true;
            int indexToInstantiate=0;
            int[] machineIndexArray = new int[7];
            bool isMergeable = false;
            long moneyToDecrease = GameDataManager.Instance.AddMachineButtonMoney;
            GameDataManager.Instance.AddMachineButtonMoney += GameDataManager.Instance.AddMachineButtonMoney / 2;
            GameDataManager.Instance.addMachineButtonLevel++;

            GameDataManager.Instance.TotalMoney -= moneyToDecrease;
            TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.TotalMoney);

            adAddMachineButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
            addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
            addMachineButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.AddMachineButtonMoney) + " $";
            for (int gridIndex = GameDataManager.Instance.gridArray.Length-1; gridIndex >=0; gridIndex--)
            {
                if (GameDataManager.Instance.gridArray[gridIndex] == -1)
                {
                    allGridsOpened = false;
                }
                if (GameDataManager.Instance.gridArray[gridIndex] == 0)
                {
                    emptySpotsLeft++;
                    indexToInstantiate = gridIndex;

                }
                if (GameDataManager.Instance.gridArray[gridIndex] > 0)
                {
                    if(minLevelOnGrid> GameDataManager.Instance.gridArray[gridIndex])
                    {
                        minLevelOnGrid = GameDataManager.Instance.gridArray[gridIndex];
                    }
                    machineIndexArray[GameDataManager.Instance.gridArray[gridIndex]]++;
                }
               
            }
            foreach (int numberOfMachines in machineIndexArray)
            {
                if (numberOfMachines > 2)
                {
                    isMergeable = true;
                    break;
                }
            }
            if (emptySpotsLeft == 1)
            {
                addMachineButton.GetComponent<Button>().interactable = false;
            }
            addMachineTapAmount++;
            if (addMachineTapAmount == 1)
            {
                addMachineHand.SetActive(false);
            }
            if (addMachineTapAmount == 2)
            {
                MergeHand.SetActive(true);
            }
            PlayerPrefs.SetInt("addMachineAmount", addMachineTapAmount);
            //level 1 şu an veriliyor !!sadece
            GameManager.Instance.gridParent.transform.GetChild(indexToInstantiate).gameObject
                .GetComponent<BoxCollider>()
                .enabled = false;

            if (emptySpotsLeft == 1 && allGridsOpened == true && isMergeable == false)
            {
                Instantiate(
                GameDataManager.Instance.moneyMachineArray[
                minLevelOnGrid],
                GameManager.Instance.gridParent.transform.GetChild(indexToInstantiate).transform);
                GameDataManager.Instance.gridArray[indexToInstantiate] = minLevelOnGrid;

            }
            else
            {
                Instantiate(
            GameDataManager.Instance.moneyMachineArray[
                GameDataManager.Instance.maxLevelMachineAmount + 1],
            GameManager.Instance.gridParent.transform.GetChild(indexToInstantiate).transform);
                GameDataManager.Instance.gridArray[indexToInstantiate] = GameDataManager.Instance.maxLevelMachineAmount + 1;

            }

            //Instantiate worker and add to stack
            StartCoroutine(Spawner.Instance.AddWorkerAfterDelay(indexToInstantiate, 1));
            
            GameDataManager.Instance.SaveData();
        }
    }

    public void AdOnAddMachineButton()
    {
        buttonIndex = 4;
        RewardedAdManager.Instance.UpgradeButtonRewardAd();
        adAddMachineButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + GameDataManager.Instance.addMachineButtonLevel;
        addMachineButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "LEVEL " + (GameDataManager.Instance.addMachineButtonLevel + 1);
        
        GameDataManager.Instance.SaveData();
    }

    public IEnumerator RewardedAddMachineButton()
    {
        
        yield return new WaitForEndOfFrame();
        int[] machineIndexArray = new int[7];
        bool isMergeable = false;
        int emptySpotsLeft = 0;
        int minLevelOnGrid = 6;
        bool allGridsOpened = true;
        int indexToInstantiate = 0;
        GameDataManager.Instance.AddMachineButtonMoney += GameDataManager.Instance.AddMachineButtonMoney / 2;
        GameDataManager.Instance.addMachineButtonLevel++;
        for (int gridIndex = 0; gridIndex < GameDataManager.Instance.gridArray.Length; gridIndex++)
        {
            if (GameDataManager.Instance.gridArray[gridIndex] == -1)
            {
                allGridsOpened = false;
            }
            if (GameDataManager.Instance.gridArray[gridIndex] == 0)
            {
                emptySpotsLeft++;
                indexToInstantiate = gridIndex;

            }
            if (GameDataManager.Instance.gridArray[gridIndex] > 0)
            {
                if (minLevelOnGrid > GameDataManager.Instance.gridArray[gridIndex])
                {
                    minLevelOnGrid = GameDataManager.Instance.gridArray[gridIndex];
                }
            }
            machineIndexArray[GameDataManager.Instance.gridArray[gridIndex]]++;
        }
        foreach(int numberOfMachines in machineIndexArray)
        {
            if(numberOfMachines > 2)
            {
                isMergeable = true;
                break;
            }
        }
            
            
                    //level 1 şu an veriliyor !!sadece
                    GameManager.Instance.gridParent.transform.GetChild(indexToInstantiate).gameObject
                        .GetComponent<BoxCollider>()
                        .enabled = false;
                if (emptySpotsLeft == 1 && allGridsOpened == true&&isMergeable == false)
                {
                    Instantiate(
                        GameDataManager.Instance.moneyMachineArray[
                        minLevelOnGrid],
                        GameManager.Instance.gridParent.transform.GetChild(indexToInstantiate).transform);
                        GameDataManager.Instance.gridArray[indexToInstantiate] = minLevelOnGrid;
                }
                else
                {
                    Instantiate(
                        GameDataManager.Instance.moneyMachineArray[
                        GameDataManager.Instance.maxLevelMachineAmount + 1],
                        GameManager.Instance.gridParent.transform.GetChild(indexToInstantiate).transform);
                        GameDataManager.Instance.gridArray[indexToInstantiate] = GameDataManager.Instance.maxLevelMachineAmount + 1;
                }

                //Instantiate worker and add to stack
                StartCoroutine(Spawner.Instance.AddWorkerAfterDelay(indexToInstantiate, 1));

        GameDataManager.Instance.SaveData();
        adAddMachineButton.SetActive(false);
    }
       
    
    
    public IEnumerator OpeningAdButtonsAfterDelay(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        //for belt
        adBeltSpeedButton.SetActive(true);
        adBeltSpeedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
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
                break;
            }
        }
    }
    
    public IEnumerator OpenGridAdButtons()
    {
        yield return new WaitForEndOfFrame();
        for(int i = 2; i < GameDataManager.Instance.gridArray.Length; i++)
        {
            if(GameDataManager.Instance.gridArray[i] == -1)
            {
                gridAddArray[i].SetActive(true);
                break;
            } 
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
        StartCoroutine(OpeningAdButtonsAfterDelay(30));
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
        isVibrateOn = GameDataManager.Instance.playVibrate;
        if (isVibrateOn == 0)
        {
            vibrationOff.gameObject.SetActive(true);
            VibrationOff();
        }

        if (isVibrateOn == 1)
        {
            vibrationOn.gameObject.SetActive(true);
            VibrationOn();
        }
    }

    public void MusicOff()
    {
        GameDataManager.Instance.playMusic = 0;
        gameMusic.SetActive(false);
        musicOn.gameObject.SetActive(false);
        musicOff.gameObject.SetActive(true);
        //UpdateMusic();

    }

    public void MusicOn()
    {
        GameDataManager.Instance.playMusic = 1;
        gameMusic.SetActive(true);
        musicOff.gameObject.SetActive(false);
        musicOn.gameObject.SetActive(true);
        //UpdateMusic();
    }

    public void SoundsOff()
    {
        GameDataManager.Instance.playSound = 0;
        soundOn.gameObject.SetActive(false);
        soundOff.gameObject.SetActive(true);
        //UpdateSound();
    }

    public void SoundsOn()
    {
        GameDataManager.Instance.playSound = 1;
        soundOff.gameObject.SetActive(false);
        soundOn.gameObject.SetActive(true);
        //UpdateSound();
    }

    public void VibrationOff()
    {
        GameDataManager.Instance.playVibrate = 0;
        vibrationOn.gameObject.SetActive(false);
        vibrationOff.gameObject.SetActive(true);
        Handheld.Vibrate();
        //UpdateVibrate();
    }

    public void VibrationOn()
    {
        GameDataManager.Instance.playVibrate = 1;
        vibrationOff.gameObject.SetActive(false);
        vibrationOn.gameObject.SetActive(true);
        Handheld.Vibrate();
       // UpdateVibrate();

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

    public void VibratePhone(){
        Handheld.Vibrate();
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