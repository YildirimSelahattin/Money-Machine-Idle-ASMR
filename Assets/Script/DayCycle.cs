using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static bool isNight = false;
    [SerializeField] private GameObject inGameLights;

    void Start()
    {
        StartCoroutine(DelayDay(3));
    }

    private void Update()
    {
        if (gameObject.transform.rotation.x > 150)
        {
            Debug.LogError("Geceeeeeeee");
            inGameLights.SetActive(true);
            PickUpLightManager.pickupLights.SetActive(true);
            isNight = true;
        }

        if (transform.rotation.x < 135)
        {
            inGameLights.SetActive(false);
            PickUpLightManager.pickupLights.SetActive(false);
            isNight = false;
        }
    }

    public void RotateSun()
    {
        transform.DORotate(new Vector3(195, 750, 0), 1000f * Time.deltaTime)
            .SetEase(Ease.Linear).OnComplete(() => { StartCoroutine(DelayNight(20)); });
    }

    IEnumerator DelayNight(int hour)
    {
        yield return new WaitForSeconds(hour);
        transform.DOLocalRotate(new Vector3(45, 750, 0), 1000f * Time.deltaTime)
            .OnComplete(() => { StartCoroutine(DelayDay(30)); });
    }

    IEnumerator DelayDay(int hour)
    {
        yield return new WaitForSeconds(hour);
        RotateSun();
    }
}