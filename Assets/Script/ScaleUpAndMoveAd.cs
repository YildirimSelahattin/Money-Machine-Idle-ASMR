using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleUpAndMoveAd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(new Vector3(0.15f, 0.15f, 0.15f), 0.5f).OnComplete(() => StartCoroutine(MoveUpAndDown()));

    }

    // Update is called once per frame

    public IEnumerator MoveUpAndDown()
    {
        yield return new WaitForSeconds(0.2f);
        transform.DOLocalMoveY(transform.localPosition.y - 1, 0.2f).OnComplete(() => transform.DOLocalMoveY(transform.localPosition.y + 1, 0.2f).OnComplete(()=>StartCoroutine(MoveUpAndDown())));
    }
}
