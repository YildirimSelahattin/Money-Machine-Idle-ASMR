using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
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
    private float beltSpeedButtonMoney;
    [SerializeField] float[] gridOpenWithMoneyPrices;

    public float BeltSpeedButtonMoney
    {
        // = 5f;
        get { return beltSpeedButtonMoney; }
        set { beltSpeedButtonMoney = GetOnly1DigitAfterPoint(value); }
    }

    [SerializeField] private float incomeButtonMoney;

    public float IncomeButtonMoney
    {
        get { return incomeButtonMoney; }
        set { incomeButtonMoney = GetOnly1DigitAfterPoint(value); }
    }

    [SerializeField] private float addMachineButtonMoney;

    public float AddMachineButtonMoney
    {
        get { return addMachineButtonMoney; }
        set { addMachineButtonMoney = GetOnly1DigitAfterPoint(value); }
    }

    [SerializeField] private float workerSpeedButtonMoney;

    public float WorkerSpeedButtonMoney
    {
        get { return workerSpeedButtonMoney; }
        set { workerSpeedButtonMoney = GetOnly1DigitAfterPoint(value); }
    }

    private float incomePerTap;

    public float IncomePerTap
    {
        get { return incomePerTap; }
        set { incomePerTap = GetOnly1DigitAfterPoint(value); }
    }

    public int beltSpeedButtonLevel = 1;
    public int incomeButtonLevel = 1;
    public int addMachineButtonLevel = 1;
    public int workerSpeedButtonLevel = 1;
    public float moneyToBeCollected = 0;
    public float totalMoney = 0;

    public float TotalMoney
    {
        get { return totalMoney; }

        set
        {
            totalMoney = GetOnly1DigitAfterPoint(value);
      
        }
    }
    

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
        maxLevelMachineAmount = PlayerPrefs.GetInt("MaxLevelMachineAmount", 0);
        //grid jobs
        for (int i = 0; i < 6; i++)
        {
            gridArray[i] = PlayerPrefs.GetInt("GridValue" + i.ToString(), 0); //open default
            if (i > 1)
            {
                gridArray[i] = PlayerPrefs.GetInt("GridValue" + i.ToString(), -1); //closed default
            }
        }

        // worker jobs
        workerBaseSpeed = PlayerPrefs.GetFloat("WorkerBaseSpeed", 3);
        ///////////////////////////////////////////
        /// Buttons


        workerSpeedButtonLevel = PlayerPrefs.GetInt("WorkerSpeedButtonLevel", workerSpeedButtonLevel);
        addMachineButtonLevel = PlayerPrefs.GetInt("AddMachineButtonLevel", addMachineButtonLevel);
        incomeButtonLevel = PlayerPrefs.GetInt("IncomeButtonLevel", incomeButtonLevel);
        beltSpeedButtonLevel = PlayerPrefs.GetInt("BeltSpeedButtonLevel", beltSpeedButtonLevel);
        IncomeButtonMoney = PlayerPrefs.GetFloat("IncomeButtonMoney", incomeButtonMoney);
        WorkerSpeedButtonMoney = PlayerPrefs.GetFloat("WorkerSpeedButtonMoney", 5);
        BeltSpeedButtonMoney = PlayerPrefs.GetFloat("BeltSpeedButtonMoney", 7);
        AddMachineButtonMoney = PlayerPrefs.GetFloat("AddMachineButtonMoney", 7);
        IncomePerTap = PlayerPrefs.GetFloat("IncomePerTap", 1);
        beltSpeed = PlayerPrefs.GetFloat("BeltSpeed", -0.05f);
        workerBaseSpeed = PlayerPrefs.GetFloat("WorkerSpeed", 3);
        TotalMoney = PlayerPrefs.GetFloat("TotalMoney", 3);
    }

    public void SaveData()
    {
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("GridValue" + i.ToString(), gridArray[i]); //closed default
        }

        // worker jobs
        PlayerPrefs.SetFloat("WorkerBaseSpeed", workerBaseSpeed);

        //////////////////////////
        /// Buttons
        PlayerPrefs.SetFloat("TotalMoney", totalMoney);
        PlayerPrefs.SetFloat("MoneyToBeCollected", moneyToBeCollected);
        PlayerPrefs.SetInt("WorkerSpeedButtonLevel", workerSpeedButtonLevel);
        PlayerPrefs.SetInt("AddMachineButtonLevel", addMachineButtonLevel);
        PlayerPrefs.SetInt("IncomeButtonLevel", incomeButtonLevel);
        PlayerPrefs.SetInt("BeltSpeedButtonLevel", beltSpeedButtonLevel);
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
        SaveData();
        PlayerPrefs.SetInt("PlaySoundKey", playSound);
        PlayerPrefs.SetInt("PlayMusicKey", playMusic);
    }

    public float GetOnly1DigitAfterPoint(float number)
    {
        //Debug.Log((float)System.Math.Round(number, 1)+"aaa");
        //return (float)System.Math.Round(number, 1);
        return (float)((int)number * 100f) / 100f;
}
    public void ControlButtons()
    {

        // UI BUTTON S�DE
        if (totalMoney >= BeltSpeedButtonMoney) //activate belt speed button
        {
            // interactable yap UIManager.Instance.beltSpeedButton.GetComponent<>// interactable yap
            UIManager.Instance.beltSpeedButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            // interactable yap UIManager.Instance.beltSpeedButton.GetComponent<>// interactable yap
            UIManager.Instance.beltSpeedButton.GetComponent<Button>().interactable = false;
        }

        if (totalMoney >= IncomeButtonMoney) //activate belt speed button
        {
            //UIManager.Instance.incomeButton.GetComponent<>// interactable yap
            UIManager.Instance.incomeButton.GetComponent<Button>().interactable = true;
        }
        else //activate belt speed button
        {
            UIManager.Instance.incomeButton.GetComponent<Button>().interactable = false;
            //UIManager.Instance.incomeButton.GetComponent<>// interactable yap
        }

        if (totalMoney >= WorkerSpeedButtonMoney) //activate belt speed button
        {
            //UIManager.Instance.workerSpeedButton.GetComponent<>// interactable yap
            UIManager.Instance.workerSpeedButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            //UIManager.Instance.workerSpeedButton.GetComponent<>// interactable yap
            UIManager.Instance.workerSpeedButton.GetComponent<Button>().interactable = false;
        }

        if (totalMoney >= AddMachineButtonMoney) //activate belt speed button
        {
            foreach (int valueOfGrid in GameDataManager.Instance.gridArray)
            {
                if (valueOfGrid == 0)
                {
                    UIManager.Instance.addMachineButton.GetComponent<Button>().interactable = true;
                    break;
                }
            }
        }
        else //activate belt speed button
        {
            //UIManager.Instance.addMachineButton.GetComponent<>// interactable yap
            UIManager.Instance.addMachineButton.GetComponent<Button>().interactable = false;
        }



        // 3D BUTTON SIDE ilkine bak�lamyabilir?
        for (int gridIndex = 0; gridIndex < gridOpenWithMoneyPrices.Length; gridIndex++)
        {
            Debug.Log("sasa");
            if (gridArray[gridIndex] == -1)
            {
                if (totalMoney > gridOpenWithMoneyPrices[gridIndex])
                {
                    UIManager.Instance.gridMoneyOpenInteractableArray[gridIndex].gameObject.SetActive(true);
                    UIManager.Instance.gridMoneyOpenNotInteractableArray[gridIndex].gameObject.SetActive(false);
                }
                else
                {
                    UIManager.Instance.gridMoneyOpenInteractableArray[gridIndex].gameObject.SetActive(false);
                    UIManager.Instance.gridMoneyOpenNotInteractableArray[gridIndex].gameObject.SetActive(true);
                }
            }
        }
    }
}