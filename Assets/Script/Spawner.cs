using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private ObjectPooling _pool;
    private Vector3 _spwanPos;
    private Vector3 _endPos;
    [SerializeField] private GameObject _endPosObject;
    
    void Start()
    {
        _spwanPos = gameObject.transform.position;

        _endPos = _endPosObject.GetComponent<Vector3>();
        
        // Havuzu oluştur ve 500 obje ile doldur
        _pool = new ObjectPooling( prefab );
        _pool.GetPooledObject( 10 );
 
        for( int i = 0; i < 10; i++ )
        {
            StartCoroutine( InstatiateAndDestroyWithPool() );
        }
    }
 
    IEnumerator InstatiateAndDestroyWithPool()
    {
        while( true )
        {
            _spwanPos = gameObject.transform.position;
 
            // Havuzdan obje çekip konumunu değiştir
            GameObject moneyPrefab = _pool.SetPooledObject();
            moneyPrefab.transform.position = _spwanPos;
 
            // 1 saniye bekle
            yield return new WaitForSeconds( 2f );
 
            // Objeyi havuza geri yolla
            _pool.AddPoolObject( moneyPrefab );
        }
    }

    private void Update()
    {
        gameObject.transform.DOMove(_endPos, 0.5f);
    }
}
