using UnityEngine;
using System;
using System.Collections;

public enum TutorialDialog
{
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q
}


public class TutorialLobby : MonoBehaviour
{
    public bool isBtnLock = false;
    public bool isClearMission = true;
    bool isTouch = false;
    bool bNextDialog = false;


    public TutorialModelController MconLobby;

    string sTutmsgKor = null;
    string sTutmsgEng = null;
    

    GameObject TextDescParent;

    public GameObject Nextbtn = null;
    public GameObject shield = null;
    public GameObject DummyUI_Panel = null;
    public GameObject [] FingerTouch;
    public GameObject []DeleteObj = null;

    
    public GameObject leftTextDesc = null;
    public GameObject rightTextDesc = null;

    public GameObject eventArrow = null;

    //E번 단계에서 버튼이 반짝이는 부분
    IEnumerator EventButtonFlash(GameObject obj)
    {
        Debug.Log("Start eventBtnFlash");
        obj.SetActive(true);
        
        while (isClearMission == false)
        {
            obj.transform.Find("NormalButton").gameObject.SetActive(true);
            obj.transform.Find("PressButton").gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            obj.transform.Find("NormalButton").gameObject.SetActive(false);
            obj.transform.Find("PressButton").gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            if (isClearMission == true)
            {
                tutDialog++;
                TutDialog = tutDialog;
                DummyUI_Panel.transform.GetChild(0).gameObject.SetActive(false);
            }

        }
    }


    IEnumerator DimAlpha()
    {
        shield.GetComponentInChildren<TweenAlpha>().enabled = true;

        while (isClearMission == false)
        {
            yield return null;
            if (isClearMission == true)
            {
                shield.GetComponentInChildren<TweenAlpha>().PlayReverse();
                yield return new WaitForSeconds(shield.GetComponentInChildren<TweenAlpha>().duration);
                shield.GetComponentInChildren<TweenAlpha>().enabled = false;
            }
        }
    }

    void Awake()
    {
        
    }

    void Start()
    {
        //GameClient.instance.tutorial = FindObjectOfType<TutorialLobby>();
        //GameClient.instance.tutoModelCon = FindObjectOfType<TutorialModelController>();
        MconLobby = LobbyMgr.Instance.tutoModelCon;

        TextDescParent = GameObject.Find("UI_Tutorial/Camera/Anchor_Tutorial/Panel");
        GameObject TutUI = GameObject.Find("UI_Tutorial");

     
        
        

        if (GameClient.instance.isVisitor == true)
        {


            CMachineList.GetInstance().SetBoxColl(false);

            if (GameClient.instance.isIngame == false)
            {
                tutDialog = TutorialDialog.A;
                shield.SetActive(true);
            }

            TutDialog = tutDialog;

        }
        else
        {
            if (GameClient.instance.isIngame == true)
            {
                tutDialog = TutorialDialog.J;
                CMachineList.GetInstance().SetBoxColl(false);
                TutDialog = tutDialog;
            }
            else
            {
                for (int i = 0; i < DeleteObj.Length; i++)
                {
                    DeleteObj[i].SetActive(false);
                }
            }

        }
        
    }

    

    

    


    public TutorialDialog tutDialog = TutorialDialog.A;
    public TutorialDialog TutDialog
    {
        get { return tutDialog; }
        set
        {

            switch (tutDialog)
            {
                case TutorialDialog.A:
                    

                    MconLobby.modelPos = TalkPos.Left;
                    leftTextDesc.SetActive(true);
                    leftTextDesc.GetComponentInChildren<UILabel>().text = "";
                    sTutmsgKor = "미스터리 블럭에 온 것을 환영합니다. \n\n간단한 튜토리얼을 진행하겠습니다.";
                    sTutmsgEng = "Welcome to MysteryBlock. \n\nThis tutorial shows you how to play game.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng));
                    }

                    break;
                case TutorialDialog.B:
                    //DummyUI_Panel.transform.GetChild(0).gameObject.SetActive(true);

                    sTutmsgKor = "현재 가지고 있는 코인입니다. \n\n머신 하단의 값만큼 코인이 소모됩니다.";
                    sTutmsgEng = "This is the coin you have. \n\nThe coins will be reduced as you play by written below.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "1"));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "1"));
                    }
                    
                    break;
                case TutorialDialog.C:
                    DummyUI_Panel.transform.GetChild(1).gameObject.SetActive(false);
                    //DummyUI_Panel.transform.GetChild(1).gameObject.SetActive(true);

                    sTutmsgKor = "게임진행을 통해 획득한 티켓입니다.\n\n게임을 완료하게되면 보상으로 일정한 티켓이 주어집니다.";
                    sTutmsgEng = "This is the ticket you earned from the game.\n\nIf you win the game, you will get tickets for reward.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "2"));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "2"));
                    }

                    break;
                case TutorialDialog.D:
                    DummyUI_Panel.transform.GetChild(2).gameObject.SetActive(false);
                    //DummyUI_Panel.transform.GetChild(2).gameObject.SetActive(true);

                    sTutmsgKor = "[007fff]'효과음'[-] 등 게임 환경의 확인 또는 변경할 수 있습니다.";
                    sTutmsgEng = "Click on the [007fff]'Setting'[-] \nbutton to change [007fff]'sound', 'vibration', 'etc'[-].";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "3"));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "3"));
                    }

                    break;
                case TutorialDialog.E:
                    DummyUI_Panel.transform.GetChild(3).gameObject.SetActive(false);
                    DummyUI_Panel.transform.GetChild(0).gameObject.SetActive(true);
                    sTutmsgKor = "난이도에 따라 게임을 선택할 수 있습니다. \n\n자! 이제 게임을 선택해 볼까요?";
                    sTutmsgEng = "You may choose game machine according to the level of difficulty.\n\nNow, let's choose game machine.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng));
                    }

                    
                    break;
                case TutorialDialog.F:
                    leftTextDesc.SetActive(false);
                    isClearMission = false;
                    Nextbtn.SetActive(false);
                    FingerTouch[0].SetActive(true);
                    StartCoroutine(EventButtonFlash(eventArrow));
                    break;
                case TutorialDialog.G:
                    leftTextDesc.SetActive(true);
                    sTutmsgKor = "게임 화면을 터치해 게임을 진행하겠습니다.";
                    sTutmsgEng = "Touch game machine to play game.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng));
                    }

                    break;
                case TutorialDialog.H:
                    leftTextDesc.SetActive(false);
                    isClearMission = false;
                    StartCoroutine(DimAlpha());
                    MconLobby.modelPos = TalkPos.None;
                    MconLobby.TalkLeft.SetActive(false);
                    FingerTouch[1].SetActive(true);
                    CMachineList.GetInstance().SetBoxColl(true);
                    //배경 텍스쳐 교체
                    break;
                case TutorialDialog.I:
                    FingerTouch[1].SetActive(false);
                    GameClient.mGameState = GameState.Mystery;
                    GameClient.instance.LoadingScene();
                    GameClient.instance.StopBGM();
                    break;
                case TutorialDialog.J:
#if UNITY_ANDROID
                    if (FingerTouch.Length > 2) FingerTouch[2].SetActive(true);
#endif

                    MconLobby.modelPos = TalkPos.Left;
                    leftTextDesc.SetActive(true);
                    leftTextDesc.GetComponentInChildren<UILabel>().text = "";
                    MconLobby.TalkLeft.SetActive(true);
                    sTutmsgKor = "터치하면 [007fff]'티켓스페이스'[-]로 이동되며 보상을 확인할 수 있습니다. \n\n또한 내가 모은 티켓으로 다양한 상품에 응모할 수도 있습니다.";
                    sTutmsgEng = "Click on the [007fff]'Ticket Space'[-] button to move to the [007fff]'Ticket Space'[-] platform and check reward.\n\nAlso, you may change your tickets to real prize.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        // idx 없이 호출: DummyUI_Panel.GetChild(4)가 없어서 에러나던 버그 수정
                        StartCoroutine(TextView(sTutmsgKor));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng));
                    }
                    break;
                case TutorialDialog.K:
                    GameClient.instance.isIngame = false;
                    CMachineList.GetInstance().SetBoxColl(true);
                    for (int i = 0; i < DeleteObj.Length; i++)
                    {
                        DeleteObj[i].SetActive(false);
                    }
                    if (GameClient.mNetwork == true)
                    {
#if UNITY_ANDROID
                        DummyUI_Panel.transform.GetChild(3).gameObject.SetActive(false);
                        MessageBox.Instance.OpenMessageBox(emMsgType.TUTO_MOVEPLATFORM);
#elif UNITY_IOS
                        CNetwork.GetInstance().CS_Tutorial();
#endif
                    }
                    else
                    {
                        // Offline: tutorial complete, hide tutorial object
                        gameObject.SetActive(false);
                    }
                    break;
            
            }
        }
    }

    public IEnumerator TextView(string str, string idx = null)
    {
        string lbText = str;
        Nextbtn.SetActive(false);
        GameObject obj = null;

        if (idx != null)
        {
            int index = Convert.ToInt32(idx);
            obj = DummyUI_Panel.transform.GetChild(index).gameObject;
            obj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        if (GameClient.mNetwork == true && GameClient.instance.isVisitor == false && GameClient.instance.isIngame == false)
        {
            yield break;
        }


        switch (MconLobby.modelPos)
        {
            case TalkPos.Left:
                leftTextDesc.GetComponentInChildren<UILabel>().text = lbText;
                break;
            case TalkPos.Right:
                rightTextDesc.GetComponentInChildren<UILabel>().text = lbText;
                break;
        }
        yield return StartCoroutine(MconLobby.PlayAnimCombination());
        Nextbtn.SetActive(true);
        yield return StartCoroutine(NextText());
    }

    IEnumerator Wait()
    {
        Debug.Log("WaitCoroutine");

        while (isClearMission == false)
        {
            yield return null;
            if (isClearMission == true)
            {
                tutDialog++;
                TutDialog = tutDialog;
                Debug.Log("CompleteCoroutine");
            }
        }
    }

    IEnumerator NextText()
    {
        
        Debug.Log(tutDialog.ToString() + " / " + isClearMission);
        isTouch = false;

        while (isTouch == false)
        {
            yield return null;
            if (Input.GetMouseButtonUp(0))
            {
                tutDialog++;
                TutDialog = tutDialog;
                isTouch = true;
            }
        }
    }

}
