using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OfflineProgress : MonoBehaviour
{
    public float offlineRewardMoney;
    public GameObject OfflineRewardPanel;
    public GameObject offlineMoneyText;
    public static OfflineProgress Instance;
    public bool isLogin = true;

    /*
     * isLoaded == true ise offlinePnale'i göster
     * App Iconunu ekle
     * Oyun girişinde DMM logosunu ekle
     * 3k para veriyoruz onu geri al
     */

    void Awake()
    {
        isLogin = true;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void OfflinePanelControl()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN"))
        {
            DateTime lastLogIn = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN"));

            TimeSpan ts = DateTime.Now - lastLogIn;
            

            if (ts.TotalSeconds < 86400)
            {
                offlineRewardMoney =
                    AbbrevationUtility.RoundNumberLikeText((long)(GameDataManager.Instance.offlineProgressNum *
                                                                  (float)ts.TotalSeconds));
                offlineMoneyText.GetComponent<TextMeshProUGUI>().text =
                    AbbrevationUtility.AbbreviateNumber((long)offlineRewardMoney);
            }
            else
            {
                offlineRewardMoney = GameDataManager.Instance.offlineProgressNum * 86400;
            }
        }
        else
        {
            Debug.Log("First Login");
            isLogin = false;
        }

        PlayerPrefs.SetString("LAST_LOGIN", DateTime.Now.ToString());
    }

    public void OnOfflineReward()
    {
        GameDataManager.Instance.TotalMoney += (long)offlineRewardMoney;
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text =
            AbbrevationUtility.AbbreviateNumber(GameDataManager.Instance.TotalMoney);
        OfflineRewardPanel.SetActive(false);
    }

    public void OnOffine3MultipleReward()
    {

        long tempTotalMoney = Convert.ToInt64(PlayerPrefs.GetString("TotalMoney", 0.ToString()));
        tempTotalMoney += (long)OfflineProgress.Instance.offlineRewardMoney * 3;
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumberForTotalMoney((long)tempTotalMoney);
        OfflineProgress.Instance.OfflineRewardPanel.SetActive(false);
    }
}