using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;
    public int levelIndexOfObject;
    public int gridIndexNumberOfObject;
    public bool dropped = true;
    public bool inMergeArea = false;
    public bool isWorking = false;
    //
    public float countWaitTime;
    private AudioSource _moneySound;
    private TMP_Text _counterText;
    public bool inSnapArea = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        levelIndexOfObject = transform.tag[transform.tag.Length - 1] - '0';
        gridIndexNumberOfObject = transform.parent.tag[transform.parent.tag.Length - 1] - '0';
        _moneySound = gameObject.GetComponent<AudioSource>();
        _counterText = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            StartCoroutine(WaitAndPrint(countWaitTime));
        }
    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        for (int i = 0; i < 5; i++)
        {
            _moneySound.Play();
            _counterText.text = (i+1) + "/5";
            yield return new WaitForSeconds(waitTime);
            if (i == 4)
            {
                isWorking = false;
            }
        }
    }
}