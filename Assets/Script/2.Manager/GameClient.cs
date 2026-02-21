
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Generic;


public enum _enMachineGrade
{
    None,
    starter,
    challenge,
    surprise,
    miracle,
    adventure,
    gevent,
}

public enum _enMachineType
{
    Static,
    Multiple,
}




public class MachineData
{
    
    public string theNumber; // 머신 고유 번호
    public string theName;     //머신 이름
    public int theJackPot; 
    public int theJackpotSum;  //누적잭팟금액
    public int theNeedCoin; //한게임당 필요코인수
    public _enMachineGrade MachineGrade;
    public bool theStatus;
    public int theLevelValue;    //cms 
    


    //public int[] mdscoredata; 
    



    public MachineData()
    {
        
        theName = "Mystery Block";
        theNumber = "1";
        theJackPot = 500;
        theNeedCoin = 100;
        theJackpotSum = 0;
        theLevelValue = 1;
        theStatus = true;
        MachineGrade = _enMachineGrade.starter;

        //mdscoredata = new int[9, 4] { { 4, 12, 40, 400 }, { 4, 12, 40, 400 }, { 6, 18, 60, 600 }, { 6, 18, 60, 600 }, { 6, 18, 60, 600 }, { 8, 24, 80, 800 }, { 10, 30, 100, 1000 }, { 10, 30, 100, 1000 }, { 8, 24, 80, 800 } };
        //mdscoredata = new int[4];

    }

    public void SetData(int Value ,  string _Number , string _Name , int _Jackpot, int _Jackpotsum , int _NeedCoin, _enMachineGrade _Grade , bool _Status )
    {
        theLevelValue = Value; 
        theNumber = _Number;
        theName = _Name;
        theJackPot = _Jackpot;
        theJackpotSum = _Jackpotsum;
        theNeedCoin = _NeedCoin;
        MachineGrade = _Grade;
        theStatus = _Status;
    }

    /*public void SetMachineScore(int[] scorestep, int jackpot, int coin, _enMachineGrade grade)
    {
        theJackPot = jackpot;
        theNeedCoin = coin;
        MachineGrade = grade;
        mdscoredata = scorestep;
    }*/
}




public class GameClient : MonoBehaviour
{


    public static bool mDevTest = false;

#if UNITY_EDITOR
    public static bool mNetwork = false;
#else 
    public static bool mNetwork = true; 
#endif

#if UNITY_EDITOR
    public bool isVisitor = true;
#else
    public bool isVisitor = true;
#endif
    





	


    /*
	public static GameClient Instance 
	{
        get{
            if (minstance == null)
                minstance = new GameClient();
						
            return minstance;
        }
    }
    */


    static public GameState mGameState = GameState.Login;
    public MysteryState mMysteryState;
    static string scenename = null;



    public bool mPause = false;

    //--------------------------------------------------------------
    // User Data Infomation 
    //-------------------------------------------------------------
    
    public long mUserTicket = 0; // 유져 보유 티켓수
    public long mUserCoin = 1000; //게임 시작전에 들어가는코인 갯수
    public long mUserSTicket = 0;
    
    public int mSelectMachine = 0;

    //20140826 ljw 무료게임추가
    public int maxShare = 0;
    public int curShare = 0;

    
    
    [HideInInspector]   public bool mbBGMSound;
    [HideInInspector]   public bool mbEffectSound;
    [HideInInspector]   public bool mbTilt;
    [HideInInspector]   public bool mbVibration;
    [HideInInspector]   public bool mbTutorial;
    [HideInInspector]   public bool mbLogin;
    [HideInInspector]   public string mUserID;
    [HideInInspector]   public string mUserPW;


    [HideInInspector]   public bool mbRankPopUp = false;
    
    public ArrayList mMachinelist = new ArrayList();
    //public ArrayList mMachineScoreList = new ArrayList();


    //리소스 프랍
    [HideInInspector]   
    public GameObject mEffBomb = null;
    [HideInInspector]   
    public GameObject mEffDoubleJackpot = null;
    [HideInInspector]   
    public GameObject mEffSlowdown = null;
    [HideInInspector]   
    public GameObject mEffFinish = null;
    [HideInInspector]
    public GameObject mEffX2 = null;
    [HideInInspector]
    public GameObject mEffCoinBomb = null;
    [HideInInspector]
    public GameObject mEffMoveTicket= null;
    public GameObject mEffJackpotFinish = null;

    //public TutorialLobby tutorial;
    //public TutorialInGame gameTuto;
    //public TutorialModelController tutoModelCon;
    public bool isIngame = false;


    [HideInInspector]   public GameObject mEffBlockBox = null;
    [HideInInspector]   public GameObject mEffRandomBox = null;
    [HideInInspector]   public GameObject mBlock = null;
    
    [HideInInspector]   public AudioSource mLobbyBGM = null;
    [HideInInspector]   public AudioSource mMysteryBGM = null;
    //[HideInInspector]   public AudioClip mLobbyBGMclip = null;
    //[HideInInspector]   public AudioClip mMysteryBGMclip = null;  

    [HideInInspector] public AudioClip mSnd_Zoomin = null;
    [HideInInspector] public AudioClip mSnd_ZoomOut = null;
    [HideInInspector] public AudioClip mSnd_Win = null;
    [HideInInspector] public AudioClip mSnd_ReadGo = null;
    [HideInInspector] public AudioClip mSnd_MBlock = null;
    [HideInInspector] public AudioClip mSnd_Lose = null;
    [HideInInspector] public AudioClip mSnd_InsertCoin = null;
    [HideInInspector] public AudioClip mSnd_InsertCoinx2 = null;
    [HideInInspector] public AudioClip mSnd_FailDrop = null;
    [HideInInspector] public AudioClip mSnd_Button = null;
    [HideInInspector] public AudioClip mSnd_BlockDrop = null;
    [HideInInspector] public AudioClip mSnd_LevelSuccess = null;
    [HideInInspector] public AudioClip mSnd_Ticketing = null; 
    [HideInInspector] public AudioClip mSnd_BlockBomb = null;
    [HideInInspector] public AudioClip mSnd_MBlockBomb = null;
    public AudioClip mSnd_x2On = null;
    public AudioClip mSnd_x2Off = null;
    public AudioClip mSnd_Count = null;
    public AudioClip mSnd_GameOver = null;
    public AudioClip mSnd_WinTicketUpdate = null;


    

    
    
    [HideInInspector]
    public float mBlockSpeed = 0.2f; 


     //mLobbyBGM.clip = Resources.Load("sound/MenuBGM") as AudioClip;

    //--------------------------------------------------------
    // Add the code from here - Get Set 
    //-----------------------------------------------------------

    public bool IsPause
    {
        set
        {
            if (value == false)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.0f;
            }
            mPause = value;
            Debug.Log("IsPause " + mPause);

            if (mGameState == GameState.Lobby)
            {
                if (mPause == true)
                    CMachineList.GetInstance().SetBoxColl(false);
                else
                    Invoke("DelayMachineBoxColl", 0.1f);

            }

        }
        get
        {
            return mPause;
        }

    }


    void DelayMachineBoxColl()
    {
        CMachineList.GetInstance().SetBoxColl(true);
    }



    static public string SceneName
    {
        get {
            if (mGameState == GameState.Lobby)
                scenename = "LobbyStage";
            else if (mGameState == GameState.Mystery)
                scenename = "MysteryGame";
            else if (mGameState == GameState.Login)
                scenename = "LoginScene";
			else if (mGameState == GameState.DirectStart)
				scenename = "LoginScene";
            return scenename;
        }

        set { scenename = value; }
    }

    void Awake()
    {
        //mInfo.BlockSpeed = 1.0f; 
        
        Debug.Log("GameClient Start");
        DontDestroyOnLoad(this);
        
        MachineData[] _MachineDatalist = new MachineData[9];


        for (int i = 0; i < _MachineDatalist.Length; i++)
        {
            _MachineDatalist[i] = new MachineData();
            mMachinelist.Add(_MachineDatalist[i]);
        }

        _MachineDatalist[0].SetData(500, "11", "DOD0001", 400, 0, 2, _enMachineGrade.starter, true);
        /*
        _MachineDatalist[1].SetData(501, "11", "DOD0001", 500, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[2].SetData(502, "11", "DOD0001", 600, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[3].SetData(503, "11", "DOD0001", 700, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[4].SetData(504, "11", "DOD0001", 800, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[5].SetData(505, "11", "DOD0001", 900, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[6].SetData(506, "11", "DOD0001", 1000, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[7].SetData(507, "11", "DOD0001", 1100, 0, 1, _enMachineGrade.starter, true);
        _MachineDatalist[8].SetData(508, "11", "DOD0001", 3000, 0, 1, _enMachineGrade.gevent, true);
        */

        LoadData();
        LoadTutResult();
        

        //by sdh 01 팝업
        CPopupMgr.instance.LoadPopupInfo();

    }

    /*
    Application.RegisterLogCallback(LogCallBack);
    public string Logstring = "";
    public void LogCallBack(string logString, string stackTrace, LogType type)
    {
        Logstring = logString;
    }
    */

    void Start()
    {
        
    }

    void LoadData()
    {
        Debug.Log("GameClient Resource Load..... ");
        
        mLobbyBGM = gameObject.AddComponent<AudioSource>();
        mLobbyBGM.clip = Resources.Load("sound/MenuBGM") as AudioClip;
        mLobbyBGM.volume = 0.5f;
        mLobbyBGM.playOnAwake = false;
        mLobbyBGM.loop = true;

        mMysteryBGM = gameObject.AddComponent<AudioSource>();
        mMysteryBGM.clip = Resources.Load("sound/PlayBGM") as AudioClip;
        mMysteryBGM.volume = 0.5f;
        mMysteryBGM.playOnAwake = false;
        mMysteryBGM.loop = true;


        mSnd_Zoomin = Resources.Load("sound/zoom in") as AudioClip;
        mSnd_ZoomOut = Resources.Load("sound/PlayBGM") as AudioClip;
        mSnd_Win = Resources.Load("sound/Win") as AudioClip;
        mSnd_ReadGo = Resources.Load("sound/ReadyGo") as AudioClip;
        mSnd_MBlock = null;
        mSnd_Lose = Resources.Load("sound/Lose") as AudioClip;
        mSnd_InsertCoin = Resources.Load("sound/InsertCoin") as AudioClip;
        mSnd_InsertCoinx2 = Resources.Load("sound/insert coin_x2") as AudioClip;
        mSnd_FailDrop = Resources.Load("sound/Fail_") as AudioClip;
        mSnd_Button = Resources.Load("sound/Button") as AudioClip;
        mSnd_BlockDrop = Resources.Load("sound/BlockDrop") as AudioClip;
        mSnd_LevelSuccess = Resources.Load("sound/Level_success") as AudioClip;
        mSnd_Ticketing = Resources.Load("sound/Ticket_PRN") as AudioClip;
        mSnd_BlockBomb = Resources.Load("sound/BlockBomb") as AudioClip;
        mSnd_MBlockBomb = Resources.Load("sound/M_block_effect_sound") as AudioClip;
        mSnd_x2On = Resources.Load("sound/X2On") as AudioClip;
        mSnd_x2Off = Resources.Load("sound/X2Off") as AudioClip;
        mSnd_Count = Resources.Load("sound/Count") as AudioClip;
        mSnd_GameOver = Resources.Load("sound/Fail") as AudioClip;
        mSnd_WinTicketUpdate = Resources.Load("sound/Ticket") as AudioClip;

        mEffBomb = (GameObject)Resources.Load("effect/bomb") as GameObject;
        mEffDoubleJackpot = (GameObject)Resources.Load("effect/Effect_DoubleJackPot") as GameObject;
        mEffSlowdown = (GameObject)Resources.Load("effect/Effect_Slowdown") as GameObject;
        mEffX2 = (GameObject)Resources.Load("effect/Effect_X2") as GameObject;
        mEffFinish = (GameObject)Resources.Load("effect/Effect_Sucessed") as GameObject;
        mEffCoinBomb = (GameObject)Resources.Load("effect/Effect_CoinBomb") as GameObject;
        mEffMoveTicket = (GameObject)Resources.Load("effect/Effect_MoveTicket") as GameObject;
        
        mEffBlockBox = (GameObject)Resources.Load("effect/BlockBox") as GameObject;
        mEffRandomBox = (GameObject)Resources.Load("effect/RandomBox") as GameObject;
        mEffJackpotFinish = (GameObject)Resources.Load("CommonUse_Effect/FireWorksRed") as GameObject;
        mBlock = (GameObject)Resources.Load("prefabs/Block") as GameObject;

        Resources.LoadAll("Texture/BlockParticle/01", typeof(Texture));
        Resources.LoadAll("Texture/BlockParticle/02", typeof(Texture));
        Resources.LoadAll("Texture/BlockParticle/03", typeof(Texture));



        //Path.GetDirectoryName()

        mbBGMSound = true;
        mbEffectSound = true;
        mbTilt = false;
        mbVibration = true;
        mbLogin = false;
        //mbTutorial = false; 
        mUserID = "";
        mUserPW = "";


        LoadUserOption();
    }
    
	public void LoadingScene()
	{
        //LoadTutResult();
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(GoogleAnalytics.instance.LogScreenWWW());
	}

    public void Vibration()
    {
        if (mbVibration == true)
        {
#if UNITY_IOS
        Handheld.Vibrate();
#endif
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }


    }

   

    public IEnumerator VibrateController(float delay, int cnt)
    {
        int count = cnt;
        while (count > 0)
        {
            Vibration();
            yield return new WaitForSeconds(delay);
            count--;
        }

    } 

    
    //--------------------------------------------------------------------------------------
    // Get Set 
    //---------------------------------------------------------------------------------------
 
    public int GetMachineJackPot()
    {

        if (Global.USER_GONUM != string.Empty)
        {
            foreach (MachineData tmp in mMachinelist)
            {
                if (tmp.theNumber == Global.USER_GONUM)
                {
                    return tmp.theJackPot;
                }
            }
        }


        MachineData data = (MachineData)mMachinelist[mSelectMachine];

        //Debug.Log("Jackpot " + data.theJackPot); 

        return data.theJackPot;
    }

    public MachineData GetMachineList(int index)
    {
        return (MachineData)mMachinelist[index];
    }
    

    public MachineData GetMachineData()
    {

        if (Global.USER_GONUM != string.Empty )
        {
            foreach (MachineData tmp in mMachinelist)
            {
                if (tmp.theNumber == Global.USER_GONUM)
                {
                    return (MachineData)tmp;
                }
            }
        }
        

        return (MachineData)mMachinelist[mSelectMachine];
    }
    
    public int GetSelectMachine()
    {
        
        if (Global.USER_GONUM != string.Empty )
        {
            for(int i = 0; i < mMachinelist.Count; ++i)
            {
                MachineData tmp = (MachineData)mMachinelist[i];
                if( tmp.theNumber == Global.USER_GONUM )
                {
                    return i;
                }
            }
        }
        return mSelectMachine; 
    }



    /*public int[] GetMachinScore()
    {
        MachineData data = (MachineData)mMachinelist[mSelectMachine];
        return data.mdscoredata;
    }*/

    //유저 튜토리얼 진행 정보 저장
    public void SaveTutResult()
    {
        PlayerPrefs.SetInt("Tuto", System.Convert.ToInt32(isVisitor));
        PlayerPrefs.Save();
    }

    //유저 튜토리얼 진행 정보 로드
    public void LoadTutResult()
    {
        if (PlayerPrefs.HasKey("Tuto") == true)
        {
            isVisitor = System.Convert.ToBoolean(PlayerPrefs.GetInt("Tuto"));
        }
    }

    //세이브및 로드
    public void SaveUserOption()
    {
        PlayerPrefs.SetInt("BGMSound", System.Convert.ToInt32(mbBGMSound));
        PlayerPrefs.SetInt("EffectSound", System.Convert.ToInt32(mbEffectSound));
        PlayerPrefs.SetInt("Tilt", System.Convert.ToInt32(mbTilt));
        PlayerPrefs.SetInt("Vibration", System.Convert.ToInt32(mbVibration));
        PlayerPrefs.SetInt("Tutorial", System.Convert.ToInt32(mbTutorial));
        PlayerPrefs.SetInt("Login", System.Convert.ToInt32(mbLogin));

        //PlayerPrefs.SetString("UserID", );
        //PlayerPrefs.SetString("UserPW", );   
        
        PlayerPrefs.Save();
    }
    
    public void LoadUserOption()
    {
        if (PlayerPrefs.HasKey("BGMSound") == true)
        {
            mbBGMSound = System.Convert.ToBoolean( PlayerPrefs.GetInt("BGMSound") ) ;
        }
        if (PlayerPrefs.HasKey("EffectSound") == true)
        {
            mbEffectSound = System.Convert.ToBoolean(PlayerPrefs.GetInt("EffectSound") ) ;
        }
        
        if (PlayerPrefs.HasKey("Tilt") == true)
        {
            mbTilt = System.Convert.ToBoolean(PlayerPrefs.GetInt("Tilt"));
        }

        if (PlayerPrefs.HasKey("Vibration") == true)
        {
            mbVibration = System.Convert.ToBoolean(PlayerPrefs.GetInt("Vibration"));
        }
        
        if(PlayerPrefs.HasKey("Tutorial") == true) 
        {
            mbTutorial = System.Convert.ToBoolean(PlayerPrefs.GetInt("Tutorial"));
        }

        
    }
   
    public void PlayBGM()
    {
        
        if (mbBGMSound == false) return;
        
        Debug.Log("PlayBGM()  " + mGameState);
        StopBGM();
        if (mGameState != GameState.Mystery)
        {
            if (mLobbyBGM != null && mLobbyBGM.isPlaying == false)
                Debug.Log("start lobby bgm");
                mLobbyBGM.Play();
        }
        else if (mGameState == GameState.Mystery)
        {
            //2014.04.17 이재우 사운드 겹침 수정중
            if (mMysteryBGM != null && mMysteryBGM.isPlaying == false)
                Debug.Log("start ingame bgm");
                mMysteryBGM.Play();

            /*
            if (mMysteryState == MysteryState.InGame)
            {
                if (mMysteryBGM != null && mMysteryBGM.isPlaying == false)
                    mMysteryBGM.Play();
            }
            else
            { 
                if (mLobbyBGM != null && mLobbyBGM.isPlaying == false)
                    mLobbyBGM.Play();
            }*/
        }
    }
        
    public void StopBGM()
    {
       Debug.Log("StopBGM");

       if (mMysteryBGM.isPlaying == true)
       {
        mMysteryBGM.Stop();
        }
           

       if (mLobbyBGM.isPlaying == true)
        {
        mLobbyBGM.Stop();
        }
           
       
    }



    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    void OnDestroy()
    {
        SaveUserOption();
        DestoryData();
    }

	
	public void DestoryData()
	{
        Debug.Log("DestoryData");

        mEffBomb = null;

        mEffDoubleJackpot = null;

        mEffSlowdown = null;

        mEffFinish = null;

        mEffX2 = null;

        mEffCoinBomb = null;

        mEffMoveTicket = null;

        mEffBlockBox = null;
        mEffRandomBox = null;
        mBlock = null;

        mLobbyBGM = null;
        mMysteryBGM = null;
        mEffJackpotFinish = null;

        mSnd_Zoomin = null;
        mSnd_ZoomOut = null;
        mSnd_Win = null;
        mSnd_ReadGo = null;
        mSnd_MBlock = null;
        mSnd_Lose = null;
        mSnd_InsertCoin = null;
        mSnd_InsertCoinx2 = null;
        mSnd_FailDrop = null;
        mSnd_Button = null;
        mSnd_BlockDrop = null;
        mSnd_LevelSuccess = null;
        mSnd_Ticketing = null;
        mSnd_BlockBomb = null;
        mSnd_MBlockBomb = null;
        mSnd_Count = null;
        mSnd_GameOver = null;
        mSnd_WinTicketUpdate = null;
        mSnd_x2On = null;
        mSnd_x2Off = null;
        for (int i = 0; i < mMachinelist.Count; ++i)
        {
            mMachinelist[i] = null;
        }

        mMachinelist.Clear();
        
    }
    
    static public void OneShotSound(Vector3 pos , AudioClip clip , float volume = 1.0f)
    {

        if (GameClient.instance.mbEffectSound == false) return; 

        GameObject go = new GameObject(clip.name);

        //pos.y = pos.y + 2f;
        
        go.transform.position = pos;
        AudioSource source = go.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.Play();
        
        GameObject.Destroy((Object)go, clip.length+2.0f );
              

        //Debug.Log("OneShotSound " +clip.length);

    }

    private static GameClient minstance = null;

    //처음 로비로 접속

    public static GameClient instance
    {
        get
        {
            if (minstance == null)
            {
                minstance = FindObjectOfType<GameClient>();

                if (minstance == null)
                {
                    minstance = new GameObject("GameClient").AddComponent<GameClient>();
                    //m_instance = Instantiate(m_instance) as GameManager;
                }
            }
            return minstance;
        }

    }
    

}
