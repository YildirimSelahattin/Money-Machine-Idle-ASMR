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

    public void DesignLevel()
    {
        // run for every index in the array, instantiate machines according to the value of array -1 grid is closed
        // 0 means gird is avaliable for machines, 
        for (int index = 0;index < GameDataManager.Instance.gridArray.Length; index++)
        {
            int valueOfGrid = GameDataManager.Instance.gridArray[index];
            if (valueOfGrid > -1)// should gird base be opened
            {
                gridParent.transform.GetChild(index).GetChild(GRID_SURFACE_INDEX).gameObject.GetComponent<MeshRenderer>().material = openedGridMat; // open grid surface if its opened before
                gridParent.transform.GetChild(index).GetChild(GRID_UPDATE_BUTTON_INDEX).gameObject.SetActive(false);
            }
            if(valueOfGrid == 0) // should snap grid be opened
            {
                gridParent.transform.GetChild(index).gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            if (valueOfGrid > 0) //
            {
                Instantiate(GameDataManager.Instance.moneyMachineArray[GameDataManager.Instance.gridArray[index]],  gridParent.transform.GetChild(index));
            }
        }
    }
}