using UnityEngine;
using System.Collections;

//2014.04.14 이재우 티켓 리카운트 클래스

public class RecountWinText : MonoBehaviour
{
    public GameObject WinticketAniA;
    public GameObject WinticketAniB;
    public GameObject MainTicketSprite;
    public bool isSkip = false;

    

    public UILabel label;

    private static RecountWinText minstance = null;

    //처음 로비로 접속

    public static RecountWinText Instance
    {
        get
        {
            if (minstance == null)
            {
                minstance = FindObjectOfType<RecountWinText>();

                if (minstance == null)
                {
                    minstance = new GameObject("GameClient").AddComponent<RecountWinText>();
                    //m_instance = Instantiate(m_instance) as GameManager;
                }
            }
            return minstance;
        }

    }

    public int winScore = 0;
    public int targetScore = 0;

    public GameObject tempWinScore;

    public void RecountSnd(float volume = 1.0f)
    {
        GameObject go = new GameObject("countSnd");
        AudioSource ASource = go.AddComponent<AudioSource>() ;
        ASource.clip = GameClient.instance.mSnd_Count;
        ASource.loop = true;
        ASource.volume = volume;
        ASource.rolloffMode = AudioRolloffMode.Linear;
        ASource.Play();            
    }

    public void StopRecountSnd(float volume = 1.0f)
    {
        GameObject go = GameObject.Find("countSnd");
        Destroy(go);
    }

    void Start()
    {
        label.gameObject.SetActive(false);
    }

    void Update()
    {
    }

    bool bTicketLoop = false;

    public IEnumerator WinTicketAni()
    {
        GameObject ticketSnd = new GameObject("ticketSnd");
        AudioSource source = ticketSnd.AddComponent<AudioSource>();
        source.clip = GameClient.instance.mSnd_WinTicketUpdate;
        source.loop = true;
        source.volume = 1;
        

        WinticketAniA.SetActive(true);
        WinticketAniB.SetActive(true);

        TweenPosition tpA = WinticketAniA.GetComponent<TweenPosition>();
        TweenPosition tpB = WinticketAniB.GetComponent<TweenPosition>();
        TweenRotation trMain = MainTicketSprite.GetComponent<TweenRotation>();

        bTicketLoop = true;
        source.Play();
        WinticketAniA.SetActive(true);
        WinticketAniB.SetActive(true);
        tpA.enabled = true;
        tpB.enabled = true;

        //GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_WinTicketUpdate);


        trMain.enabled = false;

        MainTicketSprite.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        while (bTicketLoop)
        {
            yield return null;
        }
        
        if (bTicketLoop == false)
        {

            Debug.Log("Stop Loop ");

            tpA.enabled = false;
            tpB.enabled = false;
            WinticketAniA.SetActive(false);
            WinticketAniB.SetActive(false);
            source.Stop();

            trMain.enabled = true;
            trMain.style = UITweener.Style.PingPong;
            trMain.duration = 0.5f;

            yield return new WaitForSeconds(1f);

            trMain.enabled = false;
            
            trMain.enabled = true;
            trMain.duration = 0.2f;
        }
        yield return null;
    }

    float fvibTimer = 0;

    public IEnumerator Recount()
    {
        isSkip = true;
        winScore = (GameClient.instance.GetMachineData().theJackPot + GameClient.instance.GetMachineData().theJackpotSum) * MBUIController.Instance.multiplier;
        int JacpotScore = winScore;

        targetScore = 0;

        UILabel ticketLabel = MysteryMgr.Instance.mTicketTextObj.GetComponent<UILabel>();
        long ticket = GameClient.instance.mUserTicket + MysteryMgr.Instance.SumScore;
                
        RecountSnd();
        StartCoroutine(WinTicketAni());
        StartCoroutine(FlashText());
        

        while (winScore > 0)
        {
            fvibTimer += Time.deltaTime;
            if (fvibTimer > 1)
            {
                StartCoroutine(GameClient.instance.VibrateController(1, 1));
                fvibTimer = 0;
            }
            
            
            ticket++;
            ticketLabel.text = ticket.ToString();
            
            winScore--;
            tempWinScore.GetComponent<UILabel>().text = winScore.ToString();
            
            if (Input.GetMouseButtonUp(0))
            {
                winScore = 0;
                
                break; 
            }
                        
            yield return new WaitForSeconds(0.01f);
        }

        tempWinScore.SetActive(false);
        
        //최종으로 유저티켓에 적용함
        //GameClient.instance.mUserTicket += JacpotScore;
        MysteryMgr.Instance.SumScore += JacpotScore;


        ticketLabel.text = (GameClient.instance.mUserTicket + MysteryMgr.Instance.SumScore).ToString();



        isSkip = false;
        bTicketLoop = false;
        if (GameClient.instance.isVisitor == false)
        {
            if (GameClient.mNetwork == true)
            {
                CNetwork.GetInstance().CS_Finish(MysteryMgr.Instance.SumScore.ToString(), WinPopupCallback);
                Debug.Log("CS_FINISHSUCCESS");
            }
            else
            {
                MysteryMgr.Instance.c_resultView.OpenResultWnd((GameClient.instance.GetMachineData().theJackPot + GameClient.instance.GetMachineData().theJackpotSum) * MBUIController.Instance.multiplier);
            }
            
        }
        
        
        //ticketLabel.text = GameClient.instance.mUserTicket.ToString();
        StopRecountSnd();

        

        if (GameClient.instance.isVisitor == true)
        {
            yield return new WaitForSeconds(1f);
            this.enabled = false;
        }
        
        
    }

    void WinPopupCallback()
    {
        
        //c_resultView.OpenResultWnd((GameClient.instance.GetMachineData().theJackPot + GameClient.instance.GetMachineData().theJackpotSum) * MBUIController.Instance.multiplier);
        MysteryMgr.Instance.c_resultView.OpenResultWnd((GameClient.instance.GetMachineData().theJackPot + GameClient.instance.GetMachineData().theJackpotSum) * MBUIController.Instance.multiplier);
        CPopupMgr.instance.OpenWebPopupView(MysteryMgr.Instance.webPopupView, true);
    }

    IEnumerator FlashText()
    {
        while (isSkip)
        {
            label.text = "Touch screen to skip";
            label.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            label.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        label.text = "";
        label.gameObject.SetActive(false);
        yield return null;
    }

    void OnEnable()
    {
        MBUIController.Instance.mScoreText.SetActive(false);
        tempWinScore.SetActive(true);
    }

    void OnDisable()
    {
   
        MBUIController.Instance.mScoreText.SetActive(true);

        MysteryMgr.Instance.MissionSuccess();

        if (GameClient.instance.isVisitor == true)
        {
            MysteryMgr.Instance.tutoIngame.isClearMission = true;
            MysteryMgr.Instance.tutoIngame.dlg++;
            MysteryMgr.Instance.tutoIngame.Ingamedialog = MysteryMgr.Instance.tutoIngame.dlg;
        }
    }
}
