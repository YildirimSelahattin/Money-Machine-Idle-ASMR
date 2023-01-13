using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TappingHandMove : MonoBehaviour
{
    [SerializeField] float yOffset;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(TapMove());
    }
    // Update is called once per frame
    public IEnumerator TapMove()
    {
        yield return new WaitForSeconds(0.15f);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z), 0.1f).OnComplete(() =>
        {
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), 0.4f).OnComplete(() => StartCoroutine(TapMove()));
        });

    }
}