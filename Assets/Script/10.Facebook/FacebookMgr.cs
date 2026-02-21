using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum emMysteryType
{
    BLOCK,
    OFFRODAER,
    DOD,
    PLINKO,
    HOTFIVE,
    COLOUR,
    WINNERBALL,
}

public enum emFacebookMsgMode
{
    JACKPOT,
    RESULT,
}


public class FacebookMgr : MonoBehaviour
{


    private bool isInit = false;
    private string lastResponse = "";

    public emMysteryType mMysteryType;

    emFacebookMsgMode mMsgMode;


    private string FeedLink = "";
    private string FeedLinkName = "";
    private string FeedLinkCaption = "";
    private string FeedLinkDescription = "";
    private string FeedPicture = "";
    /*
     public string FeedMediaSource = "";
     public string FeedActionName = "";
     public string FeedActionLink = "";
     public string FeedReference = "";
     */

    private bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();






    //초기화
    public void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
        isInit = true;
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }



    //로그인 
    public void CallFBLogin()
    {
        FB.Login("email,publish_actions", LoginCallback);
    }

    void LoginCallback(FBResult result)
    {
        if (result.Error != null)
            lastResponse = "Error Response:\n" + result.Error;
        else if (!FB.IsLoggedIn)
        {
            lastResponse = "Login cancelled by Player";
        }
        else
        {

            CallFBPublishInstall();

            lastResponse = "Login was successful!";
        }
    }

    //로그아웃
    public void CallFBLogout()
    {
        FB.Logout();
    }

    //스샷
    public void CallTakeScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }


    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
        wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");

        FB.API("me/photos", Facebook.HttpMethod.POST, Callback, wwwForm);
    }


    //퍼블리셔 인스톨 저장하기인가 ?? 
    public void CallFBPublishInstall()
    {
        FB.PublishInstall(PublishComplete);
    }

    private void PublishComplete(FBResult result)
    {
        Debug.Log("publish response: " + result.Text);
    }


    void Callback(FBResult result)
    {
        // Some platforms return the empty string instead of null.
        if (!String.IsNullOrEmpty(result.Error))
            lastResponse = "Error Response:\n" + result.Error;

        Debug.Log(result.Text);
        if (result.Text.Contains("cancelled")) return;

        //20140810 ljw 쉐어
        if (GameClient.mNetwork == true)
        {
            CNetwork.GetInstance().CS_SendShare(CallbackSendShare);
        }
        else
        {
            CallbackSendShare();
        }


    }

    void CallbackSendShare()
    {

        //by ljw 20140813 페이스북 공유시 메세지 
        OpenFBWnd(mMsgMode);
        GameClient.instance.mMysteryState = MysteryState.Stay;

    }

    //by ljw 20140813 페이스북 공유시 메세지 
    void OpenFBWnd(emFacebookMsgMode mode)
    {
        switch (mode)
        {
            case emFacebookMsgMode.RESULT:
                MysteryMgr.Instance.mUIResultView.gameObject.SetActive(false);
                MessageBox.Instance.OpenMessageBox(emMsgType.FBSHARERESULT, emBtnType.OK);

                break;
            case emFacebookMsgMode.JACKPOT:
                MysteryMgr.Instance.c_resultView.gameObject.SetActive(false);
                //20140818 ljw 무료게임
                //MysteryMgr.Instance.isBonusGame = true;

                //20140826 ljw 무료게임최대치 추가
                if (GameClient.mNetwork == true)
                {
                    if (GameClient.instance.curShare < GameClient.instance.maxShare) MessageBox.Instance.OpenMessageBox(emMsgType.FBSHAREWIN, emBtnType.OK);
                    else MessageBox.Instance.OpenMessageBox(emMsgType.FBSHAREFULL, emBtnType.OK);
                }
                else
                {
                    MessageBox.Instance.OpenMessageBox(emMsgType.FBSHAREWIN, emBtnType.OK);
                }
                


                break;
        }
    }



    public void CallFBFeed(int ticket, emFacebookMsgMode mode)
    {
        //로그인이 안돼어 있다면
        if (FB.IsLoggedIn == false)
        {
            CallFBLogin();
            return;
        }

        FeedLink = GameApplication.GetInstance().StoreName();

        if (Application.platform == RuntimePlatform.Android)
        {
            FeedLinkCaption = (GameApplication.GetInstance().market == Market.PLAYSTORE) ? "Play.Google.Com" : "Tstore.co.kr";
        }
        else
        {
            FeedLinkCaption = "itunes.apple.com";
        }

        int GameNameIndex = (int)emString.MysteryBlock + (int)mMysteryType;

        FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText((emString)GameNameIndex);

        switch (mMysteryType)
        {
            case emMysteryType.BLOCK:
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mb150.png";
                break;
            case emMysteryType.OFFRODAER:
                // FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText(emString.MysteryOffRoder);
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mo150.png";
                break;
            case emMysteryType.DOD:
                //  FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText(emString.MysteryDOD);
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mbd150.png";
                break;
            case emMysteryType.PLINKO:
                //   FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText(emString.MysteryPlinko);
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mp150.png";
                break;
            case emMysteryType.COLOUR:
                //   FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText(emString.MysteryColour);
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mc150.png";
                break;
            case emMysteryType.HOTFIVE:
                //  FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText(emString.MysteryHotFive);
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mh150.png";
                break;
            case emMysteryType.WINNERBALL:
                //  FeedLinkName = "Ticket Space " + TextManager.GetInstance().GetText(emString.MysteryWinnerBall);
                FeedPicture = "https://s3-us-west-1.amazonaws.com/ticketspace/images/game/mw150.png";
                break;

        }

        if (mode == emFacebookMsgMode.JACKPOT)
        {
            FeedLinkDescription = string.Format(TextManager.GetInstance().GetText(emString.GetTicketAndJackPot), TextManager.GetInstance().GetText((emString)GameNameIndex), ticket);
        }
        else
        {
            FeedLinkDescription = string.Format(TextManager.GetInstance().GetText(emString.GetTicketAndResult), TextManager.GetInstance().GetText((emString)GameNameIndex), ticket);
        }

        mMsgMode = mode;


        Dictionary<string, string[]> feedProperties = null;

        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }

        FB.Feed(
            toId: "",
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: "",
            actionName: "",
            actionLink: "",
            reference: "",
            properties: feedProperties,
            callback: Callback
        );
    }

    void Awake()
    {
        FeedProperties.Add("key1", new[] { "valueString1" });
        FeedProperties.Add("key2", new[] { "valueString2", "http://www.facebook.com" });

        // Facebook SDK (IFacebook.dll) is incompatible with Unity 6 - skip init
        // CallFBInit();
        DontDestroyOnLoad(this);

    }

    public bool IsLogin()
    {
        return FB.IsLoggedIn;
    }


    static FacebookMgr instance;

    public static FacebookMgr GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<FacebookMgr>();

            if (instance == null)
            {
                instance = new GameObject("FacebookMgr").AddComponent<FacebookMgr>();
            }
        }
        return instance;
    }
}
