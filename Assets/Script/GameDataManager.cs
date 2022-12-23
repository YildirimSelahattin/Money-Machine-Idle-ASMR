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
    public int[] gridArray = new int[6];

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
        for(int i = 0; i < 6; i++)
        {
            gridArray[i] = PlayerPrefs.GetInt("GridValue"+i.ToString(),0);//open default
            if (i>1)
            {
                gridArray[i] = PlayerPrefs.GetInt("GridValue" + i.ToString(), -1);//closed default
            }
        }
    }
    public void SaveData()
    {
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("GridValue" + i.ToString(), gridArray[i]);//closed default
        }
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt("PlaySoundKey", playSound);
        PlayerPrefs.SetInt("PlayMusicKey", playMusic);

    }
    
}
