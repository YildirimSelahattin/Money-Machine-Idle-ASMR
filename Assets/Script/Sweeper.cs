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

    // Update is called once per frame
    void Update()
    {
       //CheckGrid(0);
    }

    public void CheckGrid(int index){
           int i=index;
           Vector3 myPos;
            if (GameDataManager.Instance.gridArray[i] > 0)
            {
                myPos = GameManager.Instance.gridParent.transform.GetChild(i).transform.GetChild(2).transform.position;
                myCube.transform.DOMove(myPos,1f).OnComplete(()=>{
                i++;
                if (i == GameDataManager.Instance.gridArray.Length||GameDataManager.Instance.gridArray[i] < 0)
                {
                    myCube.transform.DOMove(new Vector3(0,0,0),2f);
                }
                
                else{CheckGrid(i);}

                });
            }
        
    }

    public void RunLoop(){

        for (int i = 0; i < GameDataManager.Instance.gridArray.Length; i++)
        {
            CheckGrid(i);
        


        if (i+1 ==GameDataManager.Instance.gridArray.Length )
        {
            return;
        }

        }
    }
}
