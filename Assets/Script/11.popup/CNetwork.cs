//****************************************
//                              2014.03.05
//               iamboss
//
//****************************************


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;


public class CNetwork : MonoBehaviour
{

    CWww m_cWww;

    static private CNetwork minstance;
    public static CNetwork GetInstance()
    {
        if (minstance == null)
        {
            minstance = FindObjectOfType<CNetwork>();

            if (minstance == null)
            {
                minstance = new GameObject("CNetwork").AddComponent<CNetwork>();
                //m_instance = Instantiate(m_instance) as GameManager;
            }
        }
        return minstance;
    }



    private EventHandler pRankComplete; //랭킹
    private EventHandler pPlayComplete; //플래이
    private EventHandler pFinishComplete;   //게임이 끝나면
    private EventHandler pLoginComplete;    //로그인 성공(ios 디바이스 로그인)
    private EventHandler pUserDataComplete; //유져정보 티켓 코인 등등
    private EventHandler pEnterLobbyComplete;   //머신정보 가져옴
    private EventHandler pSaveTicketComplete;   //티켓수령 쓰지않는다. 
    private EventHandler pTutorialComplete; //. 튜토리얼 완료
    private EventHandler pBeforePopupComplete; //. 튜토리얼 완료
    private EventHandler pSendShareComplete; //20140810 ljw 쉐어

    private EventHandler pAfterPopupComplete; //20140919 ljw 플링코 전용 팝업



    //GameApplication 호출한다. 처음실행 함. 다른곳에서 하면안됨  
    public void Init()
    {
        // 안드로이드에서 타 플랫폼에 실행하는것 체크함.. 
        // 타플랫폿에서 들어올때 직접실행때        
#if UNITY_ANDROID
        CAndroidManager.GetInstance().Init();
#elif UNITY_IOS
        iOSManager.GetInstance().Init();
#endif

        //CAndroidManager.GetInstance().SetUp(SetUp, PARSETYPE._STRING);
    }

    void Awake()
    {
        Object.DontDestroyOnLoad(this);
        m_cWww = CWww.GetInstance();
    }

    public void PlatFormDownload()
    {
        Application.OpenURL(GameApplication.GetInstance().StoreName());
        Application.Quit();
    }


    public void GoPlatform()
    {

#if UNITY_ANDROID
        CAndroidManager.GetInstance().goPlatform();
#elif UNITY_IOS
        iOSManager.GetInstance().GoPlayform();
#endif

    }

    public void StartGame()
    {

        Debug.Log("StartGame");

        if (GameClient.mNetwork == false)
        {
            // 오프라인/에디터 모드: 바로 씬 전환
            SceneManager.LoadScene("LobbyStage");
            return;
        }

#if UNITY_ANDROID
        CAndroidManager.GetInstance().startGame(ANstartGame);
#elif UNITY_IOS
        iOSManager.GetInstance().StartGame();
#endif

    }


    void startgame()
    {
        SceneManager.LoadScene("LobbyStage");
    }




    public void Logout()
    {
        //기본정보 리셋
#if UNITY_ANDROID
        CAndroidManager.GetInstance().logout(null);
#elif UNITY_IOS
        iOSManager.GetInstance().LogoutGame();
#endif

        GameApplication.GetInstance().Logout();
        GameClient.mGameState = GameState.Login;
        GameClient.instance.LoadingScene();

    }

    //안드로이드만 실행
    public void GetUserInfo()
    {
#if UNITY_ANDROID
        CAndroidManager.GetInstance().getUserInfo(ANGetUserInfo);
#endif
    }


    // Android 
    // 라이브러리 에서 다이렉트실행또는 플랫폼으로 들어오는것 체크
    //
    void SetUp(object obj)
    {


        switch ((string)obj)
        {
            case "doPlay":
#if UNITY_ANDROID
                CAndroidManager.GetInstance().getUserInfo(ANGetUserInfo);
#endif
                break;
            default:

                if (GameApplication.GetInstance().m_isTerms == false)
                {
                    GameClient.mGameState = GameState.Login;
                    SceneManager.LoadScene("LoadingScene");
                }
                else
                {
                    StartGame();
                }
                break;
        }


    }

    void ANGetUserInfo(object obj)
    {
        string[] szBuf;

        szBuf = ((string)obj).Split('|');


        //Gonum 플랫폼에서 버신번호를 넘겨줬기떄문에 바로 미스터리게임으로 보낸다. 

        switch (szBuf[0])
        {
            case "Success":
                var dict = Json.Deserialize(szBuf[1]) as IDictionary<string, object>;
                Global.USER_GONUM = (string)dict["GO_NUM"];
                GameClient.mGameState = GameState.Mystery;
                SceneManager.LoadScene("LoadingScene");
                break;
            default: break;
        }
    }



    //안드로이드만 적용
    void ANstartGame(object obj)
    {
        string szBuf = (string)obj;

        if (GameApplication.GetInstance().m_isTerms == false)
        {
            GameApplication.GetInstance().m_isTerms = true;
            GameApplication.GetInstance().SaveAppInfo();
        }

        GameClient.mGameState = GameState.Lobby;
        SceneManager.LoadScene("LoadingScene");

    }



    void SCDataInfo(string Packet, NetType protocol)
    {
        Debug.Log("~~~~~~SCDataInfo~~~~~ NetType : " + protocol);

        switch (protocol)
        {
            case NetType.JOINLOGIN: SC_JoinLogin(Packet); break;
            case NetType.LOGIN: SC_Login(Packet); break;
            case NetType.TERMPAGE: SC_TermPage(Packet); break;
            case NetType.USERDATA: SC_GetUser(Packet); break;
            case NetType.FINISH: SC_Result(Packet); break;
            case NetType.PLAY: SC_Play(Packet); break;
            case NetType.LOBBY_RANK: SC_GameRankList(Packet); break;
            case NetType.LOBBY_ROOM: SC_GameRoomList(Packet); break;
            case NetType.TUTORIAL: SC_Tutorial(Packet); break;
            case NetType.BEFOREPOPUP: SC_BeforePopup(Packet); break;    // sdh 20140704 팝업
            case NetType.SENDSHARE: SC_SendShare(Packet); break;//20140810 ljw 쉐어
            case NetType.AFTERPOPUP: SC_AfterPopup(Packet); break; //20140919 ljw 플링코 전용 팝업

                
        }

        Debug.Log(protocol+" / " +Packet);
    }

    /// Client to Server
    //
    // 로그인
    public void CS_JoinLoginUser(string szAccount, string szPassword, EventHandler pEventHandler = null)
    {
        pLoginComplete = pEventHandler;

        Debug.Log("CS_JoinLoginUser ID " + szAccount + "PW " + szPassword);

#if UNITY_ANDROID
        CAndroidManager.GetInstance().loginUser(ANloginUser, szAccount, szPassword);
#elif UNITY_IOS
        StartCoroutine(m_cWww.JoinLoingUser(szAccount, szPassword, SCDataInfo));
#endif

    }


    private EventHandler pCheckPlatformComplete;
    public void CheckPlatform(EventHandler pEventHandler = null)
    {
        pCheckPlatformComplete = pEventHandler;

#if UNITY_ANDROID
        CAndroidManager.GetInstance().checkPlatform();
#elif UNITY_IOS
        iOSManager.GetInstance().CheckPlatform();
#endif
    }


    public void CS_Login(string szAccount, string szPassword, EventHandler pEventHandler = null)
    {
        pLoginComplete = pEventHandler;

        Debug.Log("CS_Login ID " + szAccount + "PW " + szPassword);

#if UNITY_ANDROID
        CAndroidManager.GetInstance().loginUser(ANloginUser, szAccount, szPassword);
#elif UNITY_IOS
        StartCoroutine(m_cWww.loginUser(szAccount, szPassword, SCDataInfo));
#endif

    }

    //Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

    public void CS_CoinCharge(string unum, string coin, EventHandler pEventHandler = null)
    { StartCoroutine(m_cWww.CoinTapJoy(unum, coin, SCDataInfo)); }
    public void CS_TermPage(string szAccount, string szPassword, EventHandler pEventHandler = null)
    { StartCoroutine(m_cWww.TermsPageData(szAccount, szPassword, SCDataInfo)); }

    public void CS_UserData(EventHandler pEventHandler = null)
    {

        Debug.Log("CS_USERDATA");
        pUserDataComplete = pEventHandler;
        // Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
#if UNITY_ANDROID
        CAndroidManager.GetInstance().getUserData(ANgetUserData);
#elif UNITY_IOS
        StartCoroutine(m_cWww.UserData(SCDataInfo));
#endif

    }

    public void CS_EnterLobby(EventHandler pEventHandler = null)
    {
        Debug.Log("CSENTERLOBBY");
        pEnterLobbyComplete = pEventHandler;




#if UNITY_ANDROID
        CAndroidManager.GetInstance().getMachineList(ANgetmachinelist);
#elif UNITY_IOS
        StartCoroutine(m_cWww.readGameRoomListByGameNum(SCDataInfo));
#endif

    }

    public void CS_RankInfo(EventHandler pEventHandler = null)
    {
        //Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        pRankComplete = pEventHandler;

#if UNITY_ANDROID
        string gonum = GameClient.instance.GetMachineData().theNumber;
        CAndroidManager.GetInstance().setGame(gonum);
        CAndroidManager.GetInstance().getRank(ANgetRank);
#elif UNITY_IOS
        StartCoroutine(m_cWww.readGameRankListByGameOptionNum(SCDataInfo));
#endif

    }

    //20140821 ljw 무료게임추가
    public void CS_Play(PLAYTYPE type, EventHandler pEventHandler = null)
    {
        Debug.Log("CS_Play");
        pPlayComplete = pEventHandler;
        //GameClient.instance.isIngame = false;


        string[] playtypeStr = { "default", "double", "bonus" };
#if UNITY_ANDROID
        string gonum = GameClient.instance.GetMachineData().theNumber;
        CAndroidManager.GetInstance().playGame(ANplayGame, playtypeStr[(int)type], gonum);
#elif UNITY_IOS
        StartCoroutine(m_cWww.playGame(type, SCDataInfo));
#endif

    }

    public void CS_Tutorial(EventHandler pEventHandler = null)
    {
        pTutorialComplete = pEventHandler;
        GameClient.instance.mbTutorial = true;
        PlayerPrefs.SetInt("Tutorial", System.Convert.ToInt32(GameClient.instance.mbTutorial));
        PlayerPrefs.Save();
        Debug.Log("CS_Tutorial");
#if UNITY_ANDROID
        CAndroidManager.GetInstance().tutorial(ANtutorial);
#elif UNITY_IOS
        StartCoroutine(m_cWww.tutorialFinish(SCDataInfo));
#endif
    }

    public void CS_Finish(string reward_ticket, EventHandler pEventHandler = null)
    {
        pFinishComplete = pEventHandler;

        Debug.Log("CS_Finish " + reward_ticket);
#if UNITY_ANDROID
        CAndroidManager.GetInstance().finishGame(ANfinishGame, long.Parse(reward_ticket.ToString()));
#elif UNITY_IOS
        StartCoroutine(m_cWww.finishGame(reward_ticket, SCDataInfo));
#endif


    }

    public void CS_Finish2(string reward_ticket, EventHandler pEventHandler = null)
    {
        pFinishComplete = pEventHandler;

        Debug.Log("CS_Finish2 " + reward_ticket);
#if UNITY_ANDROID
        CAndroidManager.GetInstance().finishGame2(ANfinishGame, long.Parse(reward_ticket.ToString()));
#elif UNITY_IOS
        StartCoroutine(m_cWww.finishGame2(reward_ticket, SCDataInfo));
#endif
    }

    public void CS_SaveTicket(EventHandler pEventHandler = null)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        pSaveTicketComplete = pEventHandler;

#if UNITY_ANDROID
        CAndroidManager.GetInstance().getSaveTicket(ANgetSaveTicket);
#elif UNITY_IOS
        StartCoroutine(m_cWww.SaveTicket(SCDataInfo));
#endif

    }

    //
    // sdh 20140704 팝업
    //
    public void CS_BeforePopup(EventHandler pEventHandler = null)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        pBeforePopupComplete = pEventHandler;

#if UNITY_ANDROID
        CAndroidManager.GetInstance().getBeforePopup(ANgetBeforePopup);
#elif UNITY_IOS
        StartCoroutine(m_cWww.BeforePopup(SCDataInfo));
#endif

    }

    public void CS_AfterPopup(string reward_ticket, EventHandler pEventHandler = null)
    {
        pAfterPopupComplete = pEventHandler;

        Debug.Log("CS_Afterpopup " + reward_ticket);

#if UNITY_ANDROID
        CAndroidManager.GetInstance().getAfterPopup(ANgetAfterPopup, long.Parse(reward_ticket.ToString()));
#elif UNITY_IOS
        StartCoroutine(m_cWww.GetAfterPopup(reward_ticket, SCDataInfo));
#endif

    }

    //20140810 ljw 쉐어
    public void CS_SendShare(EventHandler pEventHandler = null)
    {
        Debug.Log("CS_SendShare");

        pSendShareComplete = pEventHandler;


#if UNITY_ANDROID
        CAndroidManager.GetInstance().sendShare(ANgetSendShare);

#elif UNITY_IOS
        StartCoroutine(m_cWww.SendShare(SCDataInfo));
#endif

    }



    //Android
    void ANloginUser(object obj)
    {
        string[] szBuf;
        szBuf = ((string)obj).Split('|');
        switch (szBuf[0])
        {
            case "Success": SC_Login(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }
    }




    void ANgetUserData(object obj)
    {
        string[] szBuf;

        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_GetUser(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }
    }

    void ANgetmachinelist(object obj)
    {
        string[] szBuf;

        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_GameRoomList(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }
    }

    void ANfinishGame(object obj)
    {
        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');


        switch (szBuf[0])
        {
            case "Success": SC_Result(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }

    }

    void ANgetSaveTicket(object obj)
    {

        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_SaveTicket(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }

    }

    //
    // sdh 20140704 팝업
    //

    void ANgetBeforePopup(object obj)
    {

        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');


        switch (szBuf[0])
        {
            case "Success": SC_BeforePopup(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }

    }


    void ANgetAfterPopup(object obj)
    {
        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');

        Debug.Log("ANgetAfterPopup : " + szBuf[0].ToString());

        switch (szBuf[0])
        {
            case "Success": SC_AfterPopup(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }
    }


    //20140810 ljw 쉐어
    void ANgetSendShare(object obj)
    {
        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_SendShare(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }

    }





    void ANtutorial(object obj)
    {
        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_Tutorial(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }
    }

    void ANgetRank(object obj)
    {
        string[] szBuf;
        string szTemp;


        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_GameRankList(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }


    }


    void ANplayGame(object obj)
    {
        string[] szBuf;
        string szTemp;

        szBuf = ((string)obj).Split('|');

        switch (szBuf[0])
        {
            case "Success": SC_Play(szBuf[1]); break;
            case "Failed": CallErrBox.Instance.OpenErrMsg(szBuf[1]); break;
            default:
                break;
        }

        /*
        if (szBuf[0].Equals("Success"))
        {
            SC_Play(szBuf[1]);
        }
        else if (szBuf[0].Equals("Failed"))
        {
           CallErrBox.Instance.OpenErrMsg(szBuf[1]);
        }
        */

    }


    void ANlogout(object obj)
    {
        string[] szBuf;
        string szTemp;
        szBuf = ((string)obj).Split('|');
        if (szBuf[1].Equals("1"))
        {
            LogoutState();
        }
    }

    public void LogoutState()
    {

        GameClient.instance.StopBGM();
        // GameClient.mGameState = GameState.Login;
        // GameClient.instance.LoadingScene();
    }



    //통신후 처리
    void SC_JoinLogin(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }


        //로그인 성공
        if (((string)dict["result"]).Equals("ok"))
        {
            GameApplication.GetInstance().m_isTerms = true;
            GameApplication.GetInstance().SaveAppInfo();

            /*
            if( GameClient.mGameState ==  GameState.Login ) 
            {
                LoginMgr.GetInstance().SetType(LoginType.Term);
            }
            */

#if UNITY_IOS
            Global.USER_NUM = dict["U_NUM"].ToString();
            string tmp = string.Format("userdata://unum={0}&email={1}&passwd={2}", Global.USER_NUM, Global.USER_ID, Global.USER_PW);
            iOSManager.GetInstance().SetUserData(tmp);

#endif

            if (GameClient.mGameState != GameState.Lobby)
            {
                StopAllCoroutines();
                GameClient.mGameState = GameState.Lobby;
                SceneManager.LoadScene("LoadingScene");
            }

            if (pLoginComplete != null)
                pLoginComplete();

        }
        //로그인 실패
        else
        {
            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);

                /*
                switch ((string)dict["message"])
                {
                case "no user": Debug.Log("no user"); break;    //회원정보 없음
                case "out user": Debug.Log("out user"); break; //탈퇴한 회원
                }
                 * */

            }
        }
    }

    //로그인
    void SC_Login(string text)
    {
        //시작
        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }


        //로그인 성공
        if (((string)dict["result"]).Equals("ok"))
        {
            Debug.Log("Login Success :: " + text.Trim());

#if UNITY_IOS
            string tmp = "";

            if (dict["U_NUM"].ToString() != string.Empty)
            {
                Global.USER_NUM = dict["U_NUM"].ToString();
                tmp = string.Format("userdata://unum={0}&email={1}&passwd={2}", Global.USER_NUM, Global.USER_ID, Global.USER_PW);
            }
            else
            {
                tmp = string.Format("userdata://unum={0}&email={1}&passwd={2}", "", Global.USER_ID, Global.USER_PW);
            }

            iOSManager.GetInstance().SetUserData(tmp);
#endif

            if (GameClient.mGameState != GameState.Lobby)
            {
                GameClient.mGameState = GameState.Lobby;
                GameClient.instance.LoadingScene();
            }

            GameApplication.GetInstance().m_isTerms = true;
            GameApplication.GetInstance().SaveAppInfo();
        }
        //로그인 실패
        else
        {
            Debug.Log("Login Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);

                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("no user"); break;    //회원정보 없음
                    case "out user": Debug.Log("out user"); break; //탈퇴한 회원
                }
                */

            }
        }
    }

    void SC_TermPage(string text)
    {

        //{"result":"ok","data":{"G_NN_IMG":"http://14.63.161.30:8080/spaceticketImage/game/Icon_TippinBlock.png","U_NN_IMG":"","G_NAME":"Mystery Block","U_COUNTRY":"us"}}

        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }

        //로그인 성공
        if (((string)dict["result"]).Equals("ok"))
        {
            var tempdict = (Dictionary<string, object>)dict["data"];

            /*
            Debug.Log("보유 코인 갯수 : " + tempdict["G_NAME"].ToString());
            Debug.Log("보유 티켓 갯수 : " + tempdict["G_NN_IMG"].ToString());
            Debug.Log("보유 S티켓 갯수 : " + tempdict["U_COUNTRY"].ToString());
            Debug.Log("보유 S티켓 갯수 : " + tempdict["U_NN_IMG"].ToString());
            */

            Global.USER_CONTRY = tempdict["U_COUNTRY"].ToString();
            //LoginMgr.GetInstance().SetUserTexture(tempdict["U_NN_IMG"].ToString());
            //LoginMgr.GetInstance().SetType(LoginType.TermAccount);


            Debug.Log("TERMPage Success :: " + text.Trim());
        }
        //로그인 실패
        else
        {
            Debug.Log("Login Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("no user"); break;         //회원정보 없음
                    case "out user": Debug.Log("out user"); break; //탈퇴한 회원
                }
                 * */

            }
        }


    }




    //
    // 유저 정보 얻기
    //
    void SC_GetUser(string text)
    {
        //시작
        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }

        //성공
        if (((string)dict["result"]).Equals("ok"))
        {

            var tempdict = (Dictionary<string, object>)dict["data"];

            GameClient.instance.mUserCoin = long.Parse(tempdict["U_COIN_CNT"].ToString());
            GameClient.instance.mUserTicket = long.Parse(tempdict["U_TICKET_CNT"].ToString());
            GameClient.instance.mUserSTicket = long.Parse(tempdict["U_SAVE_TICKET_CNT"].ToString());

            Debug.Log("보유 코인 갯수 : " + tempdict["U_COIN_CNT"].ToString());
            Debug.Log("보유 티켓 갯수 : " + tempdict["U_TICKET_CNT"].ToString() + " user ticket" + GameClient.instance.mUserTicket);
            Debug.Log("보유 S티켓 갯수 : " + tempdict["U_SAVE_TICKET_CNT"].ToString());

            if (pUserDataComplete != null)
                pUserDataComplete();


            //Debug.Log("주간 누적 티켓 갯수 : "+tempdict["U_ACCUM_TICKET_CNT"].ToString());
        }

        //실패
        else
        {
            Debug.Log("GetUser Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("no user"); break;
                }
                */

            }
        }


        //LobbyMgr.Instance.IsLock = false; 
    }


    //finishGame
    // 로비진입,순위얻기
    //
    void SC_GameRoomList(string text)
    {
        int nDataCount;
        int i;
        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }

        //성공
        if (((string)dict["result"]).Equals("ok"))
        {

            //일단 처음 발견한 GO_NUM 자동입력
            //기기마다 GO_NUM입력필요...

            nDataCount = (int)(long)dict["dataCnt"];
            for (i = 0; i < nDataCount; i++)
            {
                var tempdict = ((Dictionary<string, object>)((List<object>)dict["data"])[i]);

                bool status = true;
                int LevelValue = int.Parse(tempdict["GO_STATUS"].ToString());
                string szName = tempdict["GO_NAME"].ToString();
                string szgrade = tempdict["GO_ICON"].ToString(); //stater, challenge , surprise , miracle , adventure , event
                string szNum = tempdict["GO_NUM"].ToString();

                int needcoin = int.Parse(tempdict["GO_COIN_CHARGE"].ToString());
                int Jackpot = int.Parse(tempdict["GO_JACKPOT_CNT"].ToString());
                int jackpotsum = int.Parse(tempdict["GO_JACKPOT_SUM_CNT"].ToString());

                if (tempdict["GO_GAME_STATUS"].ToString().Equals("no"))
                    status = false;

                MachineData data = (MachineData)GameClient.instance.mMachinelist[i];

                _enMachineGrade MachineGrade = _enMachineGrade.None;

                if (szgrade.Equals(_enMachineGrade.starter.ToString()))
                    MachineGrade = _enMachineGrade.starter;
                else if (szgrade.Equals(_enMachineGrade.challenge.ToString()))
                    MachineGrade = _enMachineGrade.challenge;
                else if (szgrade.Equals(_enMachineGrade.surprise.ToString()))
                    MachineGrade = _enMachineGrade.surprise;
                else if (szgrade.Equals(_enMachineGrade.miracle.ToString()))
                    MachineGrade = _enMachineGrade.miracle;
                else if (szgrade.Equals(_enMachineGrade.adventure.ToString()))
                    MachineGrade = _enMachineGrade.adventure;
                else if (szgrade.Equals("event"))
                    MachineGrade = _enMachineGrade.gevent;


                data.SetData(LevelValue, szNum, szName, Jackpot, jackpotsum, needcoin, MachineGrade, status);

                /*
                if (GameClient.mGameState == GameState.Mystery)
                {
                    MysteryMgr.Instance.UpdateInfo();
                }
                */
                //m_cWww.GO_NUM = int.Parse(tempdict["GO_NUM"].ToString());
                if (pEnterLobbyComplete != null)
                    pEnterLobbyComplete();
            }
        }
        //실패
        else
        {
            Debug.Log("Get RoomList Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no game": Debug.Log("No exist game"); break;
                    case "no data": Debug.Log("No data Error"); break;
                }
                 * */

            }
        }

        //LobbyMgr.Instance.IsLock = false; 

    }

    /*
     
 {"result":"ok","rank_data":[{"rank":1,"S_SCORE":4,"U_IMG_TYPE":"st","U_NAME":"팀장님짱","U_NN_IMG":"","U_NUM":4,"U_COUNTRY":"us","S_NUM":2},
                              {"rank":1,"S_SCORE":4,"U_IMG_TYPE":"st","U_NAME":"TS00001","U_NN_IMG":"","U_NUM":3,"U_COUNTRY":"us","S_NUM":3}],
"game_data":{"GO_STATUS":100,"G_NUM":5,"G_URL":"www.goticketspace.com/download/dragracing","G_APP_TYPE":"common","G_HELP_CONTENT":"How to play\r\n1. Insert Coins('To play' button)\r\n2. Push Start('Red') button to start Dragracing\r\n3. Stop the car on finish line and Get a Win\r\n4. Get Tickets\r\nAnd\u2026 Look for \u201cMystery Bonus Chance\u201d by mystery zone !\r\n","GO_NAME":"DR0001","G_IOS_PACK_NAME":"com.goticketspace.app.dragracing","GO_COIN_CHARGE":1,"G_NAME":"Dragracing","challengerCnt":32,"GO_NUM":15,"GO_GAME_STATUS":"no","GO_PLAY_STATUS":"no","leftDays":7,"GO_JACKPOT_SUM_CNT":0,"G_ANDROID_PACK_NAME":"com.goticketspace.app.dragracing","GO_JACKPOT_CNT":200}}

     * 
    */

    void SC_GameRankList(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }

        //성공

        CRankView RankView = LobbyMgr.Instance.RankView;


        if (((string)dict["result"]).Equals("ok"))
        {

            int count = ((List<object>)dict["rank_data"]).Count;

            for (int i = 0; i < count; i++)
            {
                var tempdict = ((Dictionary<string, object>)((List<object>)dict["rank_data"])[i]);

                if (i > RankView.MemberList.Length) break;

                string rank = tempdict["rank"].ToString();
                string ImgUrl = tempdict["U_NN_IMG"].ToString();
                string uname = tempdict["U_NAME"].ToString();
                string score = tempdict["S_SCORE"].ToString();

                RankView.MemberList[i].SetRankData(rank, uname, score, ImgUrl);

            }



            if (pRankComplete != null)
            {
                pRankComplete();
            }
        }

        //실패
        else
        {
            Debug.Log("Get Rank Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no game": Debug.Log("Game does not exist(3)"); break; //존재하지 않는 게임
                    case "no data": Debug.Log("No gamedata info(3)"); break; //반환값 없음
                }
                 * */

            }
        }
    }

    //
    // 게임 시작
    //
    void SC_Play(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;
        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }

        //성공
        if (((string)dict["result"]).Equals("ok"))
        {

            var tempdict = (Dictionary<string, object>)dict["data"];

            //키등록
            m_cWww.KEY = tempdict["KEY"].ToString();

            Debug.Log("게임옵션 : " + tempdict["GO_STATUS"].ToString());
            Debug.Log("코인차감갯수 : " + tempdict["GO_COIN_CHARGE"].ToString());
            Debug.Log("초기설정잭팟값 : " + tempdict["GO_JACKPOT_CNT"].ToString());



            GameClient.instance.GetMachineData().theLevelValue = int.Parse(tempdict["GO_STATUS"].ToString());
            //GameClient.instance.GetMachineData().theNeedCoin = int.Parse(tempdict["GO_COIN_CHARGE"].ToString());
            GameClient.instance.GetMachineData().theJackPot = int.Parse(tempdict["GO_JACKPOT_CNT"].ToString());




            if (pPlayComplete != null)
                pPlayComplete();

        }
        //실패
        else
        {
            Debug.Log("Do Play Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("미가입 회원"); break;
                    case "no game": Debug.Log("존재하지 않는 게임"); break;
                    case "no game option": Debug.Log("존재하지않는 게입옵션(난이도)"); break;
                    case "no coin": Debug.Log("회원의 코인 갯수 부족"); break;
                }*/

            }
        }
    }



    void SC_AfterPopup(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;

        Debug.Log("SC_AfterPopup : " + text);

        //성공
        if ((dict != null) && (((string)dict["result"]).Equals("ok")))
        {
            CPopupMgr.instance.Init();

            //var tempdict = ((Dictionary<string, object>)((List<object>)dict["data"])[i]);


            //잭팟 했을때 팝업
            try
            {

                int Count = int.Parse((string)dict["popupWinDataCount"].ToString());

                for (int i = 0; i < Count; i++)
                {
                    var tempdict = ((Dictionary<string, object>)((List<object>)dict["popupWinData"])[i]);

                    popupInfo data = new popupInfo();

                    data.num = int.Parse(tempdict["num"].ToString());

                    data.title = tempdict["title"].ToString();

                    data.imageLink = tempdict["imageLink"].ToString();

                    data.actionType = (ActionType)int.Parse(tempdict["action"].ToString());

                    data.actionLink = tempdict["actionLink"].ToString();

                    data.language = tempdict["language"].ToString();

                    data.tUrl = tempdict["tUrl"].ToString();

                    data.go_num = tempdict["go_num"].ToString();

                    data.g_num = tempdict["g_num"].ToString();

                    data.iUrl = tempdict["iUrl"].ToString();

                    data.todayUse = tempdict["todayUse"].ToString().Equals("ok") ? true : false;

                    data.bJacpot = true;

                    CPopupMgr.instance.Add(data);
                }
            }
            catch
            {
            }

            if (pAfterPopupComplete != null)
                pAfterPopupComplete();

        }

        //실패
        else
        {
            Debug.Log("Get Result Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("미가입 회원"); break;
                    case "no game": Debug.Log("존재하지 않는 게임"); break;
                    case "no history": Debug.Log("KEY와 일치하는 게임 없음"); break;
                    case "already finish game": Debug.Log("이미 종료된 게임"); break;
                }
                 */

            }
        }
    }




    //
    // 게임종료. 결과전송
    // //{"levelUp":{"result":"no"},"result":"ok","newRank":{"result":"no"},"bestScore":{"result":"no"},
    //"popupWinData":[{"buttonLink":"http:\/\/daum.net","content":"오프로더 윈","title":"오프로더 윈","location":"win","link":"http:\/\/naver.com"}],
    //"popupAfterData":[{"buttonLink":"http:\/\/daum.net","content":"게임 후 팝업","title":"오프로더 게임 후 ","location":"ag","link":"http:\/\/naver.com"}]}


    //sdh 20140704 팝업

    void SC_Result(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;

        //성공
        if ((dict != null) && (((string)dict["result"]).Equals("ok")))
        {
            CPopupMgr.instance.Init();

            //var tempdict = ((Dictionary<string, object>)((List<object>)dict["data"])[i]);

            //게임종료후 팝업 
            try
            {
                int Count = int.Parse((string)dict["popupAfterDataCount"].ToString());

                for (int i = 0; i < Count; i++)
                {
                    var tempdict = ((Dictionary<string, object>)((List<object>)dict["popupAfterData"])[i]);

                    popupInfo data = new popupInfo();

                    data.num = int.Parse(tempdict["num"].ToString());

                    data.title = tempdict["title"].ToString();

                    data.imageLink = tempdict["imageLink"].ToString();

                    data.actionType = (ActionType)int.Parse(tempdict["action"].ToString());

                    data.actionLink = tempdict["actionLink"].ToString();

                    data.language = tempdict["language"].ToString();

                    data.tUrl = tempdict["tUrl"].ToString();

                    data.go_num = tempdict["go_num"].ToString();

                    data.g_num = tempdict["g_num"].ToString();

                    data.iUrl = tempdict["iUrl"].ToString();

                    data.todayUse = tempdict["todayUse"].ToString().Equals("ok") ? true : false;

                    data.bJacpot = false;

                    CPopupMgr.instance.Add(data);

                    Debug.Log("AfterPopup" + data.bJacpot);
                }
            }
            catch
            {
            }


            //잭팟 했을때 팝업
            try
            {

                int Count = int.Parse((string)dict["popupWinDataCount"].ToString());

                for (int i = 0; i < Count; i++)
                {
                    var tempdict = ((Dictionary<string, object>)((List<object>)dict["popupWinData"])[i]);

                    popupInfo data = new popupInfo();

                    data.num = int.Parse(tempdict["num"].ToString());

                    data.title = tempdict["title"].ToString();

                    data.imageLink = tempdict["imageLink"].ToString();

                    data.actionType = (ActionType)int.Parse(tempdict["action"].ToString());

                    data.actionLink = tempdict["actionLink"].ToString();

                    data.language = tempdict["language"].ToString();

                    data.tUrl = tempdict["tUrl"].ToString();

                    data.go_num = tempdict["go_num"].ToString();

                    data.g_num = tempdict["g_num"].ToString();

                    data.iUrl = tempdict["iUrl"].ToString();

                    data.todayUse = tempdict["todayUse"].ToString().Equals("ok") ? true : false;

                    data.bJacpot = true;

                    CPopupMgr.instance.Add(data);

                    Debug.Log("WinPopup" + data.bJacpot);
                }
            }
            catch
            {
            }


            if (pFinishComplete != null)
                pFinishComplete();

        }

        //실패
        else
        {
            Debug.Log("Get Result Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("미가입 회원"); break;
                    case "no game": Debug.Log("존재하지 않는 게임"); break;
                    case "no history": Debug.Log("KEY와 일치하는 게임 없음"); break;
                    case "already finish game": Debug.Log("이미 종료된 게임"); break;
                }
                 */

            }
        }
    }

    //
    // 게임종료. 결과전송
    //
    void SC_SaveTicket(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;

        //성공
        if ((dict != null) && (((string)dict["result"]).Equals("ok")))
        {
            if (pSaveTicketComplete != null)
                pSaveTicketComplete();
        }
        //실패
        else
        {
            Debug.Log("SC_SaveTicket Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("존재 하지 않는 회원"); break;
                    case "no save ticket": Debug.Log("받을 티켓이 없는경우"); break;
                }
                */
            }
        }
    }

    void SC_Tutorial(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;

        if ((dict != null) && (((string)dict["result"]).Equals("ok")))
        {

            if (pTutorialComplete != null)
            {
                pTutorialComplete();
            }
        }
        else
        {
            Debug.Log("SC_Tutorial Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("존재 하지 않는 회원"); break;
                    case "no save ticket": Debug.Log("받을 티켓이 없는경우"); break;
                }
                */


            }
        }

    }

    // https://goticketspace.com:7443/TicketSpace/module/getBeforePopup.jsp?U_EMAIL=ffffffff-f5ef-1cdf-ce36-ac260033c587&U_PW=Z45dFtn42Ow56Cg7fhmb&G_PACK_NAME=com.goticketspace.app.mysteryoffroader&APP_TYPE=android
    // sdh 20140704 팝업
    // {"result":"ok","popupBeforeData":[{"content":"오프로더 게임팝업","title":"오프로더 게임 팝업","location":"bg","link":"http://naver.com","buttonLink":"http://daum.net"}]}
    void SC_BeforePopup(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;

        //성공
        if ((dict != null) && (((string)dict["result"]).Equals("ok")))
        {

            //var tempdict = ((Dictionary<string, object>)((List<object>)dict["data"])[i]);

            int Count = int.Parse((string)dict["popupBeforeDataCount"].ToString());

            CPopupMgr.instance.Init();

            if (Count == 0)
            {
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    var tempdict = ((Dictionary<string, object>)((List<object>)dict["popupBeforeData"])[i]);

                    popupInfo data = new popupInfo();

                    data.num = int.Parse(tempdict["num"].ToString());

                    data.title = tempdict["title"].ToString();

                    data.imageLink = tempdict["imageLink"].ToString();

                    data.actionType = (ActionType)int.Parse(tempdict["action"].ToString());

                    data.actionLink = tempdict["actionLink"].ToString();

                    data.language = tempdict["language"].ToString();

                    data.tUrl = tempdict["tUrl"].ToString();

                    data.go_num = tempdict["go_num"].ToString();

                    data.g_num = tempdict["g_num"].ToString();

                    data.iUrl = tempdict["iUrl"].ToString();

                    data.todayUse = tempdict["todayUse"].ToString().Equals("ok") ? true : false;

                    data.bJacpot = false;


                    CPopupMgr.instance.Add(data);
                }
            }


            if (pBeforePopupComplete != null)
                pBeforePopupComplete();
        }
        //실패
        else
        {
            Debug.Log("SC_BeforePopup Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {

                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);

                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("존재 하지 않는 회원"); break;
                    case "no save ticket": Debug.Log("받을 티켓이 없는경우"); break;
                }
                */

            }
        }
    }



    //20140810 ljw 쉐어
    void SC_SendShare(string text)
    {
        var dict = Json.Deserialize(text) as IDictionary<string, object>;



        if (dict == null) { Debug.Log("Parse Err :: " + text.Trim()); return; }

        if ((dict != null) && (((string)dict["result"]).Equals("ok")))
        {

            GameClient.instance.curShare = int.Parse((string)dict["todayShareCount"].ToString());
            GameClient.instance.maxShare = int.Parse((string)dict["todayTotalCount"].ToString());



            if (pSendShareComplete != null)
            {
                pSendShareComplete();
            }


        }
        else
        {
            Debug.Log("SC_SENDSHARE Failed :: " + text.Trim());

            //지정 오류
            if (((string)dict["result"]).Equals("err"))
            {
                CallErrBox.Instance.OpenErrMsg((string)dict["message"]);
                /*
                switch ((string)dict["message"])
                {
                    case "no user": Debug.Log("존재 하지 않는 회원"); break;
                    case "no save ticket": Debug.Log("받을 티켓이 없는경우"); break;
                }
                */
            }
        }

    }
}
