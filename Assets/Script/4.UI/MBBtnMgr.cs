using UnityEngine;
using System.Collections;

//#define DEV_TEST;

public class MBBtnMgr : MonoBehaviour {

    bool bPush = false;

    //bool DevTest = false; 


    void OnClick()
    {


        //
        // by sdh 01 팝업
        //
        if (gameObject.name == "PopUpCloseBtn")
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            CPopupMgr.instance.Close();
        }
        else if (gameObject.name == "PopUpShowBtn")
        {
            //by sdh 팝업
            CPopupMgr.instance.bCheckToday = !CPopupMgr.instance.bCheckToday;

            ButtonActive(gameObject, CPopupMgr.instance.bCheckToday);

            //Application.OpenURL(GameClient.instance.mWebPopupButtonLink);
        }
        else if (gameObject.name == "PopUpActionBtn")
        {
            CPopupMgr.instance.PopupAction();
        }
        else if (gameObject.name == "WebCloseBtn")
        {
            CPopupMgr.instance.bOnWebview = false; 
            
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        ////////////////////////////////////////////////////


        //Debug.Log("OnClick()");
        if (LoadingBar.GetInstance().GetLock() == true)
            return;
        //if (GameClient.instance.tutorial.isVisitor == true) return;
        if (GameClient.instance.isVisitor == true)
        {
            return;
        }

        
        if (CPopupMgr.instance.bOnWebview == true) return;




        if (gameObject.name == "StartButton")
        {

            if (MysteryMgr.Instance.c_resultView.bOnResultView == true) return;

            if (MysteryMgr.Instance.em_playtype == PLAYTYPE._BONUS) return;

            GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_Button);

            if (MysteryMgr.Instance.mUseCoinx2 == true && MysteryMgr.Instance.mCreditCoin < 2) return;

            if (MysteryMgr.Instance.mCreditCoin <= 0)
                return;

            
            

            //Debug.Log(MysteryMgr.Instance.mCamera.GetComponent<MysteryCamera>().bCameraMove);
            
            if (MysteryMgr.Instance.mCamera.GetComponent<MysteryCamera>().bCameraMove == true)
            {
                return;
            }

            

            LoadingBar.GetInstance().SetLock(true);

            if (GameClient.mNetwork == true && GameClient.instance.isVisitor == false)
            {
                //연동 플레이~
                CNetwork.GetInstance().CS_Play(MysteryMgr.Instance.em_playtype, StartOnComplete);
                
            }
            else
            {
                StartOnComplete();
            }
            
            
        }
        else if (gameObject.name == "DoubbleButton")
        {
            if (GameClient.instance.mMysteryState == MysteryState.InGame) return;

            if (MysteryMgr.Instance.c_resultView.bOnResultView == true) return;

            //20140821 ljw 무료게임추가 //무료게임 진행시 x2버튼을 막는다.
            if (MysteryMgr.Instance.em_playtype == PLAYTYPE._BONUS)
            {
                return;
            }

            if (MysteryMgr.Instance.mCreditCoin >= 2)
            {
                MysteryMgr.Instance.mUseCoinx2 ^= true;
                MysteryMgr.Instance.em_playtype = (MysteryMgr.Instance.mUseCoinx2 == true) ? PLAYTYPE._DOUBLE : PLAYTYPE._DEFAULT;

                if (MysteryMgr.Instance.mUseCoinx2 == true)
                {
                    MysteryMgr.Instance.mDoubleToggle = 2;
                }
                else
                {
                    MysteryMgr.Instance.mDoubleToggle = 1;
                }


                MBUIController.Instance.RefreshMultiplier();
                Debug.Log("DoubleBtn RefreshMulti");

                ButtonActive(gameObject, MysteryMgr.Instance.mUseCoinx2);

                MBUIController.Instance.CoinTextUpdate();

                StartCoroutine(DoubbleCoinSound());

                //Debug.Log(ConstValue.GetMaxTicketValue());

                // 게임 종료 후 더블버튼 비활성화 해야함
            }
            else
            {
                MysteryMgr.Instance.mUseCoinx2 = false;
                MysteryMgr.Instance.mDoubleToggle = 1;
                MBUIController.Instance.RefreshMultiplier();
                MysteryMgr.Instance.em_playtype = PLAYTYPE._DEFAULT;
            }
            
        }
        else if (gameObject.name == "CoinButton")
        {

            if (MysteryMgr.Instance.c_resultView.bOnResultView == true) return;

            //20140821 ljw 무료게임추가 //무료게임 진행시 코인버튼을 막는다
            if (MysteryMgr.Instance.em_playtype == PLAYTYPE._BONUS)
            {
                return;
            }

            if (GameClient.instance.mUserCoin < GameClient.instance.GetMachineData().theNeedCoin)
            {
#if UNITY_ANDROID
                MessageBox.Instance.OpenMessageBox(emMsgType.COIN_NOT_ENOUGH);   
                return;
                
#elif UNITY_IOS
                //모바일 웹페이지
                if(GameClient.mGameState != GameState.Mystery){
                     CPopupMgr.instance.OpenWebView(true, LobbyMgr.Instance.webView);
                } else 
                {
					CPopupMgr.instance.OpenWebView(true, MysteryMgr.Instance.webview);
                }
               
                return;
#endif
            }

            ButtonActive(gameObject, false);

            gameObject.transform.Find("CoinEffect").gameObject.SetActive(true);
            
            //gameObject.transform.Find("BombAni").gameObject.SetActive(true);
            MysteryMgr.Instance.mCreditCoin++ ;
            
            int Select = GameClient.instance.mSelectMachine;

            MachineData data = (MachineData)GameClient.instance.mMachinelist[Select];

            GameClient.instance.mUserCoin -= data.theNeedCoin;


            //유저코인 차감한다.. 

            //GameClient.instance.mUserCoin;

            MBUIController.Instance.CoinTextUpdate();
            MysteryMgr.Instance.mUIUserMoneyLabel.text = GameClient.instance.mUserCoin.ToString();


            GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_InsertCoin);
            //NGUITools.PlaySound(GameClient.instance.mSnd_InsertCoin);

        }
        else if (gameObject.name == "CoinChargeBtn")
        {
            if (MysteryMgr.Instance.c_resultView.bOnResultView == true) return;

#if UNITY_ANDROID
            MessageBox.Instance.OpenMessageBox(emMsgType.PLATFORM);
            
#elif UNITY_IOS
             //모바일 웹페이지

            if (GameClient.mGameState != GameState.Mystery)
            {
				CPopupMgr.instance.OpenWebView(true, LobbyMgr.Instance.webView);
            }
            else
            {
				CPopupMgr.instance.OpenWebView(true, MysteryMgr.Instance.webview);
            }
#endif
        }
        else if (gameObject.name == "TicketPaper")
        {

            if (MysteryMgr.Instance.c_resultView.bOnResultView == true) return;

#if UNITY_ANDROID
            MessageBox.Instance.OpenMessageBox(emMsgType.GETTICKET);
#elif UNITY_IOS
            return;
#endif
        }
        else if (gameObject.name == "TicketIcon")
        {

            if (MysteryMgr.Instance.c_resultView.bOnResultView == true) return;
            
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

    }		
    
    //서버에서 확답이 오면....
    void StartOnComplete()
    {
        
        if (MBUIController.Instance.isStartGame == true)
        {
            return;
        }
        
        MBUIController.Instance.GameStart();
    }

    //포커싱이 안되었을때 다시 들어온다. 
    void OnPress()
    {
        if (GameClient.instance.isVisitor == true)
        {
            return;
        }
        bPush ^= true; 

        if (gameObject.name == "StartButton")
        {
            ButtonActive(gameObject, bPush);
        }
        else if (gameObject.name == "DoubbleButton")
        {
            ButtonActive(gameObject, bPush);
        }
        else if (gameObject.name == "CoinButton")
        {
            ButtonActive(gameObject, bPush);
        }
    }


    public void ButtonActive(GameObject obj , bool bPush)
    {
        if (bPush == true)
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(false);
            obj.transform.Find("PressButton").gameObject.SetActive(true);
        }
        else
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(true );
            obj.transform.Find("PressButton").gameObject.SetActive(false);
        }
    }

    void OnRelease()
    {
        Debug.Log("OnRelease");
    }

    //2014.04.16 이재우 X2 버튼 사운드 출력 수정
    IEnumerator DoubbleCoinSound()
    {
        if (MysteryMgr.Instance.mUseCoinx2 == true)
        {
            GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_x2On);
        }
        else
        {
            GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_x2Off);
        }
        
        //NGUITools.PlaySound(GameClient.instance.mSnd_InsertCoin);

        yield return new WaitForSeconds(0.2f);

        //GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_InsertCoin);
        //Debug.Log("second");
        //NGUITools.PlaySound(GameClient.instance.mSnd_InsertCoin);
    }

}
