using UnityEngine;
using System;
using System.Collections;

public enum IngameDlg
{
    A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P
}

public enum BlockGame
{
    B01, B02, B03, B04, B05, B06, B07, B08, B09, B10, B11
}

public class TutorialInGame : MonoBehaviour {

    public GameObject TutoA = null;
    public GameObject TutoB = null;

    public GameObject leftTextDesc = null;
    public GameObject rightTextDesc = null;
    public GameObject blackCover = null;
    public GameObject nextbtn = null;


    public TutorialModelController MconIngame;

    public GameObject[] DeleteObj = null;


    public BGBlockMgr bgBlockMgr;
    public GameObject [] Finger;

    string sTutmsgKor = null;
    string sTutmsgEng = null;
    
    

    public bool bDropComplete = false;
    public bool bNextBlockProc = true;

    public bool isClearMission = true;

    bool isTouch = false;
    bool bNextDlg = false;


	// Use this for initialization
	void Start () 
    {
        //GameClient.instance.gameTuto = FindObjectOfType<TutorialInGame>();
        //GameClient.instance.tutoModelCon = FindObjectOfType<TutorialModelController>();
        GameClient.instance.isIngame = false;
        //GameClient.instance.isVisitor = true;

        Debug.Log(GameClient.instance.isVisitor);

        bgBlockMgr = FindObjectOfType<BGBlockMgr>();
        MconIngame = MysteryMgr.Instance.tutoModelCon;

        if (GameClient.instance.isVisitor == true)
        {
            dlg = IngameDlg.A;
            Ingamedialog = dlg;
        }
        else
        {
            for (int i = 0; i < DeleteObj.Length; i++)
            {
                DeleteObj[i].SetActive(false);
            }
        }
        
        
	}

    public IEnumerator TextView(string str, string idx = null, GameObject parentIdx = null)
    {
        string lbText = str;
        nextbtn.SetActive(false);
        GameObject obj = null;

        if (idx != null)
        {
            int index = Convert.ToInt32(idx);
            obj = parentIdx.transform.GetChild(index).gameObject;
            obj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        if (GameClient.instance.isVisitor == false)
        {
            yield break;
        }


        switch (MconIngame.modelPos)
        {
            case TalkPos.Left:
                leftTextDesc.GetComponentInChildren<UILabel>().text = lbText;
                break;
            case TalkPos.Right:
                rightTextDesc.GetComponentInChildren<UILabel>().text = lbText;
                break;
        }
        yield return StartCoroutine(MconIngame.PlayAnimCombination());
        if (dlg != IngameDlg.E)
        {
            nextbtn.SetActive(true);
        }
        
        yield return StartCoroutine(NextText());
    }

    IEnumerator Wait()
    {
        while (isClearMission == false)
        {
            yield return null;
            if (isClearMission == true)
            {
                Debug.Log("Wait Off");
            }
        }
        yield return null;
    }




    IEnumerator NextText()
    {
        
        yield return StartCoroutine(Wait());

        if (isClearMission == true)
        {
            isTouch = false;
            while (isTouch == false)
            {

                yield return null;

                if (Input.GetMouseButtonUp(0))
                {
                    dlg++;
                    Ingamedialog = dlg;
                    isTouch = true;
                }
            }
        }

        
    }

    public IEnumerator AutoDropBlock()
    {
        while (bNextBlockProc == false)
        {
            yield return null;
            if (bNextBlockProc == true)
            {
                //RotateBlock();
                
                Invoke("BlockDesc", 0.5f);
            }
        }
    }


    public IngameDlg dlg = IngameDlg.A;
    public IngameDlg Ingamedialog
    {

        get { return dlg; }
        set
        {
            switch (dlg)
            {
                case IngameDlg.A:

                    MconIngame.modelPos = TalkPos.Right;
                    rightTextDesc.SetActive(true);
                    rightTextDesc.GetComponentInChildren<UILabel>().text = "";
                    MconIngame.TalkRight.SetActive(true);

                    sTutmsgKor = "[007fff]'뒤로가기'[-] 버튼을 터치하게 되면 로비로 이동하게 됩니다.";
                    sTutmsgEng = "Click on the [007fff]'Back'[-] button to go back to lobby.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "0", TutoA));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "0", TutoA));
                    }
                    //TutoA.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case IngameDlg.B:
                    TutoA.transform.GetChild(0).gameObject.SetActive(false);

                    sTutmsgKor = "게임 진행에 필요한 코인을 투입합니다.";
                    sTutmsgEng = "Insert a coin into the \n[007fff]'To play'[-]'";

                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "0", TutoB));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "0", TutoB));
                    }


                    
                    //TutoB.transform.GetChild(0).gameObject.SetActive(true);

                    
                    break;
                case IngameDlg.C:

                    TutoB.transform.GetChild(0).gameObject.SetActive(false);

                    sTutmsgKor = "[007fff]'X2'[-] 버튼으로 난이도가 \n상승하고 코인이 2배 소모되나 보상이 커집니다.";
                    sTutmsgEng = "Click on the [007fff]'X2'[-] button to increase level of difficulty and double the reward.";

                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "1", TutoB));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "1", TutoB));
                    }
                    
                    //TutoB.transform.GetChild(1).gameObject.SetActive(true);


                    break;
                case IngameDlg.D:
                    MconIngame.modelPos = TalkPos.None;
                    TutoB.transform.GetChild(1).gameObject.SetActive(false);
                    MconIngame.TalkRight.SetActive(false);
                    MconIngame.TalkLeft.SetActive(true);
                    leftTextDesc.SetActive(true);
                    rightTextDesc.SetActive(false);

                    MconIngame.modelPos = TalkPos.Left;

                    sTutmsgKor = "10개의 블럭을 쌓으면 WIN을 달성할 수 있습니다. \n\n게임중 [007fff]'미스터리'[-] 블럭을 쌓아 다양한 효과를 얻으세요!";
                    sTutmsgEng = "Stack up the 10blocks and Win. \n\nAlso, you may get surprise item if tou get [007fff]'Mystery'[-] block.";

                    
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng));
                    }




                    break;
                case IngameDlg.E:

                    //TutoB.transform.GetChild(2).gameObject.SetActive(true);
                    isClearMission = false;
                    nextbtn.SetActive(false);

                    sTutmsgKor = "자! [007fff]'시작'[-] 버튼을 터치 하세요.";
                    sTutmsgEng = "Now let's click on the [007fff]'Start'[-] button and play the game.";

                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor, "2", TutoB));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng, "2", TutoB));
                    }
                    
                    Finger[0].SetActive(true);


                    break;
                case IngameDlg.F:
                    blackCover.GetComponent<TweenAlpha>().enabled = true;
                    MBUIController.Instance.GameStart();
                    isClearMission = false;
                    MconIngame.modelPos = TalkPos.None;
                    leftTextDesc.SetActive(false);
                    Finger[0].SetActive(false);
                    MconIngame.TalkLeft.SetActive(false);
                    TutoB.transform.GetChild(2).gameObject.SetActive(false);
                    break;

                case IngameDlg.G:
                    MconIngame.modelPos = TalkPos.Right;
                    MconIngame.TalkRight.SetActive(true);
                    rightTextDesc.SetActive(true);
                    nextbtn.SetActive(true);
                    blackCover.GetComponent<TweenAlpha>().PlayReverse();
                    sTutmsgKor = "축하합니다. \n\n이제 로비로 이동해 보상을 확인해 주세요.";
                    sTutmsgEng = "Congratulation!\n\nNow go back to lobby and check reward.";
                    if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
                    {
                        StartCoroutine(TextView(sTutmsgKor));
                    }
                    else
                    {
                        StartCoroutine(TextView(sTutmsgEng));
                    }
                    break;
                case IngameDlg.H:
                    if (GameClient.instance.isVisitor == true)
                    {
                        GameClient.instance.isIngame = true;
                    }
                    else
                    {
                        GameClient.instance.isIngame = false;
                    }
                    
                    GameClient.instance.isVisitor = false;
                    GameClient.instance.SaveTutResult();
                    MconIngame.TalkRight.SetActive(false);
                    rightTextDesc.SetActive(false);
                    GameClient.mGameState = GameState.Lobby;
                    //gameObject.SetActive(false);
                    GameClient.instance.LoadingScene();
                    break;
            }
        }
    }

}