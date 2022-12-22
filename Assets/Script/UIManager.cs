using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject gridParent;
    public static UIManager Instance;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        //make a level array that contains every machine at the index of level number
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMachineAdddButtonClicked()
    {
        for(int gridIndex = 0; gridIndex < GameDataManager.Instance.gridIndexArrayList[GameDataManager.Instance.dataLists.currentGridMapIndex].Length; gridIndex++)
        {
            int valueOfGrid = GameDataManager.Instance.gridIndexArrayList[GameDataManager.Instance.dataLists.currentGridMapIndex][gridIndex];
            if (valueOfGrid == 0)//found a position that has no machines
            {
                //level 0 şu an veriliyor !!sadece
                GameDataManager.Instance.dataLists.gridIndexArray[gridIndex] = 1;
                Instantiate(GameDataManager.Instance.moneyMachineArray[1],gridParent.transform.GetChild(gridIndex).transform);
                break;
                
            }
        }
    }

}
