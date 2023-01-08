using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

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
    [SerializeField] ParticleSystem moneyTapParticle;

    // Start is called before the first frame update
    public GameObject objectToDrag;
    public GameObject objectMoney;

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
                }
              
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyUpgrade)) // when it hits to upgrade button
                {
                    //if(watchedAd)
                    GameObject parentGridOfHitButton = hit.collider.gameObject.transform.parent.gameObject;
                    parentGridOfHitButton.transform.GetChild(GameManager.Instance.GRID_SURFACE_INDEX).gameObject
                            .GetComponent<MeshRenderer>().material =
                        GameManager.Instance.openedGridMat; //open grid visually 
                    hit.collider.gameObject.SetActive(false); //close upgrade button
                    hit.collider.transform.parent.gameObject.GetComponent<BoxCollider>().enabled = true;
                    GameDataManager.Instance.gridArray[
                            parentGridOfHitButton.transform.tag[parentGridOfHitButton.transform.tag.Length - 1] - '0'] =
                        0; // open grid index base
                }
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyTapToCollect)) // if it is money tap
                {
                    moneyTapParticle.gameObject.transform.position=new Vector3(hit.point.x ,hit.point.y,hit.point.z-5);
                    moneyTapParticle.Play();
                    Debug.Log(hit.point);
                    GameDataManager.Instance.TotalMoney += GameDataManager.Instance.incomePerTap;
                   
                }
            }

            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary && objectToDrag != null)
            {
                // This is actions when finger/cursor pressed on screen
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyTapToCollect))
                {
                    objectToDrag.transform.DOKill();
                    objectToDrag.transform.DOMove(
                        new Vector3(hit.point.x, objectToDrag.transform.position.y, hit.point.z), .3f);
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
}