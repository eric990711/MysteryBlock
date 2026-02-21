using UnityEngine;
using System.Collections;

public class OptionBtnMgr : MonoBehaviour
{

    public GameObject OptionWnd;

    bool bOption = false;

    bool bPush = false;




    void OnEnable()
    {


        switch (gameObject.name)
        {
            case "EffectButton": CheckButton(ref GameClient.instance.mbEffectSound); break;
            case "SoundButton": CheckButton(ref GameClient.instance.mbBGMSound); break;
            case "GSensorButton": CheckButton(ref GameClient.instance.mbTilt); break;
            case "VibraitonButton": CheckButton(ref GameClient.instance.mbVibration); break;

        }



        if (gameObject.name == "ExitButton")
        {
            if (GameClient.mGameState == GameState.Lobby)
                transform.Find("Text").GetComponent<UILabel>().text = "EXIT";
            else if (GameClient.mGameState == GameState.Mystery)
                transform.Find("Text").GetComponent<UILabel>().text = "LOBBY";
        }
    }

    void OnClick()
    {
        if (LoadingBar.GetInstance().GetLock() == true)
            return;
   
        if (GameClient.instance.isVisitor == true)
        {
            return;
        }

        if (CPopupMgr.instance.bOnWebview == true) return;

        // if (GameClient.instance.tutorial.isVisitor == true) return;
        if (GameClient.instance.mMysteryState == MysteryState.Result
            && GameClient.mGameState == GameState.Mystery)
            return;

        if (LoadingBar.GetInstance().GetLock() == true)
            return;


        GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_Button);

        if (gameObject.name == "OptionButton")
        {
            //if (GameClient.instance.tutorial.isVisitor == true) return;
            GameClient.instance.IsPause = !GameClient.instance.IsPause;

            OptionWnd.SetActive(GameClient.instance.IsPause);

        }
        else if (gameObject.name == "EffectButton")
        {
            GameClient.instance.mbEffectSound = !GameClient.instance.mbEffectSound;

            CheckButton(ref GameClient.instance.mbEffectSound);

            //GameClient.instance.PlayBGM();
        }
        else if (gameObject.name == "SoundButton")
        {
            GameClient.instance.mbBGMSound = !GameClient.instance.mbBGMSound;

            CheckButton(ref GameClient.instance.mbBGMSound);

            if (GameClient.instance.mbBGMSound == true)
                GameClient.instance.PlayBGM();
            else
                GameClient.instance.StopBGM();

        }
        else if (gameObject.name == "GSensorButton")
        {
            GameClient.instance.mbTilt = !GameClient.instance.mbTilt;
            CheckButton(ref  GameClient.instance.mbTilt);
        }
        else if (gameObject.name == "VibraitonButton")
        {
            GameClient.instance.mbVibration = !GameClient.instance.mbVibration;
            CheckButton(ref  GameClient.instance.mbVibration);
        }
        else if (gameObject.name == "OKButton")
        {
            OptionWnd.SetActive(false);
            GameClient.instance.IsPause = false;
            GameClient.instance.SaveUserOption();
        }
        else if (gameObject.name == "LobbyButton")
        {
        }
        else if (gameObject.name == "CloseButton")
        {
            OptionWnd.SetActive(false);
            GameClient.instance.IsPause = false;
            GameClient.instance.SaveUserOption();
        }
        else if (gameObject.name == "TSButton")
        {
            
            if (GameClient.mGameState == GameState.Lobby)
            {
 
#if UNITY_ANDROID
                MessageBox.Instance.OpenMessageBox(emMsgType.PLATFORM);
                
#elif UNITY_IOS
                MessageBox.Instance.OpenMessageBox(emMsgType.BACK_BUTTON);
#endif
            }
            else if (GameClient.mGameState == GameState.Mystery)
            {
                MessageBox.Instance.OpenMessageBox(emMsgType.INGAMEEXIT);
                // MessageBox.SetActive(true);
                //OptionWnd.SetActive(false);
            }
        }
        else if (gameObject.name == "LogoutButton")
        {
            MessageBox.Instance.OpenMessageBox(emMsgType.LOGOUT);
            OptionWnd.SetActive(false);
        }
        else if (gameObject.name == "HelpButton")
        {
            GameClient.instance.IsPause = false;
            HelpBox.Instance.OpenHelpBox();
            OptionWnd.SetActive(false);
        }
        else if (gameObject.name == "TermsButton")
        {
            HelpBox.Instance.OpenTermView();
            OptionWnd.SetActive(false);
        }
        else if (gameObject.name == "CoinBuyBtn")
        {
#if UNITY_ANDROID
            MessageBox.Instance.OpenMessageBox(emMsgType.COIN_CHARGE);
            
#elif UNITY_IOS
            if (GameApplication.GetInstance().m_isPlatform == true)
            {
                MessageBox.Instance.OpenMessageBox(emMsgType.COIN_CHARGE);
            }
            else
            {
                //모바일 웹페이지
                if (GameClient.mGameState != GameState.Mystery)
                {
                    
					CPopupMgr.instance.OpenWebView(true, LobbyMgr.Instance.webView);
                }
                else
                {
					CPopupMgr.instance.OpenWebView(true, MysteryMgr.Instance.webview);
                }
            }
#endif
        }
        else if (gameObject.name == "MyTicket")
        {
#if UNITY_ANDROID
            MessageBox.Instance.OpenMessageBox(emMsgType.GETTICKET);
#elif UNITY_IOS
            if (GameApplication.GetInstance().m_isPlatform == true)
            {
            MessageBox.Instance.OpenMessageBox(emMsgType.GETTICKET);
            }
            else
            {
                return;
            }
#endif
        }
        else if (gameObject.name == "5LinkText")
        {
            Application.OpenURL("mailto:ticketspace-as@ulike.co.kr?subject=Email&body=Email%0D%0ABod");

            //"mailto:someone@gmail.com?subject=EmailSubject&body=Email%0D%0ABody" body=from Unity
        }
        //terms OK
        else if (gameObject.name == "AgreeBtn")
        {
            HelpBox.Instance.CloseTermView();
        }


    }
    void OnPress()
    {
        if (GameClient.instance.isVisitor == true)
        {
            return;
        }

        bPush ^= true;

        if (gameObject.name == "SoundButton" ||
            gameObject.name == "EffectButton" ||
            gameObject.name == "GSensorButton" ||
            gameObject.name == "VibraitonButton")
            return;


        if (gameObject.name == "OptionButton")
        {
            //if (GameClient.instance.tutorial.isVisitor == true) return;
            ButtonActive(gameObject, bPush);
        }
        else
        {
            ButtonActive(gameObject, bPush);
        }

    }


    void CheckButton(ref bool flag)
    {
        ButtonActive(gameObject, flag);
        GameClient.instance.SaveUserOption();
    }


    void ButtonActive(GameObject obj, bool bPush)
    {
        if (obj.transform.Find("NormalButton") == false ||
            obj.transform.Find("PressButton") == false)
            return;

        if (bPush == true)
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(false);
            obj.transform.Find("PressButton").gameObject.SetActive(true);
        }
        else
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(true);
            obj.transform.Find("PressButton").gameObject.SetActive(false);
        }
    }


}
