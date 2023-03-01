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
    public TextMeshProUGUI offlineMoneyText;
    public static OfflineProgress Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void OfflinePanelControl()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN_NEW"))
        {
            DateTime lastLogIn = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN_NEW"));
            OfflineProgress.Instance.OfflineRewardPanel.SetActive(true);
            TimeSpan ts = DateTime.Now - lastLogIn;
            
            Debug.Log(ts.TotalSeconds);

            if (ts.TotalSeconds < 86400)
            {
                offlineRewardMoney = AbbrevationUtility.RoundNumberLikeText((long)(GameDataManager.Instance.offlineProgressNum * (float)ts.TotalSeconds));
                Debug.Log(offlineRewardMoney);
                offlineMoneyText.text = AbbrevationUtility.AbbreviateNumber((long)offlineRewardMoney);
            }
            else
            {
                offlineRewardMoney = GameDataManager.Instance.offlineProgressNum * 86400;
            }
        }
        else
        {
            Debug.Log("First Login");
            OfflineRewardPanel.SetActive(false);
        }
        
        PlayerPrefs.SetString("LAST_LOGIN_NEW", DateTime.Now.ToString());
    }
    
    public void OnOfflineReward()
    {
        //InterstitialAdManager.Instance.ShowInterstitial();
        GameDataManager.Instance.TotalMoney +=(long) offlineRewardMoney;
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);
        OfflineRewardPanel.SetActive(false);
    }

    public void OnOffine3MultipleReward()
    {
        RewardedAdManager.Instance.MultipleOfflineProgressRewardAd();
    }
}