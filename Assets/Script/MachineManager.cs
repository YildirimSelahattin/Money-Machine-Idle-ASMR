using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public  int levelIndexOfObject;
    public int gridIndexNumberOfObject;
    public bool dropped = true;
    public bool inMergeArea = false;
    void Start()
    {
        levelIndexOfObject = transform.tag[transform.tag.Length - 1] - '0';
        gridIndexNumberOfObject = transform.parent.tag[transform.parent.tag.Length - 1] - '0';
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
