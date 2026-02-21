using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
public class LobbyMgr : MonoBehaviour
{

    public UILabel JackPotLabel;
    public UILabel NeedCoinLabel;
    public UILabel MyCoinLabel;
    public UILabel MyTicketLabel;
    public UISprite GamelevelSprite;

    public CRankView RankView;
    public GameObject webView;
    public GameObject webPopupView;    //sdh 20140704 팝업작업 

    public TutorialLobby tutoLobby = null;
    public TutorialModelController tutoModelCon = null;

    public static LobbyMgr Instance
    {
        get
        {
            if (minstance == null)
            {
                minstance = FindObjectOfType<LobbyMgr>();
            }
            return minstance;
        }
    }
    private static LobbyMgr minstance = null;

    private int GO_NUM;

    //#define UUID_PASSWD             @"Z45dFtn42Ow56Cg7fhmb"
    // Use this for initialization
    void Start()
    {
        //CNetwork.GetInstance().CS_JoinLoginUser(Global.USER_ID , Global.USER_PW);
        //CNetwork.GetInstance().CS_Login("test32@email.com" , "Z45dFtn42Ow56Cg7fhmb" );
        GameClient.mGameState = GameState.Lobby;

        Global.USER_GONUM = "";

        //연동 유져정보 얻기
        if (GameClient.mNetwork == true)
        {

            LoadingBar.GetInstance().SetLock(true);

            CNetwork.GetInstance().CS_UserData();
            CNetwork.GetInstance().CS_EnterLobby(UpdateMachineComplete);
            //
            // sdh 20140704 팝업
            //

            CNetwork.GetInstance().CS_BeforePopup(UpdateBeforePopup);

            //CNetwork.GetInstance().CS_Tutorial(UpdateTutorial);

        }
        else
        {
            UpdateUserInfo();
            UpdateMachine();
            tutoLobby.gameObject.SetActive(true);
        }
    }

    void UpdateTutorial()
    {
        Debug.Log("UpdateTutorial");
    }


    //
    // sdh 20140704 팝업
    //
    public void UpdateBeforePopup()
    {
        CPopupMgr.instance.OpenWebPopupView(LobbyMgr.Instance.webPopupView);
    }

    public void UpdateUserInfo()
    {
        MyCoinLabel.text = GameClient.instance.mUserCoin.ToString();
        MyTicketLabel.text = GameClient.instance.mUserTicket.ToString();
    }

    //
    // 유저 정보 얻기
    //
    public void UpdateMachineComplete()
    {
        LoadingBar.GetInstance().SetLock(false);

        UpdateUserInfo();
        UpdateMachine();
        tutoLobby.gameObject.SetActive(true);
    }

    public void UpdateMachine()
    {

        MachineData mData = GameClient.instance.GetMachineData();

        JackPotLabel.text = mData.theJackPot.ToString();
        NeedCoinLabel.text = mData.theNeedCoin.ToString();

        switch (mData.MachineGrade)
        {
            case _enMachineGrade.starter:
                GamelevelSprite.spriteName = "Img_GameLevel_Stater";
                break;
            case _enMachineGrade.challenge:
                GamelevelSprite.spriteName = "Img_GameLevel_Challenge";
                break;
            case _enMachineGrade.surprise:
                GamelevelSprite.spriteName = "Img_GameLevel_Surprise";
                break;
            case _enMachineGrade.miracle:
                GamelevelSprite.spriteName = "Img_GameLevel_Miracle";
                break;
            case _enMachineGrade.adventure:
                GamelevelSprite.spriteName = "Img_GameLevel_Adventure";
                break;
            case _enMachineGrade.gevent:
                GamelevelSprite.spriteName = "Img_GameLevel_Event";
                break;
        }


    }

}
