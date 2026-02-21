using UnityEngine;
using System.Collections;

public class UI_RankBtn : MonoBehaviour {

    public GameObject RankView = null;
    
    bool bPush = false;
   
    void Start()
    {
    }
    
   
    void OnClick()
    {
        if (GameClient.instance.isVisitor == true) return;

        if (gameObject.name == "RankViewBtn")
        {
            
            if (LoadingBar.GetInstance().GetLock() == true)
                return;

            LoadingBar.GetInstance().SetLock(true);
            
            RankView.GetComponent<CRankView>().ResetRank();
            
            //연동 랭크
            if (GameClient.mNetwork == true)
                CNetwork.GetInstance().CS_RankInfo(SC_RankComplete);
            else
                SC_RankComplete();

        }
        else if (gameObject.name == "PopupBtn")
        {
            
            RankView.GetComponent<CRankView>().MoveWindow();
            //PopupUpBtn.SetActive(true);
        }
        

    }
    
    void OnPress()
    {
        if (GameClient.instance.isVisitor == true) return;
        //if (GameClient.instance.tutorial.isVisitor == true) return;
        bPush ^= true; 
        ButtonActive(gameObject, bPush);
    }

    void ButtonActive(GameObject obj, bool bPush)
    {
        
        if (bPush == true)
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(false);
            obj.transform.Find("PressButton").gameObject.SetActive(true);
        }
        else
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(true);
            obj.transform.Find("PressButton").gameObject.SetActive(false);
        }
    }


    void SC_RankComplete()
    {
        
        RankView.GetComponent<CRankView>().MoveWindow();
        gameObject.SetActive(false);
    }

   
}
