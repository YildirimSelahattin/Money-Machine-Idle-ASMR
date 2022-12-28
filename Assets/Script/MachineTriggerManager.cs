using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineTriggerManager : MonoBehaviour
{
    // Start is called before the first frame update
  
    void Start()
    {
        Debug.Log("sa");
    }

    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<MachineManager>().inSnapArea = false;
        gameObject.GetComponent<MachineManager>().inMergeArea = false;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.CompareTag(this.gameObject.transform.tag) && gameObject.GetComponent<MachineManager>().dropped == true)//if they are same level
        {
            Debug.Log("emir");
            if (gameObject.GetComponent<MachineManager>().levelIndexOfObject < 6) //if the machine is mergeable
            {
                //Instantiate(GameDataManatger.Instance.moneyMachineArray[other.gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1],other.transform.parent);
                GameDataManager.Instance.gridArray[other.gameObject.GetComponent<MachineManager>().gridIndexNumberOfObject] = gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1;// save the merged machines level on the new grid
                GameDataManager.Instance.gridArray[gameObject.transform.parent.tag[transform.parent.tag.Length - 1] - '0'] = 0;
                GameManager.Instance.gridParent.transform.GetChild(other.gameObject.transform.parent.tag[other.transform.parent.tag.Length - 1] - '0').GetComponent<BoxCollider>().enabled = false;
                GameManager.Instance.gridParent.transform.GetChild(gameObject.transform.parent.tag[gameObject.transform.parent.tag.Length - 1] - '0').GetComponent<BoxCollider>().enabled = true;
                Destroy(other.gameObject);
                Destroy(gameObject);
                Debug.Log("");
                Instantiate(GameDataManager.Instance.moneyMachineArray[other.gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1], other.transform.parent);
                GameDataManager.Instance.SaveData();
            }
        }
        if(other.gameObject.transform.tag.Contains("Grid") && gameObject.GetComponent<MachineManager>().dropped == true && gameObject.GetComponent<MachineManager>().inSnapArea != false)//if machine is in the grid area 
        {
            gameObject.GetComponent<MachineManager>().inSnapArea = false;
            Debug.Log("SUI");
            int currentGridOfMachine = gameObject.transform.parent.tag[transform.parent.tag.Length - 1] - '0';
            int targetGrid = other.gameObject.transform.tag[other.gameObject.transform.tag.Length - 1] - '0';
            int levelIndexOfDraggedMachine= gameObject.GetComponent<MachineManager>().levelIndexOfObject;
            gameObject.GetComponent<MachineManager>().gridIndexNumberOfObject = targetGrid;// save it on the new grid
            GameManager.Instance.gridParent.transform.GetChild(currentGridOfMachine).GetComponent<BoxCollider>().enabled = true;
            GameManager.Instance.gridParent.transform.GetChild(targetGrid).GetComponent<BoxCollider>().enabled = false;
            GameDataManager.Instance.gridArray[targetGrid] = levelIndexOfDraggedMachine;// save it on the new grid
            GameDataManager.Instance.gridArray[currentGridOfMachine] = 0;
            gameObject.transform.SetParent(other.gameObject.transform);
            gameObject.transform.DOKill();
            gameObject.transform.DOLocalMove(new Vector3(0,0.5f,0),0.3f).OnComplete(()=> GettingTouchManager.Instance.objectToDrag = null);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag(this.gameObject.transform.tag))//if they are same level
        {
            Debug.Log("a");
            if (gameObject.GetComponent<MachineManager>().levelIndexOfObject < 6) //if the machine is mergeable
            {
                gameObject.GetComponent<MachineManager>().inMergeArea = true;
            }
        }
        if (other.gameObject.transform.tag.Contains("Grid"))
        {
            Debug.Log("a");
            gameObject.GetComponent<MachineManager>().inSnapArea = true;
        }
    }
}