using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//THE ONLY DATA READER , READS FROM JSONTEXT
public class GameDataManager : MonoBehaviour
{
    
    public static GameDataManager Instance;
    public int playSound;
    public int playMusic;
    public AudioClip brushMachineMusic;
    public GameObject[] moneyMachineArray;
    public int[] gridArray = new int[6];
    public int maxLevelMachineAmount;
    public float beltSpeedButtonMoney = 5f;
    public float incomeButtonMoney = 6;
    public float addMachineButtonMoney = 7;
    public float workerSpeedButtonMoney = 7;
    public int beltSpeedButtonLevel = 1;
    public int incomeButtonLevel = 1;
    public int addMachineButtonLevel = 1;
    public int workerSpeedButtonLevel;
    public float moneyToBeCollected = 0;
    public float totalMoney = 0;
    public float TotalMoney
    {
        get { return totalMoney; }

        set
        {

            if (value > totalMoney)
            {
                if (totalMoney > beltSpeedButtonMoney)//activate belt speed button
                {
                    // interactable yap UIManager.Instance.beltSpeedButton.GetComponent<>// interactable yap
                    UIManager.Instance.beltSpeedButton.GetComponent<Button>().interactable = true;
                }
                if (totalMoney > incomeButtonMoney)//activate belt speed button
                {
                    //UIManager.Instance.incomeButton.GetComponent<>// interactable yap
                    UIManager.Instance.incomeButton.GetComponent<Button>().interactable=true;
                }
                if (totalMoney > workerSpeedButtonMoney)//activate belt speed button
                {
                    //UIManager.Instance.workerSpeedButton.GetComponent<>// interactable yap
                    UIManager.Instance.workerSpeedButton.GetComponent<Button>().interactable=true;
                }
                if (totalMoney > addMachineButtonMoney)//activate belt speed button
                {
                    //UIManager.Instance.addMachineButton.GetComponent<>// interactable yap
                    UIManager.Instance.addMachineButton.GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                if (totalMoney < beltSpeedButtonMoney)//activate belt speed button
                {
                    // interactable yap UIManager.Instance.beltSpeedButton.GetComponent<>// interactable yap
                    UIManager.Instance.beltSpeedButton.GetComponent<Button>().interactable = false;
                }
                if (totalMoney < incomeButtonMoney)//activate belt speed button
                {
                    UIManager.Instance.incomeButton.GetComponent<Button>().interactable = false;
                    //UIManager.Instance.incomeButton.GetComponent<>// interactable yap
                }
                if (totalMoney < workerSpeedButtonMoney)//activate belt speed button
                {
                    //UIManager.Instance.workerSpeedButton.GetComponent<>// interactable yap
                    UIManager.Instance.workerSpeedButton.GetComponent<Button>().interactable = false;
                }
                if (totalMoney < addMachineButtonMoney)//activate belt speed button
                {
                    //UIManager.Instance.addMachineButton.GetComponent<>// interactable yap
                    UIManager.Instance.addMachineButton.GetComponent<Button>().interactable = false;
                }
            }
            totalMoney = value;
        }
    }
    public float incomePerTap = 1;
    public float workerBaseSpeed;
    public float beltSpeed;
    public float machineIncomeMoney;
    public float offlineProgressNum = 2;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playSound = PlayerPrefs.GetInt("PlaySoundKey", 1);
            playMusic = PlayerPrefs.GetInt("PlayMusicKey", 1);
        }
        LoadData();
    }
    
    public void LoadData()
    {
        maxLevelMachineAmount = PlayerPrefs.GetInt("MaxLevelMachineAmount",0);
        //grid jobs
        for(int i = 0; i < 6; i++)
        {
            gridArray[i] = PlayerPrefs.GetInt("GridValue"+i.ToString(),0);//open default
            if (i>1)
            {
                gridArray[i] = PlayerPrefs.GetInt("GridValue" + i.ToString(), -1);//closed default
            }
        }

        // worker jobs
        workerBaseSpeed=PlayerPrefs.GetFloat("WorkerBaseSpeed", 3);
        ///////////////////////////////////////////
        /// Buttons
        TotalMoney = PlayerPrefs.GetFloat("TotalMoney", 15);

        moneyToBeCollected = PlayerPrefs.GetFloat("MoneyToBeCollected", moneyToBeCollected);
        workerSpeedButtonLevel = PlayerPrefs.GetInt("WorkerSpeedButtonLevel", workerSpeedButtonLevel);
        addMachineButtonLevel= PlayerPrefs.GetInt("AddMachineButtonLevel", addMachineButtonLevel);
        incomeButtonLevel = PlayerPrefs.GetInt("IncomeButtonLevel", incomeButtonLevel);
        incomeButtonMoney = PlayerPrefs.GetFloat("IncomeButtonMoney", incomeButtonMoney);
        workerSpeedButtonMoney = PlayerPrefs.GetFloat("WorkerSpeedButtonMoney", workerSpeedButtonMoney);
        beltSpeedButtonMoney = PlayerPrefs.GetFloat("BeltSpeedButtonMoney", beltSpeedButtonMoney);
        addMachineButtonMoney = PlayerPrefs.GetFloat("AddMachineButtonMoney", addMachineButtonMoney);
        incomePerTap = PlayerPrefs.GetFloat("IncomePerTap", incomePerTap);
        beltSpeed= PlayerPrefs.GetFloat("BeltSpeed", 0.02f);
        workerBaseSpeed= PlayerPrefs.GetFloat("WorkerSpeed",3);
    }
    
    public void SaveData()
    {
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("GridValue" + i.ToString(), gridArray[i]);//closed default
        }

        // worker jobs
        PlayerPrefs.SetFloat("WorkerBaseSpeed", workerBaseSpeed);

        //////////////////////////
        /// Buttons
        PlayerPrefs.SetFloat("TotalMoney", totalMoney);
        PlayerPrefs.SetFloat("MoneyToBeCollected", moneyToBeCollected);
        PlayerPrefs.SetFloat("WorkerSpeedButtonLevel", workerSpeedButtonLevel);
        PlayerPrefs.SetFloat("AddMachineButtonLevel", addMachineButtonLevel);
        PlayerPrefs.SetFloat("IncomeButtonLevel", incomeButtonLevel);
        PlayerPrefs.SetFloat("BeltSpeed", beltSpeed);
        PlayerPrefs.SetFloat("IncomePercentage", machineIncomeMoney);
        PlayerPrefs.SetFloat("WorkerSpeed", workerBaseSpeed);
        PlayerPrefs.SetFloat("IncomeButtonMoney", incomeButtonMoney);
        PlayerPrefs.SetFloat("WorkerSpeedButtonMoney", workerSpeedButtonMoney);
        PlayerPrefs.SetFloat("BeltSpeedButtonMoney", beltSpeedButtonMoney);
        PlayerPrefs.SetFloat("AddMachineButtonMoney", addMachineButtonMoney);
    }
    private void OnDisable()
    {
        
        PlayerPrefs.SetInt("PlaySoundKey", playSound);
        PlayerPrefs.SetInt("PlayMusicKey", playMusic);
    }
}
