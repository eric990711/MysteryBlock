using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ConstValue
{
    public static int GetLimitJackpot()
    {
        int criterion = 1000;

        if (GameClient.instance.GetMachineJackPot() > criterion)
        {
            return criterion;
        }
        else
        {
            return GameClient.instance.GetMachineData().theJackPot;
        }
    }
}


public class MysteryMgr : MonoBehaviour
{
	
	public static MysteryMgr Instance 
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MysteryMgr>();
				
            }
		    return m_instance;
        }
    }

    public UI_GameResultView c_resultView;
    public UI_Result mUIResultView;

    public TutorialInGame tutoIngame;
    public TutorialModelController tutoModelCon;

    public const int blockCnt = 10;
    public int ArrBlockNum = 0;
	public bool isTestModeSetting = false; 

    public bool[] BlockArrangement;

    

    //20140818 ljw 무료게임
    public PLAYTYPE em_playtype = PLAYTYPE._DEFAULT;

    public int CompareTicketValue()
    {
        return 0;
    }

    //게임내 쓰는것
    [HideInInspector]
    public int mRealScore = 0;  //현재 스텝 성공 티켓수
    //[HideInInspector]
    //public int mInsertCoin = 0; //게임 시작전에 들어가는코인 갯수
    [HideInInspector]
    public int mCreditCoin = 0; //

    [HideInInspector]
    public int mLife = 0;      //플레이어 생명수
 
    public int mDoubleJackpot = 1;  //더블 잭팟 
    
    public int mDoubleTicket = 1;   //더블 티켓

    public int mDoubleToggle = 1;

    public long mTotalTicket = 0;

    public long mJackpotTicket = 50; //잭팟성공시 점수  

    public bool mUseCoinx2 = false; //코인 x2 

    public long mPrevUserTicket = 0;


    public UILabel NeedCoinlabel = null; 
    

	public BaseBlock mSlowBlcok = null;

    //점수
    //public int mBlockCount { get { return mBlockList.Count; } }
    public int mBlockStep = 0;  //박스가 윈존에 충돌하며 스텝을 한계씩 상승
    public int mBlockCnt = 0;

    public int mScore = 0;
    public bool isMysteryEff = false;

    public UILabel TicketValueText; 

    //아이템 관련변수
    public bool[] mGetItem = new bool[3] { false, false, false }; //SpeedDown,   DoubleTicket,     DoubleJackpot,  
    public bool mDropItem = false;  //한번만 나오게 설정
    public bool mItemUse = false; 

    //이펙트 이미지
    public GameObject mEffectDJacpot = null;
    public GameObject mEffectDTicket = null;
    public GameObject mEffectSpeedDown = null;

    public GameObject mTicketTextObj = null; 

    public GameObject blockObj;

    public bool mMissionSuccess = false; 


    public int mBlockStack;
    public ArrayList mBlockList = new ArrayList();
    public ArrayList mBackGroundData = new ArrayList();
    

	//관련 UI 
    public GameObject _InstantObj = null;
    GameObject mBG_Group = null;
    GameObject mUIMystery =null;
    
    
    GameObject mUISliderBar = null;
    public GameObject mUITopBar = null;

    GameObject mUIIngamePanel = null;
    GameObject mUIResultPanel = null;
    
    public UILabel     mUIJackPotLabel = null;

    public UILabel     mUIUserMoneyLabel = null;


    GameObject mUITicketPaper = null; 


    GameObject mPlayerBar = null; 


    public GameObject mTicketMachine = null;
    public GameObject mCameraObj = null; 


    GameObject mEff_randomBox;  //게임에서 승리하면 나오는 랜덤박스 이펙트;
    
	float fNowTime = 0;

    public Camera mCamera;
    public Camera mUICamera = null; 
        
	private static MysteryMgr m_instance = null;

    public bool IsLock = false;
    public int SumScore = 0;
    public int AcumulSumScore = 0;

    public GameObject webview;
    public GameObject webPopupView; //sdh 20140704 팝업작업 
    


    void Awake()
    {
        mUIMystery = GameObject.Find("UI_Mystery");
        _InstantObj = GameObject.Find("_InstantObject");
        mBG_Group = transform.Find("BG_Group").gameObject;

        mPlayerBar = transform.Find("PlayerBar").gameObject;


        mUISliderBar = mUIMystery.transform.Find("Camera/Anchor/Panel/LeftSliderBar").gameObject;
        mUITopBar = mUIMystery.transform.Find("Camera/Anchor/Panel/TopBar").gameObject;

        mUIIngamePanel = mUIMystery.transform.Find("Camera/Anchor/Panel").gameObject;
        //mUIResultPanel = GameObject.Find("UI_Result");

        mUITicketPaper = mUIMystery.transform.Find("Camera/Anchor/Panel/TicketPaper").gameObject;

        if (mBG_Group == null) Debug.Log("mBG_Group error");

        mCameraObj = transform.Find("MCamera").gameObject;
       

        fNowTime = 0;
        mBlockStack = 0;
        mScore = 0;
        mRealScore = 0;
		mPrevUserTicket  =0 ; 
		GameClient.instance.mUserTicket = 0 ; 

    }
    


	// Use this for initialization
	void InitUserInfo()
	{
		mPrevUserTicket = GameClient.instance.mUserTicket;
        mUIUserMoneyLabel.text = GameClient.instance.mUserCoin.ToString();
	}


	void Start () {

		//InitMysteryGame();
		GameClient.mGameState = GameState.Mystery;

        
        
        //StartCoroutine(GAManager.Instance.LevelSession("/Game"));
        //GameClient.instance.StopBGM();
        GameClient.instance.PlayBGM();

        //연동 로비나 인게임에서 다시 머신정보를 얻어온다.  		

        UILabel label = mTicketTextObj.GetComponent<UILabel>();
        label.text = AcumulSumScore.ToString();

        if (GameClient.mNetwork == true)
        {
            LoadingBar.GetInstance().SetLock(true);
                //2014.04.15 이재우 연동부분 수정
                //CNetwork.GetInstance().CS_EnterLobby(InitMysteryGame);
            
            //연동 유져정보를 갖고옴
            CNetwork.GetInstance().CS_UserData();
            
            CNetwork.GetInstance().CS_EnterLobby(InitUserData);
			
		
        }
        else
        {
			InitUserData();
        }
        
	}

    void MBlockArrange()
    {
        if (GameClient.instance.isVisitor == false)
        {
            BlockArrangement = new bool[10] { false, false, false, false, false, false, false, true, false, false };

            for (int i = 1; i < blockCnt - 2; i++)
            {
                bool temp = BlockArrangement[i];
                int rnd = Random.Range(1, blockCnt - 2);
                BlockArrangement[i] = BlockArrangement[rnd];
                BlockArrangement[rnd] = temp;
            }
        }
        else
        {
            BlockArrangement = new bool[10] { false, true, false, false, false, false, false, false, false, false };
        }

    }
    
	void InitUserData()
	{
        GameClient.instance.mUserTicket = 0;
        tutoIngame.gameObject.SetActive(true);
		InitMysteryGame();
        
	}

	// Update is called once per frame
    void Update()
    {

       

        if (Input.GetKeyUp(KeyCode.Z))
        {
            StartCoroutine(EffectSuccessBlock());
        }

        if (CPopupMgr.instance.bOnWebview == true) return;
        
		//배경을 랜덤으로 선택
        if (GameClient.instance.mMysteryState == MysteryState.InGame)
        {
			SelectBlock();
        }
        // 게임을 다시시작한다. 
        //아이템 관련 처리 



        if (mEffectDJacpot.activeSelf)
        {
            if (mEffectDJacpot.GetComponent<Animation>().isPlaying == false)
                mEffectDJacpot.SetActive(false);
        
        }
        else if (mEffectDTicket.activeSelf) 
        {
            if (mEffectDTicket.GetComponent<Animation>().isPlaying == false)
                mEffectDTicket.SetActive(false);

        }
        else if (mEffectSpeedDown.activeSelf)
        {
            if (mEffectSpeedDown.GetComponent<Animation>().isPlaying == false)
                mEffectSpeedDown.SetActive(false);
        }
	}

    public void UpdateInfo()
    {
        NeedCoinlabel.text = GameClient.instance.GetMachineData().theNeedCoin.ToString();
    }

    //저장된 블럭을 초기화시킨다. 
    public void InitMysteryGame()
    {
        Debug.Log("InitMysteryGame");
        LoadingBar.GetInstance().SetLock(false);

		GameClient.instance.mMysteryState = MysteryState.Stay;
        MBUIController.Instance.isStartGame = false;
        
        

        //Destroy(BlockObject.gameObject);
        MBlockArrange();
        GameClient.instance.PlayBGM();
        ArrBlockNum = 0;
        mBlockCnt = 0;
		mDoubleTicket = 1;
        mDoubleJackpot = 1;
        AcumulSumScore += SumScore;
		GameClient.instance.mUserTicket = AcumulSumScore;

        mTicketTextObj.GetComponent<UILabel>().text = GameClient.instance.mUserTicket.ToString();

		Debug.Log("sumscore : " + SumScore + " / Acumul : " + AcumulSumScore);
        SumScore = 0;
        mLife = 0;
        mBlockStep = 0;
        mScore = 0;
        mRealScore = 0;
        mDropItem = false;
        mItemUse = false;
        //mUseCoinx2 = false;


        mTotalTicket = 0;
        isMysteryEff = false;
        
		mSlowBlcok = null; 

        mPlayerBar.SetActive(true);

        mBG_Group.SetActive(true);

        FrontUIShow(true);
		
        
        
        mBlockList.Clear();
        
        transform.Find("PlayerBar").GetComponent<PlayerControll>().Init();
        
        for(int i = 0 ; i < mGetItem.Length ; i ++)
            mGetItem[i] = false; 

        //GameObject _InstantObject = GameObject.Find("_InstantObject");
        Transform[] data = _InstantObj.GetComponentsInChildren<Transform>();

        foreach (Transform obj in data)
        {
            if (obj.gameObject.Equals(_InstantObj)) continue;
            Destroy(obj.gameObject);
        }


        //배경 블럭을 모두 초기화 시킨다.
        foreach (GameObject obj in mBackGroundData)
            obj.SetActive(true);

        
        
        mBackGroundData.Clear();

        //카메라를 초기위치로 이동시킨다. 
        mCamera.GetComponent<MysteryCamera>().SetReverseCamera();
        mMissionSuccess = false;
        mUIJackPotLabel.text = GameClient.instance.GetMachineJackPot().ToString();


        
        UpdateInfo();
        InitUserInfo();

    }
    
    //랜덤으로 블럭선택
    public void SelectBlock()
    {
        float Width = Screen.width;
        float Height = Screen.height;
        if (mMissionSuccess == true || mBlockStep == 4)
            return;

        if (mSlowBlcok != null && mSlowBlcok.bCheckStayBlock == false)
            return;

        if (isMysteryEff == true)
        {
            return;
        }

        //Vector3 RandPos = new Vector3(Random.Range(0, Width-100), Random.Range( Height-Height/3, Height-Height/4.5f  ), 0);

        //Vector3 RandPos = new Vector3(224, Height-Height/3, Height-Height/4.5f  ), 0);

        //


        float randomRnd = Random.Range(-1f, 1f);
        float posx = 0;
        if (isTestModeSetting == false)
        {
            //posx = Width * (0.5f + randomRnd * (ArrBlockNum / 33.3f));
            if (ArrBlockNum == 1) posx = Width * 0.5f;
            else posx = Width * Random.Range(0.25f, 0.75f);
        }
        else
        {
            posx = Width * (0.5f);
        }

        if (GameClient.instance.isVisitor == true)
        {
            posx = Width * 0.5f;
        }

        Vector3 RandPos = new Vector3(posx, Height - Height / 3.75f, 0);
        Ray ray = mCamera.ScreenPointToRay(RandPos);
        //     Debug.Log ( Input.mousePosition);

        RaycastHit hit;

        fNowTime += Time.deltaTime;



        if (fNowTime > 3f)
        {
            if (Physics.Raycast(ray, out hit, 3000))
            {
                fNowTime = 0;
                Debug.DrawLine(ray.origin, hit.point, Color.blue);


                //Vector3 pos = new Vector3(hit.point.x , hit.point.y , 10.2156f );
                //Vector3 pos = new Vector3(hit.point.x , hit.point.y , hit.point.z+1.0f );
                Vector3 pos = hit.collider.gameObject.transform.position;
                //pos.y = hit.collider.transform.parent.position.y; 
                GameObject tmpobj;
                if (GameClient.instance.isVisitor == false)
                {
                    tmpobj = Instantiate(GameClient.instance.mBlock, pos, Quaternion.identity) as GameObject;
                }
                else
                {
                    tmpobj = Instantiate(GameClient.instance.mBlock, TutoBlockPos(), Quaternion.identity) as GameObject;
                }
                

                ArrBlockNum++;
                mBlockCnt++;
                if (ArrBlockNum == 10) ArrBlockNum = 0;
                tmpobj.transform.parent = _InstantObj.transform;
                tmpobj.GetComponent<BaseBlock>().BGBlock = hit.collider.gameObject.transform;

                hit.collider.gameObject.SetActive(false);

                mBackGroundData.Add(hit.collider.gameObject);

            }
        }

    }

    Vector3 TutoBlockPos()
    {

        Vector3 vTutoBlock = Vector3.zero;

        switch (mBlockStep)
        {
            case 0: vTutoBlock = new Vector3(0, 8, 10.2f);
                break;
            case 1: vTutoBlock = new Vector3(0, 11.1f, 10.2f);
                break;
            case 2: vTutoBlock = new Vector3(0, 14, 10.2f);
                break;
            case 3: vTutoBlock = new Vector3(0, 16.45f, 10.2f);
                break;
        }
        return vTutoBlock;
    }



	public void DropBlockProc()
	{
				
        
        if (GameClient.instance.mMysteryState != MysteryState.InGame)
            return;

        if (GameClient.instance.isVisitor == true) return;

        MBUIController.Instance.ShowDropImg(mLife);
        StartCoroutine(GameClient.instance.VibrateController(1, 1));
        mLife++;
        GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_FailDrop);

        if (isMysteryEff == true)
        {
            isMysteryEff = false;
        }


        if (mLife == Global.MAXLIFE)
        {
            
            //MissionFail();
            long PrevTicket = MysteryMgr.Instance.mPrevUserTicket;
            long UserTicket = GameClient.instance.mUserTicket;
            long cur = GameClient.instance.mUserTicket - PrevTicket;
            
            
            GameClient.instance.GetMachineData().theJackpotSum += Mathf.RoundToInt(0.02f * GameClient.instance.GetMachineData().theJackPot);
            Debug.Log("user : " + GameClient.instance.mUserTicket + " / sumJackpot : " + GameClient.instance.GetMachineData().theJackpotSum);
			MissionFail();
        }
	}

    void MissionFail()
    {
        Debug.Log("MissionFail ");
        
        GameClient.instance.StopBGM();
        StartCoroutine(GameClient.instance.VibrateController(1, 1));
        GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_GameOver);

        // 게임 오버 후 2초 뒤 광고 표시
        Invoke("ShowAdThenInit", 2f);

        // sdh 20140704 팝업
        //CPopupMgr.instance.OpenWebPopupView(MysteryMgr.Instance.webPopupView, false);
        //StartCoroutine(EffTicketText());
        
    }

    void ShowAdThenInit()
    {
        // Unity Ads 인터스티셜 광고 표시 후 MissionFailInit 실행
        if (UnityAdsManager.Instance != null)
        {
            UnityAdsManager.Instance.ShowInterstitialAd(MissionFailInit);
        }
        else
        {
            MissionFailInit();
        }
    }

    void MissionFailInit()
    {
        if (GameClient.mNetwork == true)
        {
            CNetwork.GetInstance().CS_Finish(SumScore.ToString(), CallBackFinish );
            Debug.Log("CS_FINISHFAIL");
            Debug.Log("acumul : " + AcumulSumScore);
        }

        GameClient.instance.mMysteryState = MysteryState.Stay;



        //GameObject _InstantObject = GameObject.Find("_InstantObject");
        Rigidbody[] data = _InstantObj.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody obj in data)
        {
            if (obj.gameObject.Equals(_InstantObj.name))
                continue;

            obj.useGravity = false;
            obj.isKinematic = true; //게임 실패시 리지드바디 제거
        }

        mMissionSuccess = false;

        FrontUIShow(false);
        StartCoroutine(MysteryInGameUI(true));
        InitMysteryGame();
        MBUIController.Instance.Init();
        MBUIController.Instance.RefreshMultiplier();
        //StartCoroutine(MBUIController.Instance.UpdateScore());
    }

   

    public void MissionSuccess()
    {
        Debug.Log("Jackpot!!!!!! ");

		//GameClient.instance.mMysteryState = MysteryState.Stay;

        if (GameClient.mNetwork == true && GameClient.instance.isVisitor == false)
        {
            

            Debug.Log("acumul : " + AcumulSumScore);
        }


        GameClient.instance.GetMachineData().theJackpotSum = 0;
        
        if (mEff_randomBox)
        {
            GameObject.DestroyImmediate(mEff_randomBox);
            mEff_randomBox = null;
        }

        GameClient.instance.StopBGM();


        if (GameClient.instance.isVisitor == false)
        {
            MBUIController.Instance.RefreshMultiplier();
            FrontUIShow(false);
            StartCoroutine(MysteryInGameUI(true));
            InitMysteryGame();
            MBUIController.Instance.Init();
        }
        
        //StartCoroutine(MBUIController.Instance.UpdateScore());
        //StartCoroutine(EffTicketText());

    }
    
    public IEnumerator EffectSuccessBlock()
    {
        //성공하면 모든 블럭의 중력을 뺀다
		mMissionSuccess = true;

        //GameObject _InstantObject = GameObject.Find("_InstantObject");
        Rigidbody[] data = _InstantObj.GetComponentsInChildren<Rigidbody>();

        //GetUser Success :: {"result":"ok","data":{"U_TICKET_CNT":1,"U_SAVE_TICKET_CNT":-2146554896,"U_COIN_CNT":9995}}

        foreach (Rigidbody obj in data)
        {
            if (obj.gameObject.Equals(_InstantObj.name))
                continue;

            obj.useGravity = false;
            obj.isKinematic = true; 
        }

        GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_BlockBomb);

		//destory block

		int count = mBlockList.Count; 


		for(int i = 0 ; i < count ; ++i )
		{
			GameObject go  = (GameObject)mBlockList[i];

			if( go )
			{
				StartCoroutine(go.GetComponent<BaseBlock>().ChangeMaterial());
				
				Vector3 pos = go.transform.position;    
				
				yield return new WaitForSeconds(0.2f);
				
				Instantiate(GameClient.instance.mEffFinish, new Vector3(pos.x, pos.y, pos.z - 3), Quaternion.identity);
				
				DestroyObject(go , 0.4f );
			}
        }

        //2014.04.14 이재우 잭팟시 티켓 리카운팅 적용 부분
        RecountWinText.Instance.enabled = true;
        
        //마구 움직이는 랜덤박스 끝났을때 연출

        mBG_Group.SetActive(false);
        GameObject jEff = Instantiate(GameClient.instance.mEffJackpotFinish) as GameObject;
        GameObject jEffParent = GameObject.Find("JackPartiCam");
        
        jEff.transform.parent = jEffParent.transform;
        jEff.transform.localPosition = new Vector3(0, -25108.81f, 40170.63f);
        jEff.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        jEff.transform.localScale = new Vector3(24709.64f, 567.8592f, 638.1621f);
        
        mPlayerBar.SetActive(false);
        
        mEff_randomBox = Instantiate(GameClient.instance.mEffRandomBox, new Vector3(0f, 8, -10), Quaternion.identity) as GameObject;
        GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_Win, 0.7f);
        
        
        //2014.04.14 이재우 잭팟시 티켓 리카운팅 적용 부분
     
        //yield return new WaitForSeconds(5f);

        

        yield return StartCoroutine(RecountWinText.Instance.Recount());
        

        
                

        long PrevTicket = MysteryMgr.Instance.mPrevUserTicket;
        long UserTicket = GameClient.instance.mUserTicket;
        long cur = GameClient.instance.mUserTicket - PrevTicket + GameClient.instance.GetMachineData().theJackpotSum;
        //Debug.Log("user : " + GameClient.instance.mUserTicket + " / sumJackpot : " + GameClient.instance.GetMachineData().theJackpotSum);


    }

//     public void EffectTicketPaper(bool bFlag)
//     {
// 
//         if (bFlag == true)
//         {
//            // mUITicketPaper.GetComponent<UISpriteAnimation>().enabled = false;
//             //mUITicketPaper.GetComponent<TweenScale>().enabled = bFlag; 
// 
//         }
//         else 
//         {
//             //mUITicketPaper.GetComponent<UISpriteAnimation>().enabled = true;
//             //mUITicketPaper.GetComponent<TweenScale>().enabled = bFlag; 
//         }
//     }


    IEnumerator EffectTicketText()
    {
        bool bLoop = true;


        float from = AcumulSumScore + SumScore;
        
        //float to = mScore * MBUIController.Instance.multiplier;
        //SumScore += mScore * MBUIController.Instance.multiplier;
        SumScore += MBUIController.Instance.tempLevelScore;
        float to = AcumulSumScore + SumScore;
        Debug.Log("Sum" + SumScore);
        //SumScore += mScore * MBUIController.Instance.multiplier;
//         if (mMissionSuccess == true)
//         {
//             
//             //SumScore += mScore;
//         }
//         else
//         {
//             to = GameClient.instance.mUserTicket + mRealScore * MBUIController.Instance.multiplier;
//             //SumScore += mRealScore;
//         }
//         
        long Result = 0;
        float t = 0f;
        float fNowtime = 0.0f;

        

        UILabel label = mTicketTextObj.GetComponent<UILabel>();
        
        while (bLoop)
        {
            fNowtime += Time.deltaTime;
            t = fNowtime / 0.5f;
            Result = System.Convert.ToInt64(Mathf.RoundToInt(Mathf.Lerp(from, to, t)));
            
            label.text = Result.ToString();

            if (t >= 1.0f)
            {
                //GameClient.instance.mUserTicket = Result;
                bLoop = false;
            }

            yield return null;
        }
    }



    public void ApplyItem(_BlockType type)
    {

    //    mDropItem = true; 

        mEffectDJacpot.SetActive(false);
        mEffectDTicket.SetActive(false);
        mEffectSpeedDown.SetActive(false); 
                
        if (type == _BlockType.SpeedDown)
        {
            mEffectSpeedDown.SetActive(true);
            mGetItem[(int)ItemType.SpeedDown] = true; 
        }
        else if (type == _BlockType.DoubleTicket)
        {
            mDoubleTicket = 2;
            mEffectDTicket.SetActive(true);
            mGetItem[(int)ItemType.DoubbleTicket] = true;
            //mScore = mScore * mDoubleTicket;
        }
        else if (type == _BlockType.DoubleJackpot)
        {
            mEffectDJacpot.SetActive(true);
            mDoubleJackpot = 2;
            mGetItem[(int)ItemType.DoubbleJackPot] = true;
        }
        //2014.04.16 이재우 미스터리 효과 딜레이
        //MBUIController.Instance.RefreshMultiplier();
        StartCoroutine(MBUIController.Instance.UpdateScore());
        StartCoroutine(Wait());
        
    }

    //2014.04.16 이재우 미스터리 효과 딜레이
    IEnumerator Wait()
    {
        //MBUIController.Instance.RefreshMultiplier();
        yield return new WaitForSeconds(2);
        StartCoroutine(GameClient.instance.VibrateController(1, 1));
        isMysteryEff = false;
        yield return null;
    }


    public void AddObject(GameObject obj)
    {
        if (mBlockList.Count > 0)
        {
            foreach (GameObject Obj in mBlockList)
            {
                if (Obj.Equals(obj))
                {
                     return; 
                }
            }
        }
        
		if(MysteryMgr.Instance.mMissionSuccess == true)
			return;  

        mBlockList.Add(obj);

		//MysteryMgr.Instance.Eff_Test(obj.gameObject.transform.position);


        //몇층을 쌓았냐 ? 체크 
        //CheckBlock();
    }
    
    public void DeleteObject(GameObject delobj , bool del = false)
    {
        if (mBlockList.Count > 0)
        {
            foreach (GameObject Obj in mBlockList)
            {
               if (Obj.Equals(delobj))
               {
                    mBlockList.Remove(delobj);
                    if(del) Destroy(delobj);
                    break;
               }
            }
        }
    }
	
		
    public void UpdateBlock(float fMove)
	{
		if (mBlockList.Count == 0 )
			return; 
	    		
		Vector3 newpos = Vector3.zero; 
		
		foreach (GameObject Obj in mBlockList)
        {
            if (Obj != null)
            {
                newpos = Obj.transform.position;
                newpos.x += fMove;
                Obj.transform.position = newpos;
            }
        }
	}

    //전면 전광판 UI 
    public void FrontUIShow(bool flag)
    {
        GameObject obj = mUIMystery.transform.Find("Camera/Anchor/Panel").gameObject;
        
        if (obj)
        {
            TicketValueText.gameObject.SetActive(flag);
            obj.transform.Find("WinZoneSprite").gameObject.SetActive(flag);
            obj.transform.Find("Score").gameObject.SetActive(flag);
            
        }

        /*
        GameObject.Find("UI_Mystery").transform.Find("Camera/Anchor/Panel/WinZoneSprite").gameObject.SetActive(flag);
        GameObject.Find("UI_Mystery").transform.Find("Camera/Anchor/Panel/Ticket").gameObject.SetActive(flag);
        GameObject.Find("UI_Mystery").transform.Find("Camera/Anchor/Panel/Score").gameObject.SetActive(flag);
        */

    }

    public void EffectTextTicket()
    {
        if (GameClient.instance.mMysteryState == MysteryState.Stay)
        {
            TicketValueText.text = "WIN";
            TicketValueText.transform.Find("Text").gameObject.SetActive(true);
        }
        else
        { 
            TicketValueText.text = "TICKET VALUE";
            TicketValueText.transform.Find("Text").gameObject.SetActive(false);
        }
    }


    
    public IEnumerator MysteryInGameUI(bool bActive)
    {
        
        if (bActive == false)
        {
            //drop 이 끝나지 않은상태에서 UI 꺼버리면 에러가남 드랍이 다 나오기를 대기.. 
            yield return new WaitForSeconds(0.1f);
        }
        
       

        mUIIngamePanel.SetActive(bActive);
        mCameraObj.SetActive(bActive);
        MBUIController.Instance.CoinPanel.SetActive(bActive);
        //ShowTicketMachine(!bActive);

    }
    

    public void ShowTicketMachine(bool bActive)
    { 
        //_Active
        Debug.Log("ShowTicketMachine");

        //mUIResultPanel.SetActive(bActive);
        mTicketMachine.SetActive(true);

    }

    public void OpenTicketMachine()
    {
        StartCoroutine(MysteryInGameUI(false));
       
		ShowTicketMachine(true);
    }
    
    //블럭을 체크한다. 

    public void Eff_Test(Vector3 pos)
    {
        pos = mCamera.WorldToViewportPoint(pos);
        pos = mUICamera.ViewportToWorldPoint(pos);
        
        /*
        GameObject obj = Instantiate(GameClient.instance.mEffSpark, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;
        obj.transform.parent = mUICamera.transform.parent;
        obj.transform.localScale = GameClient.instance.mEffSpark.transform.localScale;
        */


    }


    public void Eff_Bomb(Vector3 pos)
    {
        Vector3 Curpos = pos; 
        
        //이펙트 방출
        //pos = Vector3.zero;
        /*
        pos = mCamera.WorldToViewportPoint(pos);
        pos = mUICamera.ViewportToWorldPoint(pos);  
        GameObject obj = Instantiate(GameClient.instance.mEffBomb1, new Vector3(pos.x , pos.y , 0), Quaternion.identity) as GameObject;
        obj.transform.parent = mUICamera.transform.parent;
        obj.transform.localScale = GameClient.instance.mEffBomb1.transform.localScale;
        */

        //GameObject obj = NGUITools.AddChild(mUICamera.transform.parent.gameObject , GameClient.instance.mEffBomb1);
        //obj.transform.position = new Vector3(pos.x, pos.y, 0);
        //obj.transform.localScale = GameClient.instance.mEffBomb1.transform.localScale;

        

        switch (mBlockStep)
        {
            case 1:
            case 2:
            case 3:
              //  pos.x = Random.Range(mCamera.transform.position.x - 1f, mCamera.transform.position.x + 1f);
              //  pos.y = Random.Range(mCamera.transform.position.y - 1f, mCamera.transform.position.y + 2f);
                Instantiate(GameClient.instance.mEffCoinBomb , new Vector3(Curpos.x, Curpos.y, Curpos.z), Quaternion.identity);
                StartCoroutine(EffectTicketText());
                StartCoroutine(MBUIController.Instance.UpdateScore());
                MBUIController.Instance.RefreshMultiplier();
                

                if(GameClient.instance.mbEffectSound)  GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_LevelSuccess);//NGUITools.PlaySound(GameClient.instance.mSnd_LevelSuccess);
                break; 
            case 4:
                StartCoroutine(MBUIController.Instance.UpdateScore());
                //StartCoroutine(EffectTicketText());
                StartCoroutine(EffectSuccessBlock());

                break;
        }

        ShowTwinkleEffect();
    }

    


    public void ShowTwinkleEffect()
    {
        Debug.Log("ShowTwinkleEffect");
        mUISliderBar.GetComponent<EffSliderbar>().Play(LightMode.Twinkle, 2f);
        mUITopBar.GetComponent<EffChangeSprite>().Play();

    }

    //by sdh 01 팝업
    public void CallBackFinish()
    {
        CPopupMgr.instance.OpenWebPopupView(MysteryMgr.Instance.webPopupView, false);
    }

    void OnDestory()
    {
        mBlockList.Clear();
		mBlockList = null ;
     
        mBackGroundData = null;
        BlockArrangement = null;
    }



}
