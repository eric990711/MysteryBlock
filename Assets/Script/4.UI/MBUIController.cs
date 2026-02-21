using UnityEngine;
using System.Collections;

public class MBUIController : MonoBehaviour {



    public static MBUIController Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<MBUIController>();

            }
            return mInstance;
        }
    }

    public UILabel mMultiplier;

    private static MBUIController mInstance;

    public GameObject CoinPanel ;
    
    public GameObject[] NormalUI;
		
	public GameObject[] DropObj;

    public GameObject mScoreText;
    public UILabel mScoreLabel;
    
    float m_UpdateScoreTime = 0.0f;


    public bool startLock = false;
    public bool isStartGame = false;

    public GameObject mDoubbleButton;

    public int multiplier = 0;

	// Use this for initialization
	void Start () {

        Init();
    }

    //2014.04.15 더블 씨리즈 표현

    


    public void RefreshMultiplier()
    {
        multiplier = MysteryMgr.Instance.mDoubleJackpot * MysteryMgr.Instance.mDoubleTicket * MysteryMgr.Instance.mDoubleToggle;
        int tempWin = GameClient.instance.GetMachineJackPot() + GameClient.instance.GetMachineData().theJackpotSum;

        

        if (multiplier == 1)
        {
            if (GameClient.instance.mMysteryState != MysteryState.InGame)
            {
                mScoreLabel.text = (tempWin).ToString();
            }
            
            mMultiplier.gameObject.SetActive(false);
        }
        else
        {
            if (MysteryMgr.Instance.mDoubleJackpot == 2)
            {
                if (MysteryMgr.Instance.mBlockStep < 3)
                {
                    if (MysteryMgr.Instance.mDoubleToggle == 2)
                    {
                        multiplier = 2;
                        mMultiplier.text = "x" + multiplier.ToString();
                        //mScoreLabel.text = (LevelScore).ToString();
                        mMultiplier.gameObject.SetActive(true);
                    }
                    else
                    {
                        multiplier = 1;
                        //mScoreLabel.text = (LevelScore).ToString();
                        mMultiplier.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (MysteryMgr.Instance.mDoubleToggle == 2)
                    {
                        multiplier = 4;
                        mMultiplier.text = "x" + multiplier.ToString();
                        //mScoreLabel.text = (LevelScore).ToString();
                        mMultiplier.gameObject.SetActive(true);
                    }
                    else
                    {
                        multiplier = 2;
                        mMultiplier.gameObject.SetActive(true);
                        //mScoreLabel.text = (LevelScore).ToString();
                    }
                }
            }
            else if (MysteryMgr.Instance.mDoubleTicket == 2)
            {
                if (MysteryMgr.Instance.mDoubleToggle == 2)
                {
                    multiplier = 4;
                    
                    mMultiplier.gameObject.SetActive(true);
                    mMultiplier.text = "x" + multiplier.ToString();
                    //mScoreLabel.text = (LevelScore).ToString();
                }
                else
                {
                    multiplier = 2;
                    mMultiplier.gameObject.SetActive(true);
                    mMultiplier.text = "x" + multiplier.ToString();
                    //mScoreLabel.text = (LevelScore).ToString();
                }
            }
            else
            {
                if (GameClient.instance.mMysteryState != MysteryState.InGame)
                {
                    if (MysteryMgr.Instance.mDoubleToggle == 2)
                    {
                        multiplier = 2;
                        mMultiplier.text = "x" + multiplier.ToString();
                        mMultiplier.gameObject.SetActive(true);
                        mScoreLabel.text = (tempWin * multiplier).ToString();
                    }
                    else
                    {
                        multiplier = 1;
                        mMultiplier.gameObject.SetActive(false);
                        mScoreLabel.text = (tempWin * multiplier).ToString();
                    }
                }
            }
            Debug.Log("multiplier" + multiplier);
            //SetUpdateScore();
            //StartCoroutine(UpdateScore());
        }
    }

    public void Init()
    {

        foreach (GameObject obj in DropObj)
        {
            obj.SetActive(false);
        }
        //MysteryMgr.Instance.mUseCoinx2 = false;
        //MysteryMgr.Instance.mDoubleToggle = 1;
        MysteryMgr.Instance.mDoubleTicket = 1;
        MysteryMgr.Instance.mDoubleJackpot = 1;
        //multiplier = 1;
        RefreshMultiplier();

        FrontUIStartButton(true);

        CoinPanelShotDown(false);

        CoinTextUpdate();

        //mScoreText.GetComponent<UILabel>().text =(GameClient.instance.GetMachineJackPot()+GameClient.instance.GetMachineData().theJackpotSum).ToString(); 
        
        Debug.Log("Init() mUseCoinx2 " + MysteryMgr.Instance.mUseCoinx2);
        
        //mDoubbleButton.transform.Find("NormalButton").gameObject.SetActive(true);
        //mDoubbleButton.transform.Find("PressButton").gameObject.SetActive(false);
        
        //mMultiplier.gameObject.SetActive(false);
        
        //MysteryMgr.Instance.EffectTicketPaper(false);
        
        MysteryMgr.Instance.EffectTextTicket();

        
    }

    


    public void GameStart()
    {
        if (startLock == true)
        {
            return;
        }

        //LoadingBar.GetInstance().SetLock(false);
        if (isStartGame == false)
        {
            LoadingBar.GetInstance().SetLock(false);
            GameClient.instance.mMysteryState = MysteryState.InGame;

            if (MysteryMgr.Instance.em_playtype == PLAYTYPE._BONUS) { MysteryMgr.Instance.em_playtype = PLAYTYPE._DEFAULT; }
            else if (MysteryMgr.Instance.em_playtype == PLAYTYPE._DEFAULT) { MysteryMgr.Instance.mCreditCoin--; }
            else if (MysteryMgr.Instance.em_playtype == PLAYTYPE._DOUBLE) { MysteryMgr.Instance.mCreditCoin -= 2; }

            //GameClient.instance.StopBGM();
            //GameClient.instance.PlayBGM();

            MysteryMgr.Instance.mCameraObj.GetComponent<MysteryCamera>().Init();

            GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_ReadGo);

            StartCoroutine(UpdateScore());

            FrontUIStartButton(false);

            CoinPanelShotDown(true);


            //MysteryMgr.Instance.EffectTicketPaper(false);

            MysteryMgr.Instance.EffectTextTicket();

            isStartGame = true;
        }
    }

    _BlockType type;




    public int tempLevelScore = 0;
    int NextScore = 0;
    int LevelScore = 0;

//     void SetUpdateScore()
//     {
//         if (MysteryMgr.Instance.mScore > 0)
//         {
//             GameObject obj = Instantiate(GameClient.instance.mEffMoveTicket) as GameObject;
//             obj.transform.parent = mScoreText.transform;
//             obj.transform.localPosition = new Vector3(0, 0, -26);
// 
//             MysteryMgr.Instance.EffectTicketPaper(true);
// 
//         }
//         int LevelScore = 0;
//         float JackPotCost = GameClient.instance.GetMachineJackPot();
//         //2014.04.15 더블 씨리즈 표현
// 
//         if (MysteryMgr.Instance.mDoubleTicket > 1)
//         {
//             MysteryMgr.Instance.mDoubleTicket = 1;
//         }
//        
//         switch (MysteryMgr.Instance.mBlockStep)
//         {
//             case 0: LevelScore = Mathf.RoundToInt((JackPotCost * 0.01f) * multiplier); tempLevelScore = LevelScore; break;
//             case 1: LevelScore = Mathf.RoundToInt((JackPotCost * 0.03f) * multiplier); tempLevelScore = LevelScore; break;
//             case 2: LevelScore = Mathf.RoundToInt((JackPotCost * 0.1f) * multiplier); tempLevelScore = LevelScore; break;
//             case 3:
//                 LevelScore = Mathf.RoundToInt(JackPotCost) * multiplier;
//                 tempLevelScore = LevelScore;
//                 //MysteryMgr.Instance.SumScore += LevelScore;
//                 break;
//         }
// 
//         Debug.Log("LevelScore : " + LevelScore);
//         
//     }

    public IEnumerator UpdateScore()
    {
  
        bool bLoop = true;
        //int NextScore = MysteryMgr.Instance.mScore;
        MysteryMgr.Instance.mRealScore = MysteryMgr.Instance.mScore;

        if (MysteryMgr.Instance.mScore > 0)
        {
            GameObject obj = Instantiate(GameClient.instance.mEffMoveTicket) as GameObject;
            obj.transform.parent = mScoreText.transform;
            obj.transform.localPosition = new Vector3(0, 0, -26);

            //MysteryMgr.Instance.EffectTicketPaper(true);

        }

        int LevelScore = 0;
        float JackPotCost = GameClient.instance.GetMachineData().theJackPot + GameClient.instance.GetMachineData().theJackpotSum;
        //2014.04.15 더블 씨리즈 표현
        RefreshMultiplier();
        if (MysteryMgr.Instance.mDoubleTicket > 1)
        {
            MysteryMgr.Instance.mDoubleTicket = 1;
        }
        //RefreshMultiplier();
        switch (MysteryMgr.Instance.mBlockStep)
        {
            case 0: LevelScore = Mathf.RoundToInt((ConstValue.GetLimitJackpot() * 0.01f) * multiplier); tempLevelScore = LevelScore; break;
            case 1: LevelScore = Mathf.RoundToInt((ConstValue.GetLimitJackpot() * 0.03f) * multiplier); tempLevelScore = LevelScore; break;
            case 2: LevelScore = Mathf.RoundToInt((ConstValue.GetLimitJackpot() * 0.1f) * multiplier); tempLevelScore = LevelScore; break;
            case 3:
                LevelScore = Mathf.RoundToInt(JackPotCost) * multiplier;
                tempLevelScore = LevelScore;
                break;
            case 4:
                MysteryMgr.Instance.SumScore += LevelScore;
                break;
        }
        //RefreshMultiplier();
        Debug.Log("LevelScore ::::::: " + LevelScore);
        //RefreshMultiplier();
        //NextScore = LevelScore;

        /*
        if (MysteryMgr.Instance.mUseCoinx2 == true)
        {
            NextScore = LevelScore * 2;
        }*/
        
        
        //2014.04.15 더블 씨리즈 표현
        //NextScore = LevelScore;
        Debug.Log("Level Score UpdateScore" + LevelScore + "/" + NextScore);
        
        float to = LevelScore;
        float from = (float)NextScore;
        NextScore = LevelScore;
        int  score = 0; 
        float t = 0f;
        m_UpdateScoreTime = 0.0f; 

        while(bLoop)
        {
            m_UpdateScoreTime += Time.deltaTime; 
            t = m_UpdateScoreTime / 1f; 
            score = Mathf.RoundToInt( Mathf.Lerp(from , to , t));

            mScoreLabel.text = score.ToString();
         
            if (Mathf.RoundToInt(score) == NextScore)
            {
                bLoop = false;
                //화면 상단에 티켓으로 적용되는곳 현재스코어 적용 
                MysteryMgr.Instance.mScore = NextScore; 
            }
            yield return null; 
        }

        //MysteryMgr.Instance.EffectTicketPaper(false);
    }
    
    //코인 내림 올림
    public void CoinPanelShotDown(bool flag)
    {

        if (CoinPanel == null) return; 

        Animation ani = CoinPanel.GetComponent<Animation>();
        

        if (flag == false)
        {
            ani.Play("CoinPanelUp");
        }
        else
        {
            ani.Play("CoinPanelDown");
            startLock = true;
        }

        if (ani.IsPlaying("CoinPanelUp") == false)
        {
            startLock = false;
        }
        
    }
	
	public void ShowDropImg(int index)
	{
		
		//Debug.Log("ShowDrop  " + index);
		if( index > 2 ) return ; 
		
		DropObj[index].SetActive(true);
		TweenScale[] tempTS = DropObj[index].GetComponentsInChildren<TweenScale>();

        for (int i = 0; i < tempTS.Length; i++)
        {
            tempTS[i].enabled = true;
            tempTS[i].Reset();
        }
	}


    public void FrontUIStartButton(bool bShow)
    {
        foreach (GameObject obj in NormalUI)
        {
            obj.SetActive(bShow);
        }
    }
	
    public void CoinTextUpdate()
    {
        if (CoinPanel != null)
        {
            UILabel label = CoinPanel.transform.Find("CoinText").GetComponent<UILabel>();
            label.text = MysteryMgr.Instance.mCreditCoin.ToString();
        }
    }

}
