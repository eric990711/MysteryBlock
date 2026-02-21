//****************************************
//                              2014.03.25
//               iamboss
//
//****************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum PARSETYPE { _BOOL, _STRING, _END };
public class CAndroidManager : MonoBehaviour
{
#if UNITY_ANDROID
    private AndroidJavaObject curActivity;
#endif

    private string m_szMessage = null;
    List<string> m_liBox = new List<string>();

    //
    // 개채 가져오기
    //

    static CAndroidManager _instance;
    public static CAndroidManager GetInstance()
    {
        Debug.LogWarning("CAndroidManager GetInstance");
        if (_instance == null)
            _instance = new GameObject("AndroidManager").AddComponent<CAndroidManager>();
        return _instance;
    }

    //
    // 셋업
    //
    void Awake()
    {
        ///&lt; 현재 활성화된 액티비티 얻어와서 저장
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        Debug.Log("AndroidManager Created");
#endif
        Object.DontDestroyOnLoad(this);
    }

    public void Init()
    {

    }


    //
    // 매니지먼트
    //
    void Update()
    {

        /*      if (Input.GetKeyDown(KeyCode.A))
                {
                    GetMessage("getUserData|Completed");
                    GetMessage("getUserData|Success|{\"data\":{\"U_TICKET_CNT\":2147483647,\"U_COIN_CNT\":540,\"U_SAVE_TICKET_CNT\":923672},\"result\":\"ok\"}");
                    Debug.Log(m_szMessage);
                }

                if (Input.GetKeyDown(KeyCode.B))
                {
                    ClearMessage();
                }*/

        //다음 메세지
        if ((m_szMessage == null) && (m_liBox.Count != 0))
        {
            m_szMessage = new string(m_liBox[0].ToCharArray());
            m_liBox.RemoveAt(0);
            Debug.LogWarning("Next : " + m_szMessage);
        }
    }

    //
    // 안드로이드 함수 호출
    //
    public bool Call(string szFuncName, params object[] args)
    {
        Debug.LogWarning("Call : " + szFuncName);
#if UNITY_ANDROID
        if (curActivity == null)
        {
            Debug.Log("안드로이드 액티비티 입력없음");
            //StartCoroutine(GameClient.instance.ErrMessageBox("Android no Activity")); 
            return false;
        }
        if (args.Length != 0) curActivity.Call(szFuncName, args);
        else curActivity.Call(szFuncName);
#endif
        return true;
    }

    //
    // 메세지 받기
    //
    public void GetMessage(string param)
    {
        Debug.LogWarning("Get : " + param);

        if (m_szMessage == null) { m_szMessage = param; Debug.Log("Set"); }
        else { m_liBox.Add(param); Debug.Log("Add : " + m_liBox.Count); }
        //
    }

    //
    // 메세지 제거
    //
    public void ClearMessage()
    {
        m_szMessage = null;
        Debug.Log("Clear : " + m_szMessage);
    }

    //
    // 파싱함수
    //
    public delegate void Parse(object Result);

    //
    // 메세지 대기
    //
    public IEnumerator Wait(
        Parse parse, PARSETYPE parsetype,
        string szDefault, string szFuncName, string szWait,
        params object[] args)
    {
        string[] szBuf;
        string szTemp;
        int nSize;




        Debug.Log("Wait:enter : " + szFuncName);

        //내용 받기
        if (Call(szFuncName, args) == false)
        {

            //GameClient.instance.OpenErrBox(m_szMessage);
            //Debug.LogWarning("Wait:Fail : " + szFuncName);

            if (szDefault == null) yield break;
            GetMessage(szDefault);
        }

        //기다려
        Debug.Log("Wait:start : " + szWait);
        nSize = szWait.Length;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            //03-26 23:06:42.104: W/Unity(3943): Clear : loginUser|Completed
            if ((m_szMessage != null) && m_szMessage.Equals(szWait + "|Completed"))
                ClearMessage();
            if ((m_szMessage != null) && (m_szMessage.Length >= nSize))
            {

                if (m_szMessage.Substring(0, nSize).Equals(szWait)) break;
            }
        }

        //결과입력
        szBuf = m_szMessage.Split('|');
        szTemp = m_szMessage.Substring(m_szMessage.IndexOf('|') + 1);

        ClearMessage();
        if (parse != null)
        {
            switch (parsetype)
            {
                case PARSETYPE._BOOL: parse(szBuf[1].Equals("1")); break;
                case PARSETYPE._STRING: parse(szTemp); break;
            }
        }

        yield break;
    }
    public IEnumerator WaitNokey(
        Parse parse, PARSETYPE parsetype,
        string szDefault, string szFuncName, params object[] args)
    {

        yield return StartCoroutine(

            Wait(parse, parsetype, szDefault, szFuncName, szFuncName, args));

        yield break;
    }

    //
    // 로그인중인가
    //
    public void isLogin(Parse parse)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._BOOL, "isLogin|0", "isLogin"));
    }

    //
    // 자체로그인
    //
    public void loginUser(Parse parse, string U_EMAIL, string U_PW)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "loginUser", U_EMAIL, U_PW));
    }

    //
    // 간편로그인
    //
    public void startGame(Parse parse)
    {
        StartCoroutine(Wait(parse, PARSETYPE._STRING, null, "startGame", "setCompletion"));
    }

    //
    // 플랫폼으로 이동
    //
    public void goPlatform()
    {
        Debug.Log("Go PlatForm");
        Call("goPlatform");
    }

    //
    // 플랫폼 설치 여부
    //
    public void checkPlatform()
    {
        Call("checkPlatform");
    }

    // 
    // 앱이동 
    //
    public void goApp(string packageName, string pid, bool tstoreState)
    {
        Call("goApp", packageName, pid, tstoreState);
    }

    //
    // 페이지로 이동
    //
    public void goURL(string url)
    {
        Call("goURL", url);
    }


    //
    // 로그아웃
    //
    public void logout(Parse parse)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._BOOL, "logout:0", "logout"));
    }

    //
    // 게임 플레이
    //
    public void playGame(Parse parse, string PLAY_TYPE, string GO_NUM)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null,
            "playGame", PLAY_TYPE, GO_NUM));
    }

    //튜토리얼 완료
    public void tutorial(Parse parse)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "tutorial"));
    }

    //
    // 게임종료
    //
    public void finishGame(Parse parse, long U_REWARD_TICKET_CNT)
    {
        Debug.Log("finishGame");
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null,
            "finishGame", U_REWARD_TICKET_CNT));
    }

    public void finishGame2(Parse parse, long U_REWARD_TICKET_CNT)
    {
#if UNITY_ANDROID
        curActivity.Call("finishGame2", U_REWARD_TICKET_CNT);
#endif

    }

    //
    // 머신선택
    //
    public void setGame(string GO_NUM)
    {
        Debug.Log("setGame + Gonum ::::: " + GO_NUM);
        Call("setGame", GO_NUM);
    }

    //
    // 머신 목록
    //
    public void getMachineList(Parse parse)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "getMachineList"));
    }

    //
    // 랭킹
    //
    public void getRank(Parse parse)
    {
        StartCoroutine(
            WaitNokey(parse, PARSETYPE._STRING, null, "getRank"));
    }

    //
    // 유저 정보
    //
    public void getUserData(Parse parse)
    {
        StartCoroutine(
            WaitNokey(parse, PARSETYPE._STRING, null, "getUserData"));
    }

    public void getSaveTicket(Parse parse)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "getSaveTicket"));
    }

    //
    // sdh 20140704 팝업
    //
    public void getBeforePopup(Parse parse)
    {
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "getBeforePopup"));
    }

    //
    // sdh 20140704 팝업
    //
    public void sendShare(Parse parse)
    {
        Debug.Log("CAndroidMgr sendShare");
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "sendShare"));
    }

    //20140919 ljw 플링코 전용 팝업
    public void getAfterPopup(Parse parse, long U_REWARD_TICKET)
    {
        Debug.Log("CAndroidMgr getAfterPopup");
        StartCoroutine(WaitNokey(parse, PARSETYPE._STRING, null, "getAfterPopup", U_REWARD_TICKET));
    }



    //***자체제작임. 기본지원 없음
    // 유저 정보
    //
    /* 
     * Success|
     * {"isLogin":true,
     * "GO_NUM":"",
     * "UserPassword":"Z45dFtn42Ow56Cg7fhmb",
     * "UserEmail":"ffffffff-a6be-a61b-ce36-ac260033c587",
     * "PackageName":"com.goticketspace.app.mysteryblock",
     * "checkPlatform":false}
     */
    public void getUserInfo(Parse parse)
    {
        StartCoroutine(
            WaitNokey(parse, PARSETYPE._STRING, null, "getUserInfo"));
    }

    //
    // 셋업
    //
    public void SetUp(Parse parse, PARSETYPE parsetype)
    { StartCoroutine(SetUp_(parse, parsetype)); }
    IEnumerator SetUp_(Parse parse, PARSETYPE parsetype)
    {
        string[] szBuf;
        string szTemp;
        int nSize;

        //기다려
        Debug.LogWarning("Wait:start : setCompletion");
        nSize = "setCompletion".Length;
        yield return new WaitForSeconds(1.0f);
        if (m_szMessage == null) m_szMessage = "setCompletion|startGame";
        else
        {
            Debug.LogWarning("Get : " + m_szMessage);
        }

        //결과입력
        szBuf = m_szMessage.Split('|');
        szTemp = m_szMessage.Substring(m_szMessage.IndexOf('|') + 1);
        ClearMessage();
        if (parse != null)
        {
            switch (parsetype)
            {
                case PARSETYPE._BOOL: parse(szBuf[1].Equals("1")); break;
                case PARSETYPE._STRING: parse(szTemp); break;
            }
        }

        Debug.LogWarning("Wait:leave : setCompletion");
        yield break;


    }


}


