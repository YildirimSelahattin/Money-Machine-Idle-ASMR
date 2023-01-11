using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLManager : MonoBehaviour
{
    public void GoPrivacyURL()
    {
        Application.OpenURL("https://digimindmarket.com/privacy.html#");
    }

    public void GoServiceURL()
    {
        Application.OpenURL("https://digimindmarket.com/service.html");
    }

    public void GoDMM()
    {
        Application.OpenURL("https://digimindmarket.com/");
    }
}