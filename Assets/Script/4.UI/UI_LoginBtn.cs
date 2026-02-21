using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;


/*
static class _UPROTOCOL
{
    public const string LOGIN = "http://www.hello5.kr:8080/spaceticket/module/loginUser.jsp";
    public const string ENTERGAME = "http://www.hello5.kr:8080/spaceticket/module/enterGame.jsp";
}
*/


public class UI_LoginBtn : MonoBehaviour
{

    public GameObject mLogin = null;
    public GameObject mTerms = null;
    public GameObject mstart = null;


    public UIInput m_inAcc;
    public UIInput m_inPass;

    // Use this for initialization
    void Awake()
    {

#if UNITY_ANDROID
        CAndroidManager.GetInstance();
#endif

    }

    void OnClick()
    {
        if (gameObject.name == "LoginBtn")
        {
            //연동 직접 로그인
            CNetwork.GetInstance().CS_JoinLoginUser(m_inAcc.value, m_inPass.value);
        }
        else if (gameObject.name == "AgreeBtn")
        {

            /*
            PlayerPrefs.SetString("play_agree", "true");
#if UNITY_ANDROID
            CAndroidManager.GetInstance().startGame(startGame);
#elif UNITY_IOS
			iOSManager.GetInstance().StartGame();
#endif
            */

            //연동 스타트게임
            if (LoadingBar.GetInstance().GetLock() == true)
                return;

            LoadingBar.GetInstance().SetLock(true);

            GameApplication.GetInstance().m_isTerms = true;
            GameApplication.GetInstance().SaveAppInfo();
            CNetwork.GetInstance().StartGame();

            //GameClient.mGameState = GameState.Lobby;
            //GameClient.instance.LoadingScene();
        }
        else if (gameObject.name == "AlreadyBtn")
        {
            mstart.SetActive(false);
            mLogin.SetActive(true);
        }
        else if (gameObject.name == "StartBtn")
        {
            //PlayerPrefs.SetString( "user_id", (string)args[2] );
            //

            // Terms/Privacy 화면 skip - 바로 게임 시작
            CNetwork.GetInstance().StartGame();

            //if (PlayerPrefs.GetString("play_agree", "false").Equals("false"))
        }

    }

    void OnPress()
    {

    }

    

    void startGame(object obj)
    {
        //string szBuf = (string)obj;
        //Debug.LogError("startGame : "+szBuf);

        SceneManager.LoadScene("LobbyStage");

        /*
            if (result.equals("doPlay"))				
            {
                // 플랫폼에서 게임 실행
                Send("setCompletion|doPlay");
            }
            else if (result.equals("startGame"))
            {
                // START 해야함
                Send("setCompletion|startGame");
            }
            else Send("setCompletion|"+result);
         */

    }
}
