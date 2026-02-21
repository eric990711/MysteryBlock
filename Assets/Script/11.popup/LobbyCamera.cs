using UnityEngine;
using System.Collections;

public enum LobbyState
{
    SelectMode,
    GuideMode,
    CoinMode,
}

public enum emCameraMove
{
    None,
    Left,
    Right,
    LeftSkip,
    RightSkip
}

public class LobbyCamera : MonoBehaviour
{
    public Camera CurCamera;
    public float m_Distance = 20f;
    public float m_Height = 3f;
    public float m_Damping = 10f;

    private Vector3 m_Up = Vector3.up;

    bool bUpdateCam = false;
    private float m_DelayTime;
    private float m_CamMovingDelTime = 0f;

    private Vector3 mTargetPos = Vector3.zero;
    private bool bPicking = false;
    private Vector3 mStartPos = Vector3.zero;
    public float speed = 6f;
    private Transform mTrans;


    private float fTouchMove = 0.0f;
    bool bTouch = false;

    bool bTutoMachineTouch = false;

    public LobbyState mLobbyState;

    public GameObject[] GuideSprite = new GameObject[6];


    public GameObject CoinPageObj;

    public TweenRotation mTwRotation = null;
    emCameraMove mCameraMove = emCameraMove.None;
    int Currentpage = 0;
    int MachineIndex = 0;

    // Use this for initialization

    void Awake()
    {
        bPicking = false;
        mTrans = transform;
        fTouchMove = 0;
        mLobbyState = LobbyState.SelectMode;

        foreach (GameObject gb in GuideSprite)
            gb.SetActive(false);

        Currentpage = 0;

        mTwRotation = GetComponent<TweenRotation>();

        CameraMove = emCameraMove.None;

        //2014.04.15 이재우 인게임에서 로비로 이동시 해당 머신으로 이동
        MachineIndex = GameClient.instance.GetSelectMachine();
        GameClient.instance.mSelectMachine = MachineIndex;

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 30 * MachineIndex, 0));


        this.GetComponent<TweenRotation>().from = gameObject.transform.rotation.eulerAngles;
        this.GetComponent<TweenRotation>().to = gameObject.transform.rotation.eulerAngles;
    }
    /*
    public void ContinueCamPos()
    {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 30 * GameClient.instance.GetSelectMachine(), 0));
    }*/


    //로비 카메라 회전
    public emCameraMove CameraMove
    {
        get
        {
            return mCameraMove;
        }
        set
        {
            if (mTwRotation && mTwRotation.enabled == false)
            {

                if (value == emCameraMove.Right)
                {
                    MachineIndex++;

                    if (MachineIndex > Global.MAXMACHINE)
                    {
                        MachineIndex = Global.MAXMACHINE;
                        return;
                    }

                    mTwRotation.from = mTwRotation.to;
                    mTwRotation.to = new Vector3(0, mTwRotation.to.y + 30.0f, 0);
                    mTwRotation.enabled = true;
                    mTwRotation.Reset();

                    GameClient.instance.mSelectMachine = MachineIndex;

                    LobbyMgr.Instance.UpdateMachine();
                }
                else if (value == emCameraMove.Left)
                {
                    MachineIndex--;

                    if (MachineIndex < 0)
                    {
                        MachineIndex = 0;
                        return;
                    }

                    mTwRotation.from = mTwRotation.to;
                    mTwRotation.to = new Vector3(0, mTwRotation.to.y - 30.0f, 0);
                    mTwRotation.enabled = true;
                    mTwRotation.Reset();

                    GameClient.instance.mSelectMachine = MachineIndex;

                    LobbyMgr.Instance.UpdateMachine();
                }
                //2014.04.15. 이재우 카메라 스킵 적용 
                else if (value == emCameraMove.LeftSkip)
                {
                    if (MachineIndex != 0) return;
                    MachineIndex = 8;
                    /*
                       gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 240, 0));
                       mTwRotation.from = gameObject.transform.rotation.eulerAngles;
                       mTwRotation.to = gameObject.transform.rotation.eulerAngles;
                     */
                    mTwRotation.from = mTwRotation.to;
                    mTwRotation.to = new Vector3(0, mTwRotation.to.y - 120.0f, 0);
                    mTwRotation.enabled = true;
                    mTwRotation.Reset();

                    GameClient.instance.mSelectMachine = MachineIndex;
                    LobbyMgr.Instance.UpdateMachine();
                }
                else if (value == emCameraMove.RightSkip)
                {
                    if (MachineIndex != 8) return;
                    MachineIndex = 0;

                    //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    //mTwRotation.from = gameObject.transform.rotation.eulerAngles;
                    //mTwRotation.to = gameObject.transform.rotation.eulerAngles;


                    mTwRotation.from = mTwRotation.to;
                    mTwRotation.to = new Vector3(0, mTwRotation.to.y + 120.0f, 0);
                    mTwRotation.enabled = true;
                    mTwRotation.Reset();

                    GameClient.instance.mSelectMachine = MachineIndex;
                    LobbyMgr.Instance.UpdateMachine();
                }



                mCameraMove = value;
            }
        }
    }


    void Start()
    {

        GameClient.instance.StopBGM();
        GameClient.instance.PlayBGM();


    }

    void OnEnable()
    {
        GameClient.instance.IsPause = false;

    }


    // Update is called once per frame
    void Update()
    {

        //by sdh 01 팝업
        if (CPopupMgr.instance.bOnWebview == true) return;

        if (GameClient.instance.IsPause == true) return;

        if (GameClient.instance.mbRankPopUp == true) return;

        if (LoadingBar.GetInstance().GetLock() == true)
            return;

        //if (GameClient.instance.tutorial.isVisitor == true) return;



        if (mLobbyState == LobbyState.SelectMode)
        {
            LobbyCamMove();
            if (mTwRotation.enabled == false)
            {

                SelectMachine();

            }

            if (bPicking == true)
            {
                UpdateCamPos();
            }
        }
        else if (mLobbyState == LobbyState.GuideMode)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Currentpage++;

                if (Currentpage > 5) return;

                foreach (GameObject gb in GuideSprite)
                    gb.SetActive(false);


                GuideSprite[Currentpage].SetActive(true);

            }

            //코인 페이지
            if (Currentpage == 6)
            {
                //mLobbyState = LobbyState.CoinMode;
                //GameClient.instance.mbTutorial = true;
                //GameClient.instance.SaveUserOption();


                GameClient.mGameState = GameState.Mystery;
                GameClient.instance.LoadingScene();
                GameClient.instance.StopBGM();

                //GameObject.Find("SceneController").GetComponent<SceneController>().GoMysteryGame();
                //CoinPageObj.SetActive(true);
            }
        }

    }


    void LateUpdate()
    {



    }

    // 9.508787


    public void UpdateCamPos()
    {
        /*if (GameClient.instance.tutorial.tutDialog != TutorialDialog.G) return;*/
        //RuntimePlatform.WindowsPlayer
        Vector3 curPos = transform.position;

        Vector3 currentpos = Vector3.zero;
        Vector3 newPos = Vector3.zero;

        if (LoadingBar.GetInstance().GetLock() == true) return;

        if (GameClient.instance.isVisitor == true && LobbyMgr.Instance.tutoLobby.tutDialog != TutorialDialog.H) return;


        if (LobbyMgr.Instance.tutoLobby.tutDialog == TutorialDialog.J) return;
        currentpos.x = Mathf.LerpAngle(curPos.x, mTargetPos.x, m_Damping * Time.deltaTime);
        currentpos.y = Mathf.LerpAngle(curPos.y, mTargetPos.y, m_Damping * Time.deltaTime);
        currentpos.z = Mathf.LerpAngle(curPos.z, mTargetPos.z, m_Damping * Time.deltaTime);

        transform.position = currentpos;

        bUpdateCam = true;
        float dist = Vector3.Distance(transform.position, mTargetPos);

        if (dist <= 2.0f)
        {

            bPicking = false;

            if (GameClient.instance.isVisitor == false)
            {

                GameClient.mGameState = GameState.Mystery;
                GameClient.instance.LoadingScene();
                GameClient.instance.StopBGM();
            }
            else
            {
                LobbyMgr.Instance.tutoLobby.isClearMission = true;
                LobbyMgr.Instance.tutoLobby.tutDialog++;
                LobbyMgr.Instance.tutoLobby.TutDialog = LobbyMgr.Instance.tutoLobby.tutDialog;
            }

            /*

            if (GameClient.instance.mbTutorial == true)
            {
                GameClient.mGameState = GameState.Mystery;
                GameClient.instance.LoadingScene();
                GameClient.instance.StopBGM();
            }
            else 
            {
                mLobbyState = LobbyState.GuideMode;
                GuideSprite[0].SetActive(true);
            }
            */




            //GameObject.Find("SceneController").GetComponent<SceneController>().GoMysteryGame();
        }

        Vector3 dir = mTargetPos - mTrans.position;
        float mag = dir.magnitude;

        if (mag > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            mTrans.rotation = Quaternion.Slerp(mTrans.rotation, lookRot, Mathf.Clamp01(speed * Time.deltaTime));
        }
    }

    bool CheckScreenTouch()
    {
        if (GameClient.instance.IsPause == true)
            return false;

        return Input.GetMouseButton(0);
    }


    void LobbyCamMove()
    {

        if (bUpdateCam == true || bPicking == true)
        {
            return;
        }

        //Debug.LogError("LobbyCamMove");
        Vector3 pos = Input.mousePosition;

        if (GameClient.instance.IsPause == true)
            return;

        if (Input.GetMouseButtonDown(0) == true)
        {
            fTouchMove = pos.x;
            bTouch = true;
        }
        else if (Input.GetMouseButton(0))
        {

            if (GameClient.instance.isVisitor == true) return;

            if (Mathf.Abs(fTouchMove - pos.x) > 50 && bTouch == true)
            {
                bTouch = false;


                if (pos.x > fTouchMove)
                {
                    if (MachineIndex == 0)
                    {
                        CameraMove = emCameraMove.LeftSkip;
                    }
                    else
                    {
                        CameraMove = emCameraMove.Left;
                    }

                    //CurCamera.transform.Rotate( Vector3.up * -1.0f );


                }
                else if (pos.x < fTouchMove)
                {
                    if (MachineIndex == 8)
                    {
                        CameraMove = emCameraMove.RightSkip;
                    }
                    else
                    {
                        CameraMove = emCameraMove.Right;
                    }



                    //CurCamera.transform.Rotate( Vector3.up * 1.0f );					
                }
                else
                {

                    CameraMove = emCameraMove.None;
                }



                //Debug.Log(string.Format("{0} ,{1}", CurCamera.transform.localEulerAngles.y ));
            }
        }
        else
        {
            fTouchMove = pos.x;
            CameraMove = emCameraMove.None;
        }

        /*
        else
        {
            CameraMove = emCameraMove.Right;
        }
        */


        //29.8
        /*
        if (mCameraMove == emCameraMove.Right)
        {
            
            CurCamera.transform.RotateAroundLocal(Vector3.up, -1.4f * Time.deltaTime);        
        }
        else if (mCameraMove == emCameraMove.Left)
        {
            CurCamera.transform.RotateAroundLocal(Vector3.up, 1.4f * Time.deltaTime);
        }
        */


    }

    public void SelectMachine()
    {


        if (GameClient.instance.IsPause == true)
            return;

        //         if (GameClient.instance.isVisitor == true && bPicking == false)
        //         {
        //             Ray ray = CurCamera.ScreenPointToRay(Input.mousePosition);
        //             RaycastHit hit;
        // 
        //             
        // 
        //             if (Physics.Raycast(ray, out hit, 200))
        //             {
        //                 
        //                 if (GameClient.instance.isVisitor == true && GameClient.instance.tutorial.tutDialog == TutorialDialog.H)
        //                 {
        //                     if (GameClient.instance.tutorial.isClearMission == false)
        //                     {
        //                         GameClient.OneShotSound(new Vector3(0, 0, 0), GameClient.instance.mSnd_Zoomin);
        //                     } 
        //                     
        //                     Debug.DrawLine(ray.origin, hit.point, Color.red);
        // 
        //                     Transform obj = hit.collider.transform.Find("LookCamPos");
        // 
        // 
        //                     if (obj)
        //                     {
        //                         Debug.Log("Cam Pick " + obj.position);
        //                         mTargetPos = obj.position;
        // 
        //                     }
        // 
        //                     bPicking = true;
        // 
        //                     
        //                     this.GetComponent<TweenRotation>().enabled = false;
        //                 }
        // 
        // 
        //                 
        // 
        // 
        // 
        // 
        //                 //float dist = Vector3.Distance(transform.position, mTargetPos);
        //                 //Debug.Log("dist " + dist);
        // 
        // 
        //                 //pos.y = hit.collider.transform.parent.position.y; 
        // 
        //             }
        //         }

        if (GameClient.instance.isVisitor == true && LobbyMgr.Instance.tutoLobby.tutDialog != TutorialDialog.H) return;
        if (LobbyMgr.Instance.tutoLobby.tutDialog == TutorialDialog.J) return;

        if (Input.GetMouseButtonUp(0) && bPicking == false)
        {
            Ray ray = CurCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200))
            {
                GameClient.OneShotSound(new Vector3(0, 0, 0), GameClient.instance.mSnd_Zoomin);

                Debug.DrawLine(ray.origin, hit.point, Color.red);

                Transform obj = hit.collider.transform.Find("LookCamPos");


                if (obj)
                {
                    Debug.Log("Cam Pick " + obj.position);
                    mTargetPos = obj.position;

                }
                //Debug.LogError("SelectMachine / "+mTargetPos);
                bPicking = true;


                this.GetComponent<TweenRotation>().enabled = false;



                //float dist = Vector3.Distance(transform.position, mTargetPos);
                //Debug.Log("dist " + dist);


                //pos.y = hit.collider.transform.parent.position.y; 

            }
        }

    }


    void OnDestory()
    {
        GuideSprite = null;
    }

}
