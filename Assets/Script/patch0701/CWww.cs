//****************************************
//                              2014.03.05
//               iamboss
//
//****************************************
using UnityEngine;
using System.Collections;


//
// Defines
//
public enum PLAYTYPE { _DEFAULT, _DOUBLE, _BONUS, _END };

public enum NetType
{
    JOINLOGIN,
    LOGIN,
    TERMPAGE,
    USERDATA,
    LOBBY_ROOM,
    LOBBY_RANK,
    PLAY,
    FINISH,
    SAVE_TICKET,
    TUTORIAL,
    AFTERPOPUP,
    BEFOREPOPUP,    // sdh 20140704 팝업
    SENDSHARE //ljw 20140810 쉐어
}

public class CWww : MonoBehaviour
{

    static private CWww instance;
    public static CWww GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("WWW GetInstan");
            instance = new GameObject("CWww").AddComponent<CWww>();
        }
        return instance;
    }

    public string KEY = "";  //
    public string mLogstring = "";

    //
    // 파싱함수
    //
    public delegate void Parse(string text, NetType protocol);

    void Awake()
    {
        Object.DontDestroyOnLoad(this);
        Application.RegisterLogCallback(LogCallBack);
    }

    public void LogCallBack(string logString, string stackTrace, LogType type)
    {
        mLogstring = logString;
    }


    void CheckException()
    {
        if (mLogstring.IndexOf("Host not found") > 0)
        {
            Debug.Log("The server is under inspection. ");
        }
    }


    void Start()
    {
        Debug.Log(Global.USER_ID + " sss" + Global.USER_PW + " " + Global.G_PACK_NAME + " " + Global.APP_OS_TYPE.ToString());
    }

    //
    // 로그인
    // https://www.goticketspace.com:7443/TicketSpace/module/loginUser.jsp?U_EMAIL=test30@email.com&U_PW=111111

    //http://www.hello5.kr:8080/spaceticket/shop/getCoinByTapjoy.jsp?U_NUM=425&COIN=10


    public IEnumerator CoinTapJoy(string user_num, string coin, Parse parse)
    {

        WWWForm form = new WWWForm();

        form.AddField("NUM", user_num);
        form.AddField("COIN", coin);

        WWW w = new WWW(Global.HOST + "/shop/getCoinByTapjoy.jsp", form.data);

        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else
        {
            Debug.Log("Coin Charge : " + coin);
            //parse(w.text, NetType.JOINLOGIN);
        }


        CheckException();

        yield break;
    }


    public IEnumerator JoinLoingUser(string u_email, string u_pw, Parse parse)
    {

        WWWForm form = new WWWForm();

        form.AddField("ID", u_email);
        form.AddField("PW", u_pw);
        form.AddField("TYPE", "library");

        WWW w = new WWW(Global.HOST + "/user/joinOrLoginUser.jsp", form.data);

        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else
        {
            Global.USER_ID = u_email;
            Global.USER_PW = u_pw;

            parse(w.text, NetType.JOINLOGIN);
        }


        CheckException();

        yield break;
    }


    public IEnumerator loginUser(string u_email, string u_pw, Parse parse)
    {

        WWWForm form = new WWWForm();

        form.AddField("U_EMAIL", u_email);
        form.AddField("U_PW", u_pw);
        WWW w = new WWW(Global.HOST + "/module/loginUser.jsp", form.data);

        yield return w;

        if (w.error != null) CallErrBox.Instance.OpenErrMsg("unknown error");
        else
        {
            Global.USER_ID = u_email;
            Global.USER_PW = u_pw;

            parse(w.text, NetType.LOGIN);
        }


        CheckException();

        yield break;
    }

    public IEnumerator TermsPageData(string email, string pw, Parse parse)
    {

        WWWForm form = new WWWForm();
        string ostype = Global.APP_OS_TYPE.ToString();


        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);
        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);


        form.AddField("APP_TYPE", ostype);
        WWW w = new WWW(Global.HOST + "/" + Global.HOSTDIR + "/module/getTermsPageData.jsp", form.data);

        yield return w;

        if (w.error != null) CallErrBox.Instance.OpenErrMsg("unknown error");
        else
        {
            parse(w.text, NetType.TERMPAGE);
        }

        CheckException();

        yield break;

        //TERMPAGE
    }
    //


    //
    // 유저 정보
    //
    public IEnumerator UserData(Parse parse)
    {

        Debug.Log("UserData SC Send");



        WWWForm form = new WWWForm();

        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);
        WWW w = new WWW(Global.HOST + "/module/getUserData.jsp", form.data);
        yield return w;

        if (w.error != null) CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.USERDATA);

        CheckException();

        yield break;
    }

    //
    // 방번호
    //
    public IEnumerator readGameRoomListByGameNum(Parse parse)
    {
        WWWForm form = new WWWForm();


        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        WWW w = new WWW(Global.HOST + "/" +
            "module/readGameRoomListByGameNum.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.LOBBY_ROOM);

        CheckException();

        yield break;
    }

    //
    // 순위
    //
    public IEnumerator readGameRankListByGameOptionNum(Parse parse)
    {
        WWWForm form = new WWWForm();

        string gonum = GameClient.instance.GetMachineData().theNumber;

        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        form.AddField("GO_NUM", gonum);
        WWW w = new WWW(Global.HOST + "/" +
            "module/readGameRankListByGameOptionNum.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.LOBBY_RANK);

        CheckException();

        yield break;
    }

    public IEnumerator SaveTicket(Parse parse)
    {
        WWWForm form = new WWWForm();

        string gonum = GameClient.instance.GetMachineData().theNumber;

        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);

        WWW w = new WWW(Global.HOST + "/" + "module/getSaveTicket.jsp", form.data);

        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.SAVE_TICKET);

        CheckException();

    }


    //
    // 플레이
    //
    public IEnumerator playGame(PLAYTYPE PlayType, Parse parse)
    {
        string[] playtype = { "default", "double", "bonus" };
        WWWForm form = new WWWForm();

        Debug.Log("PlayGame type :" + playtype);

        string gonum = GameClient.instance.GetMachineData().theNumber;


        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);
        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        form.AddField("GO_NUM", gonum);
        form.AddField("GO_KEY", "");
        form.AddField("PLAY_TYPE", playtype[(int)PlayType]);
        WWW w = new WWW(Global.HOST + "/" +
            "module/playGame.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.PLAY);

        CheckException();

        yield break;
    }



    public IEnumerator tutorialFinish(Parse parse)
    {
        WWWForm form = new WWWForm();
        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);
        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        WWW w = new WWW(Global.HOST + "/module/setGuTutorial.jsp", form.data);

        yield return w;

        if (w.error != null)
        {
            CallErrBox.Instance.OpenErrMsg("Unknown error");
        }
        else
        {
            parse(w.text, NetType.TUTORIAL);
        }
        CheckException();
        yield break;

    }


    //
    // 종료
    // userticket 랭킹에 반영될점수
    public IEnumerator finishGame(string u_reward_ticket_cnt, Parse parse)
    {
        WWWForm form = new WWWForm();
        string gonum = GameClient.instance.GetMachineData().theNumber;

        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);

        form.AddField("U_REWARD_TICKET_CNT", u_reward_ticket_cnt);
        form.AddField("SUM_TICKET", "0");

        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        form.AddField("GO_NUM", gonum);
        form.AddField("KEY", KEY);
        form.AddField("LANGUAGE", Global.USER_LANGUAGE());


        WWW w = new WWW(Global.HOST + "/" +
            "module/finishGame.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.FINISH);

        CheckException();

        yield break;
    }

    public IEnumerator finishGame2(string u_reward_ticket_cnt, Parse parse)
    {
        WWWForm form = new WWWForm();
        string gonum = GameClient.instance.GetMachineData().theNumber;

        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);

        form.AddField("U_REWARD_TICKET_CNT", u_reward_ticket_cnt);
        form.AddField("SUM_TICKET", "0");

        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        form.AddField("GO_NUM", gonum);
        form.AddField("KEY", "TEST");
        form.AddField("LANGUAGE", Global.USER_LANGUAGE());

        WWW w = new WWW(Global.HOST + "/" +
            "module/finishGame2.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.FINISH);

        CheckException();

        yield break;
    }



    // sdh 20140704 팝업

    public IEnumerator BeforePopup(Parse parse)
    {
        WWWForm form = new WWWForm();
        string gonum = GameClient.instance.GetMachineData().theNumber;



        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);
        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        form.AddField("LANGUAGE", Global.USER_LANGUAGE());

        WWW w = new WWW(Global.HOST + "/" + "module/getBeforePopup.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.BEFOREPOPUP);

        CheckException();

        yield break;
    }


    //20140919 ljw 플링코 전용 팝업
    public IEnumerator GetAfterPopup(string U_REWARD_TICKET, Parse parse)
    {

        WWWForm form = new WWWForm();

        string gonum = GameClient.instance.GetMachineData().theNumber;

        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);
        form.AddField("G_PACK_NAME", Global.G_PACK_NAME);
        form.AddField("GO_NUM", gonum);
        form.AddField("APP_TYPE", Global.APP_OS_TYPE.ToString());
        form.AddField("U_REWARD_TICKET_CNT", U_REWARD_TICKET);
        form.AddField("SUM_TICKET", "0");
        form.AddField("LANGUAGE", Global.USER_LANGUAGE());

        WWW w = new WWW(Global.HOST + "/" + "module/getAfterPopup.jsp", form.data);
        yield return w;

        if (w.error != null) CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.AFTERPOPUP);

        CheckException();

        yield break;
    }

    //20140810 ljw 쉐어
    public IEnumerator SendShare(Parse parse)
    {
        WWWForm form = new WWWForm();

        form.AddField("U_EMAIL", Global.USER_ID);
        form.AddField("U_PW", Global.USER_PW);

        WWW w = new WWW(Global.HOST + "/" + "module/sendShare.jsp", form.data);
        yield return w;

        if (w.error != null)
            CallErrBox.Instance.OpenErrMsg("unknown error");
        else parse(w.text, NetType.SENDSHARE);

        CheckException();
        yield break;

    }

}
