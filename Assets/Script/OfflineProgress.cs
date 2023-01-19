using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OfflineProgress : MonoBehaviour
{
    public float offlineRewardMoney;
    public GameObject OfflineRewardPanel;
    public GameObject offlineMoneyText;
    public static OfflineProgress Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN"))
        {
            DateTime lastLogIn = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN"));

            TimeSpan ts = DateTime.Now - lastLogIn;
            
            Debug.Log(ts.TotalSeconds);

            if (ts.TotalSeconds < 86400)
            {
                offlineRewardMoney = GameDataManager.Instance.GetOnly1DigitAfterPoint( GameDataManager.Instance.offlineProgressNum * (float)ts.TotalSeconds);
                Debug.Log(offlineRewardMoney);
                offlineMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(offlineRewardMoney);
            }
            else
            {
                offlineRewardMoney = GameDataManager.Instance.offlineProgressNum * 86400;
            }
            OfflineRewardPanel.SetActive(true);
        }
        else
        {
            Debug.Log("First Login");
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LAST_LOGIN", DateTime.Now.ToString());
    }

    public void OnOfflineReward()
    {
        OfflineRewardPanel.SetActive(false);
        InterstitialAdManager.Instance.ShowInterstitial();
        
        GameDataManager.Instance.TotalMoney += GameDataManager.Instance.GetOnly1DigitAfterPoint(offlineRewardMoney);
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumberForTotalMoney(GameDataManager.Instance.GetOnly1DigitAfterPoint(GameDataManager.Instance.TotalMoney));
        OfflineRewardPanel.SetActive(false);
    }

    public void OnOffine3MultipleReward()
    {
        RewardedAdManager.Instance.MultipleOfflineProgressRewardAd();
    }
}
