using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PickUpLightManager : MonoBehaviour
{
    public static GameObject pickupLights;

    private void Awake()
    {
        pickupLights = gameObject.transform.GetChild(4).gameObject;
    }

    private void Start()
    {
        if(DayCycle.isNight)
            pickupLights.SetActive(true);
        if(DayCycle.isNight == false)
            pickupLights.SetActive(false);
    }
}
