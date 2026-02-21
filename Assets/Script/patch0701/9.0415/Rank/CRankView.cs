using UnityEngine;
using System.Collections;

public class CRankView : MonoBehaviour {


    bool IsPlay = false;
    bool bRankPopup = false;
    Vector3 Startpos  = Vector3.zero ;

    public GameObject PopUpBtn;
    
    public GameObject MemberObj; 
    
    public CMemberBase[] MemberList;
    public UILabel DateLabel; 
    

    void Awake()
    {
        MemberList = MemberObj.GetComponentsInChildren<CMemberBase>();
    }


	// Use this for initialization
	void Start () {
        Startpos = transform.localPosition; 
	}
    
    public void ResetRank()
    {
        foreach (CMemberBase obj in MemberList)
        {
            obj.Reset();
        }
    }

    public void MoveWindow()
    {
        
        if (IsPlay == true) return;

        bRankPopup ^= true;

        GameClient.instance.mbRankPopUp = bRankPopup; 

        if (bRankPopup == true)
        {
             string date = System.DateTime.Now.ToString("hh:mmtt MMM.dd.yyyy");
             DateLabel.text = "Ranking(last update " + date + ")";
            
            StartCoroutine(RankWindow(Startpos.y, 500f));
        }
        else
        {
            StartCoroutine(RankWindow(500, Startpos.y));
        }
    }
        

    IEnumerator RankWindow(float from, float to)
    {
        float fReverseTime = 0.0f;
        IsPlay = true;

        while (true)
        {
            fReverseTime += Time.fixedDeltaTime;
            float t = fReverseTime / 0.3f;
            float posy = Mathf.Lerp(from, to, t);

            Vector3 pos = transform.localPosition;
            pos.y = posy;

            transform.localPosition = pos;
            yield return null;
            
            
            if ( t > 0.8f)
                PopUpBtn.SetActive(!bRankPopup);
            
            if (t >= 1.0f)
            {
                IsPlay = false;
                LoadingBar.GetInstance().SetLock(false);
                /*
                  if (bRankPopup == true)
                  {
                      gameObject.transform.Find("NormalButton").transform.localEulerAngles = new Vector3(0, 0, 180);
                      gameObject.transform.Find("PressButton").transform.localEulerAngles = new Vector3(0, 0, 180);
                  }
                  else
                  {
                      gameObject.transform.Find("NormalButton").transform.localEulerAngles = new Vector3(0, 0, 0);
                      gameObject.transform.Find("PressButton").transform.localEulerAngles = new Vector3(0, 0, 0);
                  }
                 */

                //ButtonActive(gameObject, bRankPopup);
                break;
            }
        }
    }

}
