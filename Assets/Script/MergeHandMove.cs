using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MergeHandMove : MonoBehaviour
{
    Vector3 startPos;
  
    void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(MergeMove());
    }

    public IEnumerator MergeMove()
    {
        yield return new WaitForSeconds(0.15f);
        transform.DOMoveX(GameManager.Instance.gridParent.transform.GetChild(1).transform.position.x, 1f).OnComplete(() =>
           transform.DOMoveX(startPos.x, 0.2f).OnComplete(() => StartCoroutine(MergeMove()))
        );
    }
}