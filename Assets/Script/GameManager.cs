using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  GameObject gridParent;
    public  int GRID_SURFACE_INDEX = 0;
    public int GRID_UPDATE_BUTTON_INDEX = 1;
    public static GameManager Instance;
    public Material openedGridMat;
    // Start is called before the first frame update


     void Awake() {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        
        DesignLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DesignLevel()
    {
        // run for every index in the array, instantiate machines according to the value of array -1 grid is closed
        // 0 means gird is avaliable for machines, 
        for (int index = 0;index < GameDataManager.Instance.gridArray.Length; index++)
        {
            if (GameDataManager.Instance.gridArray[index]>-1)// only for grid 
            {
                gridParent.transform.GetChild(index).GetChild(GRID_SURFACE_INDEX).gameObject.GetComponent<MeshRenderer>().material = openedGridMat; // open grid surface if its opened before
                gridParent.transform.GetChild(index).GetChild(GRID_UPDATE_BUTTON_INDEX).gameObject.SetActive(false);
            }

            if (GameDataManager.Instance.gridArray[index] > 0)
            {
                Instantiate(GameDataManager.Instance.moneyMachineArray[GameDataManager.Instance.gridArray[index]],  gridParent.transform.GetChild(index));
            }
        }
    }
}
