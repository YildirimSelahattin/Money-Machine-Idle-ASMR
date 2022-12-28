using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MoneyMove : MonoBehaviour
{
    public void MoneyMoveTruck()
    {
        transform.DOLocalMove(MachineManager.Instance._firstStep, .3f).OnComplete(
            () =>
            {
                transform.DOLocalMove(new Vector3(0, 0, 0), .3f);
            });
    }
}
