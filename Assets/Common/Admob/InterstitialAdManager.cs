/*using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InterstitialAdManager : MonoBehaviour
{
    public InterstitialAd interstitialButtons;
    public static InterstitialAdManager Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        RequestInterstitial();
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4384667521830956/4444852325";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitialButtons = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitialButtons.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialButtons.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialButtons.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        this.interstitialButtons.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialButtons.LoadAd(request);
    }
    // if there is no internet and 
    
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args);
        RequestInterstitial();
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        // only sound side 
        /*
        if (GameManager.Instance.matchRate >= 69)
        {
            if (GameDataManager.Instance.playSound == 1)
            {
                GameObject sound = new GameObject("sound");
                sound.AddComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.winSound);
                Destroy(sound, GameDataManager.Instance.winSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
            }
        }
        else
        {
            if (GameDataManager.Instance.playSound == 1)
            {
                GameObject sound = new GameObject("sound");
                sound.AddComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.loseSound);
                Destroy(sound, GameDataManager.Instance.loseSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
            }
        }
        
    }
    public void ShowInterstitial()
    {
        if (interstitialButtons.IsLoaded())
        {
            interstitialButtons.Show();
        }
        RequestInterstitial();
    }
}*/