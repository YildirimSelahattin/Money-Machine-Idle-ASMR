using System.Collections.Generic;
using UnityEngine;
 
public class ObjectPooling
{
    private GameObject prefab;
    private Stack<GameObject> objeHavuzu = new Stack<GameObject>();
 
    public ObjectPooling( GameObject prefab )
    {
        this.prefab = prefab;
    }
 
    public void GetPooledObject( int miktar )
    {
        for( int i = 0; i < miktar; i++ )
        {
            GameObject obje = Object.Instantiate( prefab );
            AddPoolObject( obje );
        }
    }
 
    public GameObject SetPooledObject()
    {
        if( objeHavuzu.Count > 0 )
        {
            GameObject obje = objeHavuzu.Pop();
            obje.gameObject.SetActive( true );
 
            return obje;
        }
        return Object.Instantiate( prefab );
    }
 
    public void AddPoolObject( GameObject obje )
    {
        obje.gameObject.SetActive( false );
        objeHavuzu.Push( obje );
    }
}