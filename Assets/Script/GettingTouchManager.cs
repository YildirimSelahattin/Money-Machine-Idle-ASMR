using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class GettingTouchManager : MonoBehaviour
{
    public static GettingTouchManager Instance;
    [SerializeField] GameObject particles;
    float instantiateZ = 0;
    float celSize = 10;
    [SerializeField] float startScale = 0.1f;
    [SerializeField] float endScale = 0.5f;

    [SerializeField] LayerMask touchableLayerOnlyMachines;
    [SerializeField] LayerMask touchableLayerOnlyCoins;

    [SerializeField] LayerMask touchableLayerExceptMachines;

    [SerializeField] LayerMask touchableLayerOnlyUpgrade;

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
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // This is actions when finger/cursor hit screen
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyMachines))
                {
                    objectToDrag = hit.collider.gameObject;
                    objectToDrag.GetComponent<MachineManager>().dropped = false;
                    objectToDrag.AddComponent<MachineTriggerManager>();
                    originalPosOrDraggingObject = hit.collider.transform.localPosition;
                    Spawner.Instance.gridWorkerArray[objectToDrag.GetComponent<MachineManager>().gridIndexNumberOfObject].GetComponent<WorkerManager>().GoBackToPile();
                    Spawner.Instance.gridWorkerArray[objectToDrag.GetComponent<MachineManager>().gridIndexNumberOfObject].GetComponent<WorkerManager>().waitingForGridDecision = true;

                }
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyCoins))
                {
                    //Get the money game object in second plane's child 
                    objectMoney = hit.collider.gameObject.transform.GetChild(0).gameObject;
                    //Instantiate a money clone
                    GameObject cloneMoney = Instantiate(objectMoney, objectMoney.transform.position,
                        objectMoney.transform.rotation, objectMoney.transform.parent);
                    objectMoney.SetActive(false);
                    GameObject cloneParticle =
                        Instantiate(particles, objectMoney.transform.position, Quaternion.identity);
                    cloneParticle.SetActive(true);

                    // Scale the instantiated money and destroy
                    cloneMoney.transform.DOMove(new Vector3(0, 2, -8), 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
                    {
                        cloneMoney.transform.DOScale(startScale, 0.2f).SetEase(Ease.Flash)
                            .OnComplete(() => { Destroy(cloneMoney); });

                        // Make it active after X time to collect again
                        objectMoney.SetActive(true);
                    });
                }
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyUpgrade))
                {
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
            }

            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary && objectToDrag != null)
            {
                // This is actions when finger/cursor pressed on screen
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerExceptMachines))
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