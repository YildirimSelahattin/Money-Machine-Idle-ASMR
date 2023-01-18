using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class MachineTriggerManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject comingWorkerObject = null;

    void Start()
    {
    }

    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<MachineManager>().inSnapArea = false;
        gameObject.GetComponent<MachineManager>().inMergeArea = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // merge job
        if (other.gameObject.transform.CompareTag(this.gameObject.transform.tag) &&
            gameObject.GetComponent<MachineManager>().dropped == true) //if they are same level
        {
            if (gameObject.GetComponent<MachineManager>().levelIndexOfObject < 6) //if the machine is mergeable
            {
                if (PlayerPrefs.GetInt("isFirstMerge", 1)==1)
                {
                    PlayerPrefs.SetInt("isFirstMerge", -1);
                    UIManager.Instance.MergeHand.transform.DOKill();
                    UIManager.Instance.MergeHand.SetActive(false);
                }
                int targetGrid = other.gameObject.GetComponent<MachineManager>().gridIndexNumberOfObject;
                int currentGrid = gameObject.transform.parent.tag[transform.parent.tag.Length - 1] - '0';
                //make changes on grid array
                GameDataManager.Instance.gridArray[targetGrid] =gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1; // save the merged machines level on the new grid
                GameDataManager.Instance.gridArray[currentGrid] = 0;
                //worker deleting
                GameObject workerThatWorksForCurrentGrid = Spawner.Instance.gridWorkerArray[currentGrid];
                Spawner.Instance.gridWorkerArray[currentGrid] = null;
                Destroy(workerThatWorksForCurrentGrid);
                ////
                GameManager.Instance.gridParent.transform.GetChild(targetGrid).GetComponent<BoxCollider>().enabled = false;
                GameManager.Instance.gridParent.transform.GetChild(currentGrid).GetComponent<BoxCollider>().enabled = true;
                if (gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1 == 6)
                {
                    GameDataManager.Instance.maxLevelMachineAmount++;
                }
                Destroy(other.gameObject);
                Destroy(gameObject);
                Instantiate(
                    GameDataManager.Instance.moneyMachineArray[
                        other.gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1],
                    other.transform.parent);
                GameDataManager.Instance.SaveData();
                GameDataManager.Instance.ControlButtons();

            }
        }

        if (other.gameObject.transform.tag.Contains("Grid") &&
            gameObject.GetComponent<MachineManager>().dropped == true &&
            gameObject.GetComponent<MachineManager>().inSnapArea != false) //if machine is in the grid area 
        {
            gameObject.GetComponent<MachineManager>().inSnapArea = false;
            int currentGridOfMachine = gameObject.transform.parent.tag[transform.parent.tag.Length - 1] - '0';
            int targetGrid = other.gameObject.transform.tag[other.gameObject.transform.tag.Length - 1] - '0';
            int levelIndexOfDraggedMachine = gameObject.GetComponent<MachineManager>().levelIndexOfObject;
            GameObject worker = Spawner.Instance.gridWorkerArray[currentGridOfMachine];
            gameObject.GetComponent<MachineManager>().gridIndexNumberOfObject = targetGrid; // save it on the new grid
            GameManager.Instance.gridParent.transform.GetChild(currentGridOfMachine).GetComponent<BoxCollider>().enabled = true;
            GameManager.Instance.gridParent.transform.GetChild(targetGrid).GetComponent<BoxCollider>().enabled = false;
            GameDataManager.Instance.gridArray[targetGrid] = levelIndexOfDraggedMachine; // save it on the new grid
            GameDataManager.Instance.gridArray[currentGridOfMachine] = 0;
            //delete current grids worker from array
            GameObject workerToDelete = Spawner.Instance.gridWorkerArray[currentGridOfMachine];
            Spawner.Instance.gridWorkerArray[currentGridOfMachine] = null;
            Destroy(workerToDelete);
            //ad the new grids worker to the array
            StartCoroutine(Spawner.Instance.AddWorkerAfterDelay(targetGrid,1));
            ///
            gameObject.transform.SetParent(other.gameObject.transform);
            gameObject.transform.DOKill();
            gameObject.transform.DOLocalMove(GameDataManager.Instance.moneyMachineArray[levelIndexOfDraggedMachine].transform.position, 0.3f)
                .OnComplete(() => GettingTouchManager.Instance.objectToDrag = null).OnComplete(()=> worker.GetComponent<WorkerManager>().MoveMachineAndComeBackByIndex());// when snapping to neew grid job finishes, start worker movement
            GameDataManager.Instance.SaveData();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag(this.gameObject.transform.tag)) //if they are same level
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