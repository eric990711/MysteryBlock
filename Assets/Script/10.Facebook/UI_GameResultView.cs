using UnityEngine;
using System.Collections;

public class UI_GameResultView : MonoBehaviour
{

    public UILabel pTicket;
    public UILabel pBonus;
    public UILabel pTotalTicket;
    public UILabel pFBText;

    long PrevTicket = 0;
    float UserTicket = 0;

    public bool bOnResultView = false;

    void Awake()
    {
     
    }
    
	// Use this for initialization
	void Start () {

	}

    //결과창
    public void OpenResultWnd(float ticket)
    {
        bOnResultView = true;
        
        UserTicket = System.Convert.ToUInt64(ticket);
        gameObject.SetActive(true);

        // sdh 20140704 팝업
        //GameClient.instance.OpenWebPopupView(MysteryMgr.Instance.webPopupView , true );
        //CPopupMgr.instance.OpenWebPopupView(MysteryMgr.Instance.webPopupView, true);
        

    }

    
    



    void OnEnable()
    {
        
        TweenScale scale = GetComponent<TweenScale>();

        //20140810 ljw 쉐어

//         if (GameClient.instance.shareCnt > 2)
//             pFBText.text = TextManager.GetInstance().GetText(emString.FBSHARETEXT);
//         else
//             pFBText.text = TextManager.GetInstance().GetText(emString.FBFULLREWARD);
        //pFBText.text = TextManager.GetInstance().GetText(emString.FBSHARETEXT);
//         string kor = "페이스북에 WIN 점수를 공유하시고 \n보상을 받아가세요";
//         string eng = "Share at facebook your WIN \nYou can get rewards.";
// 
//         if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
//         {
//             pFBText.text = kor;
//         }
//         else
//         {
//             pFBText.text = eng;
//         }

        pFBText.text = string.Format(TextManager.GetInstance().GetText(emString.FBSHARETEXT));

        if (scale)
        {
            scale.Reset();
            scale.duration = 0.2f;
            scale.enabled = true;
        }
        
        pBonus.text = ""; 
        pTotalTicket.text = "";
        pTicket.text = "";

        //PrevTicket = MysteryMgr.Instance.mPrevUserTicket;
        //UserTicket = GameClient.instance.mUserTicket;

        StartCoroutine(EventResult());
    }


    IEnumerator EventResult()
    {

        float CurTicket = UserTicket;

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
        RecountWinText.Instance.enabled = false;

        bOnResultView = false;

        GameClient.instance.mMysteryState = MysteryState.Stay;
        
        gameObject.SetActive(false);
    }

    void ResultFacebook()
    {
        //GameClient.mGameState
        
        FacebookMgr.GetInstance().CallFBFeed( System.Convert.ToInt32(UserTicket) , emFacebookMsgMode.JACKPOT);

        bOnResultView = false;
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
