using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdManager : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    public void Start()
    {
        this.RequestBanner();
    }

    private void RequestBanner()
    {

#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-4384667521830956/5916748189"; //ca-app-pub-4384667521830956/2742164828
#else
        string adUnitId = "unexpected_platform";
#endif

        //AdSize adSize = new AdSize(320, 100);
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }
}