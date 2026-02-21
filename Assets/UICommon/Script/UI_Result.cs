using UnityEngine;
using System.Collections;

public class UI_Result : MonoBehaviour {

    public UILabel pTicket;
    public UILabel pBonus;
    public UILabel pTotalTicket;
    public UILabel pFBText;

    long PrevTicket = 0;
    long UserTicket = 0;

    public GameObject FacebookBtn;
    public GameObject OkBtn; 
    


    void Awake()
    {
     
        if (GameApplication.GetInstance().m_DevFacebook == false)
        {
            if (FacebookBtn != null)
            { 
                FacebookBtn.SetActive(false);
            }
            
            OkBtn.transform.localPosition = new Vector3(6, -55, 0);
            
        }

        

    }
    
	// Use this for initialization
	void Start () {

	}

    void OnEnable()
    {
        
        TweenScale scale = GetComponent<TweenScale>();

        //20140810 ljw 쉐어

//         string kor = "페이스북에 티켓 점수를 공유하시고 \n보상을 받아가세요";
//         string eng = "Share at facebook your score \nYou can get rewards.";
// 
//         if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
//         {
//             pFBText.text = kor;
//         }
//         else
//         {
//             pFBText.text = eng;
//         }


        if (scale)
        {
            scale.Reset();
            scale.duration = 0.2f;
            scale.enabled = true;
        }
        
        pBonus.text = ""; 
        pTotalTicket.text = "";
        pTicket.text = "";


        PrevTicket = MysteryMgr.Instance.mPrevUserTicket;
        UserTicket = GameClient.instance.mUserTicket;

        StartCoroutine(EventResult());

    }


    IEnumerator EventResult()
    {

        long CurTicket = UserTicket;

        StartCoroutine(EffTicketText(0.0f, CurTicket , pTicket));
        
        yield return new WaitForSeconds(1f);
        
        if (MysteryMgr.Instance.mUseCoinx2 == true)
        {
            pBonus.text = "x 2";
        }
        else
        {
            pBonus.text = "x 1";
        }
        
        yield return new WaitForSeconds(1f);


        StartCoroutine(EffTicketText(0.0f, UserTicket , pTotalTicket));
        

    }
    
    //게임 로비로 이동
    void ResultOK()
    {
        if (LoadingBar.GetInstance().GetLock() == true) return;

        GameClient.mGameState = GameState.Lobby;
        GameClient.instance.LoadingScene();
    }

    void ResultFacebook()
    {
        //GameClient.mGameState

        if (LoadingBar.GetInstance().GetLock() == true) return;

        FacebookMgr.GetInstance().CallFBFeed( System.Convert.ToInt32(UserTicket) , emFacebookMsgMode.RESULT);
    }
    

    IEnumerator EffTicketText(float from, float to, UILabel label)
    {
        bool bLoop = true;

        int Result = 0;
        float t = 0f;
        float fNowtime = 0.0f;
        
        while (bLoop)
        {
            fNowtime += Time.deltaTime;
            t = fNowtime / 0.5f;
            Result = Mathf.RoundToInt(Mathf.Lerp(from, to, t));

            label.text = MPUtil.MoneyFormatString(Result.ToString());

            if (t >= 1.0f) break; 

            yield return null;
        }
    }
}
