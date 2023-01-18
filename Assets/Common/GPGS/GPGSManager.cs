using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
public class GPGSManager : MonoBehaviour

{
    public static GPGSManager Instance;
    public bool isAuth = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
#endif
    }
    
    private void OnSignInResult(SignInStatus signInStatus)
    {
#if UNITY_ANDROID
        if (signInStatus == SignInStatus.Success)
        {
            Debug.Log("Oldum");
            isAuth = true;
        }
        else
        {
            Debug.Log("Olmadim");
        }
#endif
    }
}