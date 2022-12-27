using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static UIManager Instance;

    public int NumberOfDiamonds
    {
        get { return PlayerPrefs.GetInt("NumberOfDiamondsKey", 0); }   // get method
        set
        {
            PlayerPrefs.SetInt("NumberOfDiamondsKey", value);
            //diamondNumberText.text = value.ToString();
        }
    }  // set method
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        //make a level array that contains every machine at the index of level number
    }

    public void OnMachineAdddButtonClicked()
    {
        for(int gridIndex = 0; gridIndex < GameDataManager.Instance.gridArray.Length; gridIndex++)
        {
            int valueOfGrid = GameDataManager.Instance.gridArray[gridIndex];
            if (valueOfGrid == 0)//found a position that has no machines
            {
                Debug.Log("qwe"+ gridIndex);
                //level 0 şu an veriliyor !!sadece
                GameManager.Instance.gridParent.transform.GetChild(gridIndex).gameObject.GetComponent<BoxCollider>().enabled = false;
                GameObject newMachine =Instantiate(GameDataManager.Instance.moneyMachineArray[1],GameManager.Instance.gridParent.transform.GetChild(gridIndex).transform);
                GameDataManager.Instance.gridArray[gridIndex] = 1;
                break;
            }
        }
        GameDataManager.Instance.SaveData();
    }
}
