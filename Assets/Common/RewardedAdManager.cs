using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using TMPro;

public class RewardedAdManager : MonoBehaviour
{
    public RewardedAd rewardedAd;
    public RewardedAd rewardedGridAd;
    public RewardedAd rewardedUpgradeButtonsAd;

    public static RewardedAdManager Instance;
    int nextLevelNumber;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        RequestRewarded();
        RequestGridRewarded();
        RequestRewardedUpgradeButtonsAd();
    }

    private void RequestRewarded()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-4384667521830956/2940198969";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        /*
                // Called when an ad request has successfully loaded.
                this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
                // Called when an ad request failed to load.
                this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
                // Called when an ad is shown.
                this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
                // Called when an ad request failed to show.
                this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
                // Called when the user should be rewarded for interacting with the ad.
                this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
                // Called when the ad is closed.
                this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        */
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    
    private void RequestGridRewarded()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-4384667521830956/7803844534";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedGridAd = new RewardedAd(adUnitId);
        // Called when an ad request has successfully loaded.
        this.rewardedGridAd.OnAdLoaded += HandleRewardedAdLoadedGrid;
        /*
                // Called when an ad request has successfully loaded.
                this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
                // Called when an ad request failed to load.
                this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
                // Called when an ad is shown.
                this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
                // Called when an ad request failed to show.
                this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
                // Called when the user should be rewarded for interacting with the ad.
                this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
                // Called when the ad is closed.
                this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        */
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedGridAd.OnUserEarnedReward += HandleUserEarnedGridReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedGridAd.LoadAd(request);
    }
    
    private void RequestRewardedUpgradeButtonsAd()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-4384667521830956/8192525642";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedUpgradeButtonsAd = new RewardedAd(adUnitId);

        /*
                // Called when an ad request has successfully loaded.
                this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
                // Called when an ad request failed to load.
                this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
                // Called when an ad is shown.
                this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
                // Called when an ad request failed to show.
                this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
                // Called when the user should be rewarded for interacting with the ad.
                this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
                // Called when the ad is closed.
                this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        */
        
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedUpgradeButtonsAd.OnUserEarnedReward += HandleUserEarnedUpgradeButtonReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedUpgradeButtonsAd.LoadAd(request);
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        long tempTotalMoney = Convert.ToInt64(PlayerPrefs.GetString("TotalMoney", 0.ToString())); 
        tempTotalMoney += (long)OfflineProgress.Instance.offlineRewardMoney * 3;
        UIManager.Instance.TotalMoneyText.GetComponent<TextMeshProUGUI>().text = AbbrevationUtility.AbbreviateNumberForTotalMoney((long)tempTotalMoney);
        OfflineProgress.Instance.OfflineRewardPanel.SetActive(false);
        
        RequestRewarded();
    }
    public void HandleRewardedAdLoadedGrid(object sender, EventArgs args)
    {
        if(UIManager.Instance != null)
        {
            StartCoroutine(UIManager.Instance.OpenGridAdButtons());
        }
    }
    public void HandleUserEarnedGridReward(object sender, Reward args)
    {
        GettingTouchManager.Instance.StartCoroutine(GettingTouchManager.Instance.GiveGridReward());
        RequestGridRewarded();
    }    
    
    public void HandleUserEarnedUpgradeButtonReward(object sender, Reward args)
    {
        if (UIManager.Instance.buttonIndex == 1)
            StartCoroutine( UIManager.Instance.RewardedBeltSpeedUpgradeButton());
        if (UIManager.Instance.buttonIndex == 2)
            StartCoroutine(UIManager.Instance.RewardedIncomeUpgradeButton());
        if (UIManager.Instance.buttonIndex == 3)
            StartCoroutine(UIManager.Instance.RewardedAdWorkerUpgradeButton());
        if (UIManager.Instance.buttonIndex == 4)
            StartCoroutine(UIManager.Instance.RewardedAddMachineButton());

        RequestRewardedUpgradeButtonsAd();
    }
    
    public void GridRewardAd()
    {
        if (this.rewardedGridAd.IsLoaded())
        {
            this.rewardedGridAd.Show();
        }
        RequestGridRewarded();
    }
    
    public void UpgradeButtonRewardAd()
    {
        if (this.rewardedUpgradeButtonsAd.IsLoaded())
        {
            this.rewardedUpgradeButtonsAd.Show();
        }
        RequestRewardedUpgradeButtonsAd();
    }

    public void MultipleOfflineProgressRewardAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            OfflineProgress.Instance.OfflineRewardPanel.SetActive(false);
        }
        else
        {
            RequestRewarded();
        }
    }
}