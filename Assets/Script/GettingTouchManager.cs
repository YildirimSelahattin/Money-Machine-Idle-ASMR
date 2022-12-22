using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class GettingTouchManager : MonoBehaviour
{
    float instantiateZ = 0;
    float celSize = 10;
    [SerializeField]LayerMask touchableLayerOnlyMachines;
    [SerializeField] LayerMask touchableLayerExceptMachines;
    [SerializeField] LayerMask touchableLayerOnlyUpgrade;
    // Start is called before the first frame update
    public GameObject objectToDrag;
    Vector3 originalPosOrDraggingObject;
    RaycastHit hit;
    Ray ray;
    public static GettingTouchManager Instance;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.touchCount > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
            { // This is actions when finger/cursor hit screen
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyMachines))
                {
                    objectToDrag = hit.collider.gameObject;
                    objectToDrag.GetComponent<MachineManager>().dropped = false;
                    objectToDrag.AddComponent<MachineTriggerManager>();
                    originalPosOrDraggingObject = hit.collider.transform.position;
                }

                else if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerOnlyUpgrade)){
                    Debug.Log(hit.collider.gameObject.transform.parent.tag);
                }
            }

            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary  && objectToDrag != null) 
            {
                // This is actions when finger/cursor pressed on screen
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, touchableLayerExceptMachines))
                {
                    objectToDrag.transform.DOKill();
                    objectToDrag.transform.DOMove(new Vector3(hit.point.x, objectToDrag.transform.position.y, hit.point.z),1);
                }
            }

            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended ) 
            { // This is actions when finger/cursor get out from screen
                if (objectToDrag != null)
                {
                    objectToDrag.GetComponent<MachineManager>().dropped = true;
                    if(objectToDrag?.GetComponent<MachineManager>().inMergeArea != true)
                    {
                        objectToDrag.transform.DOKill();
                        objectToDrag.transform.position = originalPosOrDraggingObject;
                        Destroy(objectToDrag.GetComponent<MachineTriggerManager>());
                        objectToDrag = null;
                    }
                    
                }
                
            }
        }
    }

   
}
