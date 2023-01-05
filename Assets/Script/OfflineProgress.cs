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

    void Start()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN"))
        {
            DateTime lastLogIn = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN"));

            TimeSpan ts = DateTime.Now - lastLogIn;
            
            Debug.Log(ts.TotalSeconds);

            if (ts.TotalSeconds < 86400)
            {
                offlineRewardMoney = GameDataManager.Instance.offlineProgressNum * (float)ts.TotalSeconds;
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
        float tempTotalMoney = PlayerPrefs.GetFloat("TotalMoney", 0);
        tempTotalMoney += offlineRewardMoney;
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(tempTotalMoney);
        OfflineRewardPanel.SetActive(false);
    }

    public void OnOffine3MultipleReward()
    {
        float tempTotalMoney = PlayerPrefs.GetFloat("TotalMoney", 0);
        tempTotalMoney += offlineRewardMoney * 3;
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumber(tempTotalMoney);
        OfflineRewardPanel.SetActive(false);
    }
}
