using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
//THE ONLY DATA READER , READS FROM JSONTEXT
public class GameDataManager : MonoBehaviour
{
    
    public static GameDataManager Instance;
    public int playSound;
    public int playMusic;
    public AudioClip brushMachineMusic;
    public GameObject[] moneyMachineArray;
    public WorkerManager[] workerArray;
    public int[] gridArray = new int[6];
    public int maxLevelMachineAmount;

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
        int workerArrayLength = PlayerPrefs.GetInt("WorkerArrayLength",1);
        workerArray = new WorkerManager[workerArrayLength];
        for (int i= 0; i < workerArrayLength; i++)
        {
            WorkerManager wm = new WorkerManager();

            wm.GetComponent<WorkerManager>().wheelBorrowCapacity = PlayerPrefs.GetFloat("Worker"+i+"wheelBorrowCapacity",10);
            wm.GetComponent<WorkerManager>().addedTimeWhileGoing = PlayerPrefs.GetFloat("Worker" + i + "addedTimeWhileGoing", 3);
            wm.GetComponent<WorkerManager>().maxComeAndGoCounter = PlayerPrefs.GetFloat("Worker" + i + "maxComeAndGoCounter", 10);
            wm.GetComponent<WorkerManager>()._baseSpeed = PlayerPrefs.GetFloat("Worker" + i + "baseSpeed", 3);

            workerArray[i] = wm;
        }
    }
    
    public void SaveData()
    {
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("GridValue" + i.ToString(), gridArray[i]);//closed default
        }
        for (int i = 0; i < workerArray.Length; i++)
        {
            PlayerPrefs.SetFloat("Worker" + i + "wheelBorrowCapacity", workerArray[i].wheelBorrowCapacity);
            PlayerPrefs.SetFloat("Worker" + i + "addedTimeWhileGoing", workerArray[i].addedTimeWhileGoing);
            PlayerPrefs.SetFloat("Worker" + i + "maxComeAndGoCounter", workerArray[i].maxComeAndGoCounter);
            PlayerPrefs.SetFloat("Worker" + i + "baseSpeed", workerArray[i]._baseSpeed);
        }
        PlayerPrefs.SetInt("WorkerArrayLength", workerArray.Length);
    }
    private void OnDisable()
    {
        
        PlayerPrefs.SetInt("PlaySoundKey", playSound);
        PlayerPrefs.SetInt("PlayMusicKey", playMusic);
    }
}
