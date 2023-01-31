using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

//THE ONLY DATA READER , READS FROM JSONTEXT
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public int playSound;
    public int playMusic;
    public int playVibrate;
    public AudioClip brushMachineMusic;
    public GameObject[] moneyMachineArray;
    public int[] gridArray = new int[6];
    public int maxLevelMachineAmount;
    public long beltSpeedButtonMoney;
    public long[] gridOpenWithMoneyPrices;

    public long BeltSpeedButtonMoney
    {
        // = 5f;
        get { return beltSpeedButtonMoney; }
        set { beltSpeedButtonMoney = AbbrevationUtility.RoundNumberLikeText(value); }
    }

    [SerializeField] public long incomeButtonMoney;

    public long IncomeButtonMoney
    {
        get { return incomeButtonMoney; }
        set { incomeButtonMoney = AbbrevationUtility.RoundNumberLikeText(value); }
    }

    [SerializeField] public long addMachineButtonMoney;

    public long AddMachineButtonMoney
    {
        get { return addMachineButtonMoney; }
        set { addMachineButtonMoney = AbbrevationUtility.RoundNumberLikeText(value); }
    }

    [SerializeField] public long workerSpeedButtonMoney;

    public long WorkerSpeedButtonMoney
    {
        get { return workerSpeedButtonMoney; }
        set { workerSpeedButtonMoney = AbbrevationUtility.RoundNumberLikeText(value); }
    }

    public long incomePerTap;

    public float IncomePerTap
    {
        get { return incomePerTap; }
        set { incomePerTap = AbbrevationUtility.RoundNumberLikeText((long)value); }
    }

    public int beltSpeedButtonLevel = 1;
    public int incomeButtonLevel = 1;
    public int addMachineButtonLevel = 1;
    public int workerSpeedButtonLevel = 1;
    public long moneyToBeCollected = 0;
    public long totalMoney = 0;
    public float IncomePercantage = 0.04f;

    public long TotalMoney
    {
        get { return totalMoney; }

        set
        {
            totalMoney = value;
            if (UIManager.Instance != null)
                ControlButtons();
        }
    }


    public float workerBaseSpeed;
    public float beltSpeed;
    public long machineIncomeMoney;
    public float offlineProgressNum = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        LoadData();
    }
    
    void Start()
    {
        playSound = PlayerPrefs.GetInt("PlaySoundKey", 1);
        playMusic = PlayerPrefs.GetInt("PlayMusicKey", 1);
        playVibrate = PlayerPrefs.GetInt("PlayVibrateKey", 1);
        
        OfflineProgress.Instance.OfflinePanelControl();
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
        string temp = PlayerPrefs.GetString("IncomeButtonMoney", incomeButtonMoney.ToString());
        IncomeButtonMoney = Convert.ToInt64(temp);
        temp = PlayerPrefs.GetString("WorkerSpeedButtonMoney", 5.ToString());
        WorkerSpeedButtonMoney = Convert.ToInt64(temp);
        temp = PlayerPrefs.GetString("BeltSpeedButtonMoney", 7.ToString());
        BeltSpeedButtonMoney = Convert.ToInt64(temp);
        temp = PlayerPrefs.GetString("AddMachineButtonMoney", 7.ToString());
        AddMachineButtonMoney = Convert.ToInt64(temp);
        IncomePerTap = PlayerPrefs.GetFloat("IncomePerTap", 2);
        beltSpeed = PlayerPrefs.GetFloat("BeltSpeed", -0.05f);
        workerBaseSpeed = PlayerPrefs.GetFloat("WorkerSpeed", 3);
        temp = PlayerPrefs.GetString("TotalMoney", 1.ToString());
        TotalMoney = Convert.ToInt64(temp);
        IncomePercantage = PlayerPrefs.GetFloat("IncomePercentage", IncomePercantage);
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
        PlayerPrefs.SetFloat("IncomePercentage", IncomePercantage);
        PlayerPrefs.SetFloat("IncomePerTap", IncomePerTap);
        PlayerPrefs.SetFloat("WorkerSpeed", workerBaseSpeed);
        PlayerPrefs.SetString("TotalMoney", TotalMoney.ToString());
        PlayerPrefs.SetString("IncomeButtonMoney", IncomeButtonMoney.ToString());
        PlayerPrefs.SetString("WorkerSpeedButtonMoney", WorkerSpeedButtonMoney.ToString());
        PlayerPrefs.SetString("BeltSpeedButtonMoney", BeltSpeedButtonMoney.ToString());
        PlayerPrefs.SetString("AddMachineButtonMoney", AddMachineButtonMoney.ToString());
        PlayerPrefs.SetInt("MaxLevelMachineAmount", maxLevelMachineAmount);
    }

    private void OnDisable()
    {
        SaveData();
        PlayerPrefs.SetInt("PlaySoundKey", playSound);
        PlayerPrefs.SetInt("PlayMusicKey", playMusic);
        PlayerPrefs.SetInt("PlayVibrateKey", playVibrate);
    }


    public void ControlButtons()
    {
        // UI BUTTON 
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
                    if (UIManager.Instance.addMachineTapAmount == 0)
                    {
                        UIManager.Instance.addMachineHand.SetActive(true);
                    }

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
            if (gridArray[gridIndex] == -1)
            {
                if (totalMoney > gridOpenWithMoneyPrices[gridIndex])
                {
                    UIManager.Instance.gridMoneyOpenInteractableArray[gridIndex].gameObject.SetActive(true);
                    if (UIManager.Instance.gridOpenHand.active != true)
                    {
                        UIManager.Instance.gridOpenHand.transform.position =
                            UIManager.Instance.gridMoneyOpenInteractableArray[gridIndex].gameObject.transform.position +
                            Vector3.up * 4f;
                        UIManager.Instance.gridOpenHand.SetActive(true);
                    }

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