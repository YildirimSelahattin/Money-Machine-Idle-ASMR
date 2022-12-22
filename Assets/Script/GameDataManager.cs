using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
//THE ONLY DATA READER , READS FROM JSONTEXT
public class GameDataManager : MonoBehaviour
{
    [SerializeField] TextAsset JSONText;
    public static GameDataManager Instance;
    public DataList dataLists;
    public int playSound;
    public int playMusic;
    public AudioClip brushMachineMusic;
    public List<int[]> gridIndexArrayList = new List<int[]>(); // 6 blocks means one grid
    public GameObject[] moneyMachineArray;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playSound = PlayerPrefs.GetInt("PlaySoundKey", 1);
            playMusic = PlayerPrefs.GetInt("PlayMusicKey", 1);
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("PlaySoundKey", playSound);
        PlayerPrefs.SetInt("PlayMusicKey", playMusic);
    }
    
}
