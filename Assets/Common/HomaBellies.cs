using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomaGames.HomaBelly;

public class HomaBellies : MonoBehaviour
{
    public void Awake()
    {
        if (!HomaBelly.Instance.IsInitialized)
        {
            // Listen event for initialization
            Events.onInitialized += OnInitialized;
        }
        else
        {
            // Homa Belly already initialized
        }
    }
		
    private void OnDisable()
    {
        Events.onInitialized -= OnInitialized;
    }

    private void OnInitialized()
    {
        Debug.Log("HomaBelly");
    }
}
