using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTexMove : MonoBehaviour
{
    public Renderer bgRend;
    public float breake = 3;

void Update()
    {
        bgRend.materials[0].mainTextureOffset += new Vector2(breake * GameDataManager.Instance.beltSpeed * Time.deltaTime, 0);
        if (bgRend.materials.Length > 1)
        {
            bgRend.materials[1].mainTextureOffset += new Vector2(-breake * GameDataManager.Instance.beltSpeed * Time.deltaTime, 0);
        }
    }
}



