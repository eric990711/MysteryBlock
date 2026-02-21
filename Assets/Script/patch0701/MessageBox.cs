using UnityEngine;
using System.Collections;


public enum emMsgType
{
    LOGOUT,     //로그아웃
    INGAMEEXIT, //게임을 나갔을때 
    PLATFORM,   //플랫폼으로 이동할때
    GETTICKET,  //티켓을 수령받을 갈때
    COIN_CHARGE, //코인 충전
    COIN_NOT_ENOUGH, //코인 부족할떄 나오는 메세지
    NO_PLATFORM,    //TS 미설치
    BACK_BUTTON,    //안드로이드만 적용 백버튼 누른후 처리
    TUTO_OUT, //튜토리얼 스킵
    TUTO_MOVEPLATFORM,
    IOS_PLATFORM,
    FBSHAREWIN,
    FBSHARERESULT,
    FBSHAREFULL
}

public enum emBtnType
{ 
    YESNO,
    OK,
}

public class MessageBox : MonoBehaviour {

    emMsgType mMsgType = emMsgType.LOGOUT;

    public static MessageBox Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MessageBox>();

            }
            return m_instance;
        }
    }

    private static MessageBox m_instance = null;

    //내용
    
    public UILabel DescTextLabel;
    public UILabel YesLabel;
    public UILabel NoLabel;

    
    public GameObject MBox;
    public GameObject TMBox;

    Vector3 []BtnPos = new Vector3[2];

	// Use this for initialization

    GameObject []BtnObj = new GameObject[2];



	void Start () {

        BtnObj[0] = YesLabel.transform.parent.gameObject;
        BtnObj[1] = NoLabel.transform.parent.gameObject;


        BtnPos[0] = BtnObj[0].transform.localPosition;
        BtnPos[1] = BtnObj[1].transform.localPosition;
        
	}
     
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            OpenMessageBox(emMsgType.BACK_BUTTON);
        }
    }



    public void MsgBoxActive(bool bActive)
    {
//         if (GameClient.instance.isVisitor == true)
//         {
//             //mView = TMBox.gameObject;
//             TMBox.SetActive(true);
//             MBox.SetActive(false);
//         }
//         else
//         {
//             //mView = MBox.gameObject;.
//             MBox.SetActive(true);
//             TMBox.SetActive(false);
//         }
        TMBox.SetActive(bActive);
        //mView.SetActive(bActive);
    }
    
    public bool IsBoxActive()
    {
        //return mView.activeSelf;
        if (GameClient.instance.isVisitor == true)
        {
            //mView = TMBox.gameObject;
            
            return TMBox.activeSelf;
        }
        else
        {
            //mView = MBox.gameObject;.
            //MBox.SetActive(true);
            
            return MBox.activeSelf;
        }

    }
    
    public void OpenMessageBox(emMsgType _MsgType , emBtnType _BtnType = emBtnType.YESNO)
    {
        if (LoadingBar.GetInstance().GetLock() == true)
            return; 

        GameClient.instance.IsPause = true; 
        
        mMsgType = _MsgType;

        if (_BtnType == emBtnType.YESNO)
        {
            BtnObj[0].transform.localPosition = BtnPos[0] ;
            BtnObj[1].transform.localPosition = BtnPos[1];
            
            BtnObj[1].SetActive(true);

            YesLabel.text = TextManager.GetInstance().GetText(emString.Yes);
            NoLabel.text = TextManager.GetInstance().GetText(emString.No);
        }
        else
        {
            Vector3 vec = BtnPos[0];
            vec.x = 0.0f;

            BtnObj[0].transform.localPosition = vec;

            
            YesLabel.text = TextManager.GetInstance().GetText(emString.Ok);

            BtnObj[1].SetActive(false);
            

        }
      
        switch(_MsgType)
        {
            case emMsgType.LOGOUT: DescTextLabel.text = TextManager.GetInstance().GetText(emString.LogoutMsg); break;
            case emMsgType.INGAMEEXIT: DescTextLabel.text = TextManager.GetInstance().GetText(emString.IngameExitMsg); break;
            case emMsgType.GETTICKET: DescTextLabel.text = TextManager.GetInstance().GetText(emString.GetTicketMsg); break;
            case emMsgType.PLATFORM: DescTextLabel.text = TextManager.GetInstance().GetText(emString.PlatformMsg);
#if UNITY_ANDROID

                DescTextLabel.text = TextManager.GetInstance().GetText(emString.PlatformMsg);

#elif UNITY_IOS
                DescTextLabel.text = TextManager.GetInstance().GetText(emString.IOS_Platform);
#endif
                
                break;
            
            case emMsgType.COIN_CHARGE: DescTextLabel.text = TextManager.GetInstance().GetText(emString.CoinChargeMsg); break;
            case emMsgType.COIN_NOT_ENOUGH: DescTextLabel.text = TextManager.GetInstance().GetText(emString.CoinNotEnoughMsg); break;
            case emMsgType.NO_PLATFORM: DescTextLabel.text = TextManager.GetInstance().GetText(emString.NoPlatformMsg); break;
            case emMsgType.BACK_BUTTON: DescTextLabel.text = TextManager.GetInstance().GetText(emString.BackbuttonMsg); break;
            case emMsgType.TUTO_OUT:
                if (GameClient.mGameState != GameState.Mystery)
                {
                    DescTextLabel.text = TextManager.GetInstance().GetText(emString.ExitTutoLobby);
                }
                else
                {
                    DescTextLabel.text = TextManager.GetInstance().GetText(emString.ExitTutoIngame);
                }
                
                break;
            case emMsgType.TUTO_MOVEPLATFORM:

                DescTextLabel.text = TextManager.GetInstance().GetText(emString.EndTuto);
                if (GameClient.mNetwork == true)
                {
                    CNetwork.GetInstance().CS_Tutorial();
                }
                break;


            //by ljw 20140813 페이스북 공유시 메세지 
            case emMsgType.FBSHARERESULT:
                DescTextLabel.text = TextManager.GetInstance().GetText(emString.FBSHARERESULT);
                break;
            //by ljw 20140813 페이스북 공유시 메세지 
            case emMsgType.FBSHAREWIN:
                DescTextLabel.text = string.Format(TextManager.GetInstance().GetText(emString.FBSHAREWIN), GameClient.instance.curShare, GameClient.instance.maxShare);
                break;
            case emMsgType.FBSHAREFULL:
                DescTextLabel.text = TextManager.GetInstance().GetText(emString.FBFULLREWARD);
                break;
        }

        MsgBoxActive(true);
    }
       
    public void MsgYes()
    {
        GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_Button);
        
        switch (mMsgType)
        {
            case emMsgType.LOGOUT:
                //연동 로그아웃
                CNetwork.GetInstance().Logout();
                 break;
            case emMsgType.INGAMEEXIT:
                //  GameClient.mGameState = GameState.Lobby;
                //  GameClient.instance.LoadingScene();
                 //GAManager.Instance.bEndLevelSession = true;

			Debug.Log("MsgYes() mUserTicket " + GameClient.instance.mUserTicket);
                 if (GameClient.instance.mUserTicket == 0)
                 {
					 Invoke("DelayGoLobby" , 0.5f);
                 }
                 else
                 {
                     MysteryMgr.Instance.OpenTicketMachine();
                 }
                break;
            case emMsgType.PLATFORM:
            case emMsgType.GETTICKET:
            case emMsgType.COIN_CHARGE:
            case emMsgType.COIN_NOT_ENOUGH:
                //연동 플랫폼 이동
                if (GameApplication.GetInstance().m_isPlatform == true)
                {
                    CNetwork.GetInstance().GoPlatform();
                    DelayGameExit();
                    //Invoke("DelayGameExit", 0.1f);
                }
                else 
                {
                    Invoke("DelayOpenMessgaeBox", 0.5f);
                }
                break; 
            case emMsgType.BACK_BUTTON:
                #if UNITY_ANDROID

                System.Diagnostics.ProcessThreadCollection pt = System.Diagnostics.Process.GetCurrentProcess().Threads;
                foreach (System.Diagnostics.ProcessThread p in pt)
                {
                    p.Dispose();
                }

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                Application.Quit();
                Debug.Log("Quit");
                
#elif UNITY_IOS
                if (GameApplication.GetInstance().m_isPlatform == true)
                {
                    CNetwork.GetInstance().GoPlatform();
                }
                else 
                {
                    Application.Quit();
                }
#endif

                break;
            case emMsgType.NO_PLATFORM:
                
#if UNITY_ANDROID
                CNetwork.GetInstance().PlatFormDownload();
                DelayGameExit();
#elif UNITY_IOS
                MsgNo();
#endif
                break; 
            case emMsgType.TUTO_OUT:
                if (GameClient.mGameState != GameState.Mystery)
                {
                    //MBox.SetActive(true);
                    //GameClient.instance.isVisitor = false;
                    CNetwork.GetInstance().CS_Tutorial();
                    GameClient.instance.isVisitor = false;
                    
                    GameClient.instance.SaveTutResult();
                    GameClient.instance.isIngame = false;
                    //GameClient.instance.tutorial.gameObject.SetActive(false);
                    for (int i = 0; i < LobbyMgr.Instance.tutoLobby.DeleteObj.Length; i++)
                    {
                        LobbyMgr.Instance.tutoLobby.DeleteObj[i].SetActive(false);
                    }
                }
                else
                {
                    GameClient.instance.isVisitor = false;
                    GameClient.instance.SaveTutResult();
                    GameClient.instance.isIngame = false;
                    GameClient.mGameState = GameState.Lobby;
                    GameClient.instance.LoadingScene();
                }
                
                break;
            case emMsgType.TUTO_MOVEPLATFORM:

                GameClient.instance.isVisitor = false;
                GameClient.instance.SaveTutResult();
                GameClient.instance.isIngame = false;

                if (GameClient.mNetwork == false)
                {
                    // Offline: just hide tutorial UI and stay in lobby
                    for (int i = 0; i < LobbyMgr.Instance.tutoLobby.DeleteObj.Length; i++)
                    {
                        LobbyMgr.Instance.tutoLobby.DeleteObj[i].SetActive(false);
                    }
                    LobbyMgr.Instance.tutoLobby.gameObject.SetActive(false);
                }
                else if (GameApplication.GetInstance().m_isPlatform == true)
                {
                    CNetwork.GetInstance().GoPlatform();
                    DelayGameExit();
                }
                else
                {
                    Invoke("DelayOpenMessgaeBox", 0.5f);
                }
                //GameClient.instance.tutorial.gameObject.SetActive(false);
                break;


            case emMsgType.FBSHARERESULT:
                GameClient.mGameState = GameState.Lobby;
                GameClient.instance.LoadingScene();

                //by ljw 20140813 페이스북 공유시 메세지 
                break;
            case emMsgType.FBSHAREWIN:
                //무료게임 진행 로직을 넣어주세요! 이재우

                RecountWinText.Instance.enabled = false;
                MysteryMgr.Instance.em_playtype = PLAYTYPE._BONUS;
                Invoke("BonusGame", 1);
                break;
            //by ljw 20140813 페이스북 공유시 메세지 
            case emMsgType.FBSHAREFULL:
                //20140826 ljw 무료게임최대치 추가
                RecountWinText.Instance.enabled = false;
                
                MysteryMgr.Instance.em_playtype = (MysteryMgr.Instance.mUseCoinx2 == true) ? MysteryMgr.Instance.em_playtype = PLAYTYPE._DOUBLE : MysteryMgr.Instance.em_playtype = PLAYTYPE._DEFAULT;
                break;

            default:
                break; 

        }
        
        MsgBoxActive(false);
        GameClient.instance.IsPause = false;
         
    }
        


    public void MsgNo()
    {
        GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_Button);
        MsgBoxActive(false);
        if (mMsgType == emMsgType.TUTO_MOVEPLATFORM)
        {
            GameClient.instance.isVisitor = false;
            GameClient.instance.isIngame = false;
            GameClient.instance.SaveTutResult();

            if (GameClient.mNetwork == false)
            {
                for (int i = 0; i < LobbyMgr.Instance.tutoLobby.DeleteObj.Length; i++)
                {
                    LobbyMgr.Instance.tutoLobby.DeleteObj[i].SetActive(false);
                }
                LobbyMgr.Instance.tutoLobby.gameObject.SetActive(false);
            }
        }
        /*
        switch (mMsgType)
        {
            case emMsgType.
         * :
                break;
            case emMsgType.INGAMEEXIT:
              
        
                break;
        }
        */
        GameClient.instance.IsPause = false;

    }

    public void DelayGameExit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void DelayOpenMessgaeBox()
    {
#if UNITY_ANDROID
        GameClient.instance.IsPause = true;
        OpenMessageBox(emMsgType.NO_PLATFORM);
        
#elif UNITY_IOS
		CPopupMgr.instance.OpenWebView(true, LobbyMgr.Instance.webView);
#endif
    }


	public void DelayGoLobby()
	{
		GameClient.mGameState = GameState.Lobby;
		GameClient.instance.LoadingScene();
	}

    void PlayGameCallback()
    {
        MBUIController.Instance.GameStart();
    }

    void BonusGame()
    {
        GameObject doubleBtn = GameObject.Find("DoubbleButton");
        doubleBtn.GetComponent<MBBtnMgr>().ButtonActive(doubleBtn, false);

        MysteryMgr.Instance.mDoubleTicket = 1;
        MysteryMgr.Instance.mDoubleJackpot = 1;
        MysteryMgr.Instance.mDoubleToggle = 1;
        
        MBUIController.Instance.RefreshMultiplier();

        MysteryMgr.Instance.InitMysteryGame();

        if (GameClient.mNetwork == true)
        {
            CNetwork.GetInstance().CS_Play(MysteryMgr.Instance.em_playtype, PlayGameCallback);
        }
        else
        {
            PlayGameCallback();
        }
        
    }
}
