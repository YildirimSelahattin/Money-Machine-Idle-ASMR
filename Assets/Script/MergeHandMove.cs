using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MergeHandMove : MonoBehaviour
{
    Vector3 startPos;
    // Start is called before the first frame update
    void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(MergeMove());
    }

    public IEnumerator MergeMove()
    {
        yield return new WaitForSeconds(0.15f);
        transform.DOMove(GameManager.Instance.gridParent.transform.GetChild(1).transform.position, 1f).OnComplete(() =>
           transform.DOMove(startPos, 0.2f).OnComplete(() => StartCoroutine(MergeMove()))
        );
    }
}