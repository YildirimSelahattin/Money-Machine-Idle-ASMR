using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sweeper : MonoBehaviour
{
    [SerializeField] GameObject myCube;

    void Start()
    {
        CheckGrid(0);
    }

    public void CheckGrid(int index)
    {
        int i = index;
        Vector3 myPos;
        if (GameDataManager.Instance.gridArray[i] > 0)
        {
            myPos = GameManager.Instance.gridParent.transform.GetChild(i).transform.GetChild(2).transform.position;
            myPos.y += 1f;
            myPos.z += 0.4f;
            myCube.transform.DOMove(myPos, 1f).OnComplete(() =>
            {
                i++;
                if (i == GameDataManager.Instance.gridArray.Length || GameDataManager.Instance.gridArray[i] < 0)
                {
                    myCube.transform.DOMove(new Vector3(0, 2, -20), 2f);
                }

                else
                {
                    CheckGrid(i);
                }
            });
        }
    }
}