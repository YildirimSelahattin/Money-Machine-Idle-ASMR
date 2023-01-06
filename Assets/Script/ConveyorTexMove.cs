using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTexMove : MonoBehaviour
{
    public float bgSpeed = 0.1f;
    public Renderer bgRend;
    public static int breake = 1;

    void Start()
    {
        bgSpeed = 0.002f;
    }

    void Update()
    {
        bgRend.materials[0].mainTextureOffset += new Vector2(breake * bgSpeed * Time.deltaTime, 0);
        if (bgRend.materials.Length > 1)
        {
            bgRend.materials[1].mainTextureOffset += new Vector2(-breake * bgSpeed * Time.deltaTime, 0);
        }
    }
}



