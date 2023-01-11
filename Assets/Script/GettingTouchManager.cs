using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
public class GettingTouchManager : MonoBehaviour
{
    public static GettingTouchManager Instance;
    [SerializeField] GameObject particles;
    float instantiateZ = 0;
    float celSize = 10;
    [SerializeField] float startScale = 0.1f;
    [SerializeField] float endScale = 0.5f;

    [SerializeField] LayerMask touchableLayerOnlyMachines;
    [SerializeField] LayerMask touchableLayerOnlyUpgrade;
    [SerializeField] LayerMask touchableLayerOnlyTapToCollect;
    [SerializeField] LayerMask touchableLayerEverything;
    [SerializeField] ParticleSystem moneyTapParticle;
    int maxTapNumberUntilInterstitial = 10;
    int moneyTapNumber = 0;
    // Start is called before the first frame update
    public GameObject objectToDrag;
    public GameObject objectMoney;
    GameObject gridObjectToOpen;
    Vector3 originalPosOrDraggingObject;
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)                // This is actions when finger/cursor hit screen
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyMachines)) // if it hit to a machine object
                {
                    objectToDrag = hit.collider.gameObject;
                    objectToDrag.GetComponent<MachineManager>().dropped = false;
                    objectToDrag.AddComponent<MachineTriggerManager>();
                    originalPosOrDraggingObject = hit.collider.transform.localPosition;
                    Spawner.Instance.gridWorkerArray[objectToDrag.GetComponent<MachineManager>().gridIndexNumberOfObject].GetComponent<WorkerManager>().GoBackToPile();
                    Spawner.Instance.gridWorkerArray[objectToDrag.GetComponent<MachineManager>().gridIndexNumberOfObject].GetComponent<WorkerManager>().waitingForGridDecision = true;
                    Debug.Log("1");
                }
              
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyUpgrade)) // when it hits to upgrade button
                {
                    gridObjectToOpen = hit.collider.gameObject.transform.parent.gameObject;
                    RewardedAdManager.Instance.GridRewardAd();
                }
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyTapToCollect)) // if it is money tap
                {
                    moneyTapParticle.gameObject.transform.position=new Vector3(hit.point.x ,hit.point.y+1,hit.point.z);
                    moneyTapParticle.Play();
                    GameDataManager.Instance.TotalMoney += GameDataManager.Instance.IncomePerTap;
                    UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);
                    Debug.Log("3");
                    if( moneyTapNumber > maxTapNumberUntilInterstitial)
                    {
                        maxTapNumberUntilInterstitial += 5;
                        moneyTapNumber = 0;
                        //request interstitial here
                    }
                }
            }

            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary && objectToDrag != null)
            {

                Debug.Log("POINT");
                // This is actions when finger/cursor pressed on screen
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerEverything))
                {
                         objectToDrag.transform.DOMove(
                             new Vector3(hit.point.x, objectToDrag.transform.position.y, hit.point.z), .3f);
                         Debug.Log("POINTtt"+hit.point);
                }
            }

            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // This is actions when finger/cursor get out from screen
                if (objectToDrag != null)
                {
                    objectToDrag.GetComponent<MachineManager>().dropped = true;
                    if (objectToDrag.GetComponent<MachineManager>().inMergeArea != true &&
                        objectToDrag.GetComponent<MachineManager>().inSnapArea != true)
                    {
                        objectToDrag.transform.DOKill();
                        GameObject workerForThisMachine = Spawner.Instance.gridWorkerArray[objectToDrag.GetComponent<MachineManager>().gridIndexNumberOfObject];
                        workerForThisMachine.GetComponent<WorkerManager>().waitingForGridDecision = false;
                        if (workerForThisMachine.GetComponent<WorkerManager>().moveStage == 1)
                        {
                            Spawner.Instance.gridWorkerArray[objectToDrag.GetComponent<MachineManager>().gridIndexNumberOfObject].GetComponent<WorkerManager>().MoveMachineAndComeBackByIndex();
                        }
                        objectToDrag.transform.localPosition = originalPosOrDraggingObject;
                        Destroy(objectToDrag.GetComponent<MachineTriggerManager>());
                        objectToDrag = null;
                    }
                }
            }
        }
    }
    public void GiveGridReward()
    {
        gridObjectToOpen.transform.GetChild(GameManager.Instance.GRID_SURFACE_INDEX).gameObject
                            .GetComponent<MeshRenderer>().material =
                        GameManager.Instance.openedGridMat; //open grid visually 
        gridObjectToOpen.transform.GetChild(GameManager.Instance.GRID_UPDATE_BUTTON_INDEX).gameObject.SetActive(false); //close upgrade button
        gridObjectToOpen.GetComponent<BoxCollider>().enabled = true;
        GameDataManager.Instance.gridArray[
                gridObjectToOpen.transform.tag[gridObjectToOpen.transform.tag.Length - 1] - '0'] =
            0; // open grid index base
        Debug.Log("2");
        if (GameDataManager.Instance.TotalMoney >= GameDataManager.Instance.AddMachineButtonMoney)
        {
            UIManager.Instance.addMachineButton.GetComponent<Button>().interactable = true;
        }
    }
}