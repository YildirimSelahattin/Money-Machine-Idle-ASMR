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
                GameDataManager.Instance.gridArray[other.gameObject.GetComponent<MachineManager>().gridIndexNumberOfObject] = gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1;// save it on the 
                GameDataManager.Instance.gridArray[gameObject.transform.parent.tag[transform.parent.tag.Length - 1] - '0'] = 0;
                Destroy(other.gameObject);
                Destroy(gameObject);
                Instantiate(GameDataManager.Instance.moneyMachineArray[other.gameObject.GetComponent<MachineManager>().levelIndexOfObject + 1], other.transform.parent);
                GameDataManager.Instance.SaveData();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag(this.gameObject.transform.tag))//if they are same level
        {

            if (gameObject.GetComponent<MachineManager>().levelIndexOfObject < 6) //if the machine is mergeable
            {

                gameObject.GetComponent<MachineManager>().inMergeArea = true;
            }
        }
    }
}
