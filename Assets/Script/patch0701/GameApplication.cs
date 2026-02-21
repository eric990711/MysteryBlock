using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum DeviceType
{
    PC = 0,
    Android,
    IOS,
}

public enum emLanguage
{
    Kor,
    Eng,
};

public enum Market
{
    TSTORE, PLAYSTORE
}


public class GameApplication : MonoBehaviour
{
    public Market market = Market.TSTORE;
    public int m_screenWidth = 0;
    public int m_screenHeight = 0;
    public bool m_isTerms = false; //terms set true
    public bool m_isDirectID = false;
    public bool m_isPlatform = false;

    public DeviceType deviceType = DeviceType.Android;
    public bool isDirectPlay = false;

    public bool m_DevFacebook = true;   //페이스북 과 팝업을 같이 처리한다. true 활성화 false 기능끔.


    [HideInInspector]
    public emLanguage m_Language;

    private GameObject IntroSprite = null;

    public ScreenOrientation m_Orientation;


    void Init() { }

    public bool GetLanguage(emLanguage lang)
    {
        return m_Language == lang ? true : false;
    }

    public string StoreName()
    {
        string url = null;
#if UNITY_ANDROID
        switch (market)
        {
            case Market.TSTORE:
                url = "https://goticketspace.com:7443/TicketSpace/shop/downloadUrl.jsp?market=tstore";
                break;
            case Market.PLAYSTORE:
                url = "https://goticketspace.com:7443/TicketSpace/shop/downloadUrl.jsp?market=googleplay";
                break;

        }
#elif UNITY_IOS
        url = "https://goticketspace.com:7443/TicketSpace/shop/downloadUrl.jsp?market=appstore";
#endif
        return url;
    }

    // Use this for initialization
    void Awake()
    {
        Debug.Log("GameApplication Awake~~~");

        Screen.SetResolution(m_screenWidth, m_screenHeight, true);

        m_Orientation = Screen.orientation;



        //ios send direct
        CNetwork.GetInstance().Init();
        Object.DontDestroyOnLoad(this);

        LoadAppInfo();

        IntroSprite = GameObject.Find("UI_Intro").transform.Find("Camera/Anchor/Panel/Sprite").gameObject;


        if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
            m_Language = emLanguage.Kor;
        else
            m_Language = emLanguage.Eng;


#if UNITY_IOS
		/*
		if( IntroSprite )
		{
			IntroSprite.SetActive(true);
			IntroSprite.GetComponent<UISprite>().alpha = 1.0f; 
			IntroSprite.GetComponent<TweenAlpha>().enabled = false; 
		}
		*/
#endif

        //GameClient.mGameState = GameState.Login;
    }

    void Update()
    {
        //        Debug.Log(Screen.fullScreen);
    }

    void Start()
    {
        Debug.Log(gameObject + "Start~~~~ ");
        CNetwork.GetInstance().CheckPlatform();




        //Application.backgroundLoadingPriority = ThreadPriority.High;

        //연동 초기화
        //Invoke("NetworkInit", 1.0f);

        //GameClient.instance.LoadUserOption();

        //GameManager.Instance.deviceType = deviceType;
        //m_GameManager = GameManager.Instance;
        //Debug.Log("intro~~");


        StartCoroutine(IntroFadeOut());


        if (gameObject == null) Debug.Log("Not in here Application");
        else Debug.Log("Yes in GameApplication");

    }

    bool IsPlatformPlay = false;

    IEnumerator IntroFadeOut()
    {
        Debug.Log("start IntroFadeOut");

        yield return new WaitForSeconds(0.5f);

        TweenAlpha tweenAlpah = null;

        if (IntroSprite)
        {
            IntroSprite.SetActive(true);
            tweenAlpah = IntroSprite.GetComponent<TweenAlpha>();
        }


        while (true)
        {
            if (tweenAlpah.tweenFactor >= 1.0f)
            {

                //tweenAlpah.gameObject.SetActive(false);
                Debug.Log("IntroFadeOut");


                yield return new WaitForSeconds(1.5f);

                if (isDirectPlay == false)
                {
                    DirectStartGame();
                    break;
                }

                break;
            }
            yield return null;
        }

    }


    public void DirectStartGame()
    {


        if (m_isTerms == false)
        {
            GameClient.mGameState = GameState.Login;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            CNetwork.GetInstance().StartGame();
        }
    }



    public void TSANReciveInfo(string msg)
    {
        //go to looby

        Debug.Log("TSANReciveInfo~~  " + msg + " terms " + m_isTerms);

        //iOSManager.GetInstance().SetLog(str);
        if (msg.Contains("result:"))
        {
            msg = msg.Replace("result:", "");
            if (msg.Equals("doPlay"))
            {
                isDirectPlay = true;
                if (m_isTerms == false)
                {
                    m_isTerms = true;
                    SaveAppInfo();
                }
                CNetwork.GetInstance().GetUserInfo();
            }
            //loginpage start Gamepage
            else if (msg.Equals("startGame"))
            {
                isDirectPlay = true;

                if (GameApplication.GetInstance().m_isTerms == false)
                {
                    GameClient.mGameState = GameState.Login;
                    SceneManager.LoadScene("LoadingScene");
                }
                else
                {
                    GameClient.mGameState = GameState.Lobby;
                    SceneManager.LoadScene("LoadingScene");
                }
            }
            else
            {
                CallErrBox.Instance.OpenErrMsg(msg);
                Debug.Log("Error :" + msg);
                //Msg error
            }
        }
        else if (msg.Contains("checkPlatform:"))
        {
            msg = msg.Replace("checkPlatform:", "");

            Debug.Log("checkplatform msg " + msg);

            if (msg == "1")
                m_isPlatform = true;
            else
                m_isPlatform = false;

            Debug.Log("m_isPlatform " + m_isPlatform);

        }

        //return ; 
    }


    public void TSReciveInfo(string msg)
    {
        //go to looby
        Debug.Log("TSReciveInfo~~  " + msg + " terms " + m_isTerms);

        //iOSManager.GetInstance().SetLog(str);
        if (msg.Contains("result:"))
        {
            msg = msg.Replace("result:", "");

            if (msg.Equals("doPlay"))
            {
                isDirectPlay = true;
                if (m_isTerms == false)
                {
                    m_isTerms = true;
                    SaveAppInfo();
                }

                GameClient.mGameState = GameState.Mystery;
                SceneManager.LoadScene("LoadingScene");
            }
            //loginpage start Gamepage
            else if (msg.Equals("startGame"))
            {
                isDirectPlay = true;

                if (m_isTerms == false)
                {
                    GameApplication.GetInstance().m_isTerms = true;
                    GameApplication.GetInstance().SaveAppInfo();
                }

                GameClient.mGameState = GameState.Lobby;
                SceneManager.LoadScene("LoadingScene");


            }
            else if (msg.Equals("DirectStart"))
            {

                isDirectPlay = true;
                if (m_isTerms == true)
                    GameClient.mGameState = GameState.Lobby;
                else
                    GameClient.mGameState = GameState.Login;


                SceneManager.LoadScene("LoadingScene");
            }
            else
            {
                CallErrBox.Instance.OpenErrMsg(msg);
                Debug.Log("Error :" + msg);
                //Msg error
            }
        }
        else if (msg.Contains("joinOrLogin"))
        {
            Debug.Log("JoinOrLogin" + "id" + Global.USER_ID + "pw" + Global.USER_PW);
            CNetwork.GetInstance().CS_JoinLoginUser(Global.USER_ID, Global.USER_PW);
        }


        //return ; 
    }




    public void UserInfo(string szMsg)
    {

        string[] components = (szMsg.Substring(szMsg.LastIndexOf("://") + 3)).Split('&');

        Global.USER_ID = Command("email", components);
        Global.USER_PW = Command("passwd", components);
        Global.USER_NUM = Command("unum", components);
        Global.USER_GONUM = Command("gonum", components);

        Debug.Log("id" + Global.USER_ID + "pw" + Global.USER_PW + "unum" + Global.USER_NUM + "gonum" + Global.USER_GONUM);

    }


    public void Logout()
    {
        Global.USER_ID = "";
        Global.USER_PW = "";
        Global.USER_NUM = "";
        Global.USER_GONUM = "";
        //m_isTerms = false;
        SaveAppInfo();
    }


    public void LoadAppInfo()
    {
        if (PlayerPrefs.HasKey("Terms") == true)
        {
            m_isTerms = System.Convert.ToBoolean(PlayerPrefs.GetInt("Terms"));
        }

        if (PlayerPrefs.HasKey("DirectID") == true)
        {
            m_isDirectID = System.Convert.ToBoolean(PlayerPrefs.GetInt("DirectID"));
        }

    }

    //ios -> Unity

    public void UserInfoMsg(string szMsg)
    {
        UserInfo(szMsg);
    }

    public void AppReciveMsg(string szMsg)
    {
        Debug.Log("AppReciveMsg " + szMsg);

        if (szMsg.Contains("result:") == true)
        {
            TSReciveInfo(szMsg);
            return;
        }

        string[] components = (szMsg.Substring(szMsg.LastIndexOf("://") + 3)).Split('&');
        string cmdstr = Command("method", components);

        TSReciveInfo("result:" + cmdstr);

    }

    public void SaveAppInfo()
    {

        PlayerPrefs.SetInt("Terms", System.Convert.ToInt32(m_isTerms));
        PlayerPrefs.SetInt("DirectID", System.Convert.ToInt32(m_isDirectID));
        PlayerPrefs.Save();
    }

    public string Command(string str, string[] components)
    {
        string word;

        foreach (string s in components)
        {
            word = s.Substring(0, s.IndexOf('='));
            if (word.Equals(str))
            {
                int StartIndex = s.IndexOf('=') + 1; // +1??-> ("=")
                return s.Substring(StartIndex, (s.Length - StartIndex));
            }
        }

        return "";
    }


    static GameApplication instance;
    public static GameApplication GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameApplication>();
            if (instance == null)
            {
                instance = new GameObject("GameApplication").AddComponent<GameApplication>();
            }
        }
        return instance;
    }


}

