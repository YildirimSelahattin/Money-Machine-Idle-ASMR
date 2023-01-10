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
        StartCoroutine(DelayDay(30));
    }
    
    public void RotateSun()
    {
        transform.DOLocalRotate(new Vector3(135, 750, 0), 650f * Time.deltaTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            inGameLights.SetActive(true);
            PickUpLightManager.pickupLights.SetActive(true);
            isNight = true;
            transform.DOLocalRotate(new Vector3(195, 750, 0), 350f * Time.deltaTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                StartCoroutine(DelayNight(20));
            });
        });
    }
    
    IEnumerator DelayNight(int hour)
    {
        yield return new WaitForSeconds(hour);
      
        transform.DOLocalRotate(new Vector3(0, 750, 0), 650f * Time.deltaTime).SetEase(Ease.Linear).OnComplete(() =>
        { 
            inGameLights.SetActive(false);
            PickUpLightManager.pickupLights.SetActive(false);
            isNight = false;
            transform.DOLocalRotate(new Vector3(45, 750, 0), 350f * Time.deltaTime)
            .OnComplete(() =>
            {
                StartCoroutine(DelayDay(30));
            });
        });
    }

    IEnumerator DelayDay(int hour)
    {
        yield return new WaitForSeconds(hour);
        RotateSun();
    }
}
