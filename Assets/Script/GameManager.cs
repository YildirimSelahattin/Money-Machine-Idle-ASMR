using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  GameObject gridParent;
    int GRID_OPEN_CLOSE_BASE_INDEX = 0;
    public static GameManager Instance;
    [SerializeField] Material openedGridMat;
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
                gridParent.transform.GetChild(index).GetChild(GRID_OPEN_CLOSE_BASE_INDEX).gameObject.GetComponent<MeshRenderer>().material = openedGridMat;
            }

            if (GameDataManager.Instance.gridArray[index] > 0)
            {
                Instantiate(GameDataManager.Instance.moneyMachineArray[GameDataManager.Instance.gridArray[index]],  gridParent.transform.GetChild(index));
            }
        }
    }
}
