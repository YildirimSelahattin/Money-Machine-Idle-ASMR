using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private MachinesProcess Instance;
    [SerializeField] private GameManager moneyPrefab;
    
    public GameObject prefab;
 
    private Stack<GameObject> objeHavuzu = new Stack<GameObject>();
 
    void Start()
    {
        StartCoroutine( SurekliObjeOlusturVeYokEt() );
    }
 
    IEnumerator SurekliObjeOlusturVeYokEt()
    {
        while( true )
        {
            Vector3 konum = Random.insideUnitSphere * 3f;
 
            // Havuzdan obje çekip konumunu değiştir
            GameObject obje = HavuzdanObjeCek();
            obje.transform.position = konum;
 
            // 1 saniye bekle
            yield return new WaitForSeconds( 1f );
 
            // Objeyi havuza geri yolla
            HavuzaObjeEkle( obje );
        }
    }
 
    GameObject HavuzdanObjeCek()
    {
        // Havuzda obje var mı kontrol et
        if( objeHavuzu.Count > 0 )
        {
            // Havuzdaki en son objeyi çek
            GameObject obje = objeHavuzu.Pop();
 
            // Objeyi aktif hale getir
            obje.gameObject.SetActive( true );
 
            // Objeyi döndür
            return obje;
        }
 
        // Havuz boş, mecburen yeni bir obje Instantiate et
        return Instantiate( prefab );
    }
 
    void HavuzaObjeEkle( GameObject obje )
    {
        // Objeyi inaktif hale getir (böylece obje artık ekrana çizilmeyecek ve objede
        // Update vs. fonksiyonlar varsa, bu fonksiyonlar obje havuzdayken çalıştırılmayacak)
        obje.gameObject.SetActive( false );
 
        // Objeyi havuza ekle
        objeHavuzu.Push( obje );
    }
}
