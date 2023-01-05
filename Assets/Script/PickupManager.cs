using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance;
    [SerializeField] private GameObject pickupPosition;
    [SerializeField] private GameObject pickupTarget;
    [SerializeField] private GameObject pickupSpawner;
    [SerializeField] private GameObject pickupPrefab;
    private bool isPress = true;
    GameObject tempPickup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        tempPickup = Instantiate(pickupPrefab, pickupPosition.transform.position,quaternion.identity);
    }

    public void SellMoneyWithTruck()
    {
        if (isPress)
        {
            tempPickup.transform.DOLocalMove(pickupTarget.transform.position, 2f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Destroy(tempPickup);
                    tempPickup = Instantiate(pickupPrefab, pickupSpawner.transform.position,quaternion.identity);
                    StartCoroutine(DelayEnum(1));
                });
        }
    }
    
    IEnumerator DelayEnum(float time)
    {
        yield return new WaitForSeconds(time);
        tempPickup.transform.DOLocalMove(pickupPosition.transform.position, 1f).SetEase(Ease.Linear);
        isPress = true;
    }
}