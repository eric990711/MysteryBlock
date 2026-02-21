using UnityEngine;
using System.Collections;

public enum emBlockState
{
    Ready,      //배경블록에서 빠져나온다. 
    Stop,       //빠져나온블럭은 일시정지
    Drop,       //떨어진다.
    CollEnter,  //블럭끼리 충돌
    CollStay,
    CollExit,
};

public enum _BlockType
{
    Normal,
    SpeedDown,      // 다음블록 하강 스피를 줄여줌
    DoubleTicket,   // 현재 스텝 티켓 수량을 두배로
    DoubleJackpot,  // 잿팟 수량을 두배를 
    QuestMark,      // ????? 
    FocusBox,
}

public class BaseBlock : MonoBehaviour
{

	[HideInInspector]
    public Transform BGBlock = null;
    
    [HideInInspector]
    public bool bDown = false;
    
    
    public bool bCollision = false;

    public _BlockType mType = _BlockType.Normal;
    public _BlockType mPrevType = _BlockType.Normal; //판정후 흰색으로 표시;



    public emBlockState BlockState = emBlockState.Ready;
    
    public float ItemUseTime = 3f; 
    public float fItemTime = 0.0f; 
	public float fStayTime = 0.0f;
    
    GameObject Model = null;
    public bool bCheckStayBlock = false;


    bool bOneRun = false; 


    public Material[] mat;

    Vector3 pos = Vector3.zero;

    int Num = 0;

    void CMSData(int cms)
    {
        float from = 22;
        float to = 0;
        int CMS = cms;
        int MAX_CMS = 1000;

        float result = Mathf.Lerp(from, to, (float)CMS / (float)MAX_CMS);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (GameClient.instance.isVisitor == false)
        {
            rb.drag = result;
        }
        else
        {
            rb.drag = 10;
        }

    }

    void Awake()
    {
        bOneRun = false; 
        
        Model = transform.Find("Model_Block01").gameObject;
        pos = transform.position;
        fStayTime = 0.0f;

        if (MysteryMgr.Instance.BlockArrangement[MysteryMgr.Instance.ArrBlockNum] == false) Num = 0;
        else Num = Random.Range(1, 4);
        //else Num = Random.Range(1, 4);
        //else Num = 3;
        //else Num = 3;
        switch (Num)
        {
            case 1: mType = _BlockType.SpeedDown; MysteryMgr.Instance.isMysteryEff = true; break;
            case 2: mType = _BlockType.DoubleTicket; MysteryMgr.Instance.isMysteryEff = true; break;
            case 3: mType = _BlockType.DoubleJackpot; MysteryMgr.Instance.isMysteryEff = true; break;
        }
        CMSData(GameClient.instance.GetMachineData().theLevelValue);
          
        //MysteryMgr.Instance.mGetItem[(int)ItemType.SpeedDown] = true; 


        //현재 11  = cms 500 
        //0~22 
        //난이도 값을 적용시킨다. 
        //GameClient.instance.GetMachineData().theLevelValue //1~1000 cms;
        
        

        Rigidbody rb = GetComponent<Rigidbody>();
        if (MysteryMgr.Instance.mUseCoinx2 == true)
        {
            //중력은 높을수록 물체가 느려짐;
            float t = rb.drag - (rb.drag * 0.8f);
            rb.drag = rb.drag - t;

            Debug.Log(rb.drag);
        }

        //Set Speed Item 
        if (MysteryMgr.Instance.mGetItem[(int)ItemType.SpeedDown] == true && MysteryMgr.Instance.mItemUse == false)
        {

            float t = rb.drag - (rb.drag * 0.5f);

            rb.drag = rb.drag + t;

            MysteryMgr.Instance.mItemUse = true;


			MysteryMgr.Instance.mSlowBlcok = this ; 
        }
        
        if (MysteryMgr.Instance.mDropItem == false)
        {
            mType = (_BlockType)Num;

            MysteryMgr.Instance.mDropItem = true; 

        }
        
        //is Mystery block 
        if (mType != _BlockType.Normal)
        {
            //Model.GetComponent<Renderer>().material = mat[(int)mType]; 
            Model.GetComponent<Renderer>().material = mat[(int)_BlockType.QuestMark]; 
        }

    }

    void Start()
    {

    }

	// Update is called once per frame
    void FixedUpdate()
    {
      
        //광선에 맞고 그자리에서 나오는 애니메이션;
        if (BlockState == emBlockState.Ready)
        {
            if (GameClient.instance.isVisitor == true)
            {
                BlockState = emBlockState.Stop;
            }
            else
            {
                if (BGBlock != null && bOneRun == false)
                {
                    bOneRun = true;
                    pos.x = BGBlock.position.x;
                    pos.y = BGBlock.position.y;
                }


                if (pos.z > 10.2156f)
                {
                    pos.z -= Time.deltaTime * 4f;
                }
                else
                    BlockState = emBlockState.Stop;

                transform.position = pos;
            }
		}
        else if( BlockState == emBlockState.Stop )
		{
			BlockState = emBlockState.Drop; 
			rigidbody.useGravity = true; 
			rigidbody.isKinematic = false; 

			if(MysteryMgr.Instance.mMissionSuccess == true)
			{
				DestoryBlock();
			}

		}
        else if( BlockState == emBlockState.CollEnter )
        {
            fStayTime = 0.0f;
            BlockState = emBlockState.CollStay;
        }
        else if (BlockState == emBlockState.CollStay)
        {
            fStayTime += Time.fixedDeltaTime;

            //float checkTime = 0.5f;
            float checkTime = 0.5f;
            /*
            if (MysteryMgr.Instance.mBlockStep > 2)
                checkTime = 0.3f;
            */


            if (fStayTime > checkTime && bCheckStayBlock == false)
            {
                mPrevType = mType;
                
                bCheckStayBlock = true; 
                StartCoroutine(ChangeMaterial());

                if (mType == _BlockType.SpeedDown)
                {
                    MysteryMgr.Instance.ApplyItem(_BlockType.SpeedDown);
                }
                else if (mType == _BlockType.DoubleTicket)
                {
                    MysteryMgr.Instance.ApplyItem(_BlockType.DoubleTicket);
                }
                else if (mType == _BlockType.DoubleJackpot)
                {
                    MysteryMgr.Instance.ApplyItem(_BlockType.DoubleJackpot);
                }

				if(GameClient.instance.isVisitor == true){
					Rigidbody rbVis = GetComponent<Rigidbody>();
					rbVis.useGravity = false;
					rbVis.isKinematic = true; 
				}

            }
        }
	}
    

	void OnCollisionEnter(Collision col)
	{
        //mBlockList;
        
        if( col.gameObject.tag == "BlockBox" || col.gameObject.tag == "Player")
        {
            MysteryMgr.Instance.AddObject(gameObject);
            if (BlockState == emBlockState.Drop)
            {
                
                //NGUITools.PlaySound(GameClient.instance.mSnd_BlockDrop);
                GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_BlockDrop);
                StartCoroutine(GameClient.instance.VibrateController(1, 1));
                BlockState = emBlockState.CollEnter;
            }
            else if (BlockState == emBlockState.CollExit)
            {
                GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_BlockDrop);
                //NGUITools.PlaySound(GameClient.instance.mSnd_BlockDrop);
                BlockState = emBlockState.CollEnter;

            }
        }


		/*		
		AudioSource source = gameobject.AddComponent<AudioSource>();
		source.clip = m_AirplanBoomClip;
        source.volume = m_EffectVolume;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.Play();
		*/
	//	Debug.Log("OnCollisionEnter ");
        


       
	}
	
	/*
	void OnCollisionStay(Collision col)
	{
        
        //제자리에서 일정시간 이상이면 성공으로 판단;
        
        if (fStayTime > 0.5f)
        {
            BlockState = emBlockState.CollStay;
            MysteryMgr.Instance.mItemUse = true;
            mPrevType = mType;

            StartCoroutine(ChangeMaterial());
        }
        else 
        {
            fStayTime += Time.deltaTime;
        }

        

        Debug.Log("OnCollisionStay  ~~~ ");

	}*/

    public IEnumerator ItemEffect()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            Model.GetComponent<Renderer>().material = mat[(int)_BlockType.FocusBox];
            yield return new WaitForSeconds(0.1f);
            Model.GetComponent<Renderer>().material = mat[(int)mPrevType];
            yield return new WaitForSeconds(1f);
        }
    }


    public IEnumerator ChangeMaterial()
    {
        Model.GetComponent<Renderer>().material = mat[(int)_BlockType.FocusBox];
        yield return new WaitForSeconds(0.1f);
        Model.GetComponent<Renderer>().material = mat[(int)mPrevType];

        GameObject obj = null;
        
        switch (mPrevType)
        {
            case _BlockType.DoubleJackpot: obj = GameClient.instance.mEffDoubleJackpot; break;
            case _BlockType.DoubleTicket: obj = GameClient.instance.mEffX2; break;
            case _BlockType.SpeedDown: obj = GameClient.instance.mEffSlowdown; break; 
        }

        if( (int)mPrevType  > (int)_BlockType.Normal && (int)mPrevType  < (int)_BlockType.QuestMark ) 
        {
            StartCoroutine(ItemEffect());                 
        }
        

        Vector3 pos = transform.position;
        pos.z -= 1.0f;
        

        //Mblock Bomb
        
        if (obj != null)
        {
            GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_MBlockBomb);
            Instantiate(obj, pos, Quaternion.identity);
        }
        
    }

	//바닥에서 완전 벗어나지 않으면 .. 
	void OnCollisionExit(Collision col)
	{
		//MysteryMgr.Instance.DeleteObject(gameObject);		
        if (BlockState == emBlockState.CollStay)
        {
            BlockState = emBlockState.CollExit;
        }
                
       //  Debug.Log("OnCollisionExit  ~~~ ");
    }

    void OnDestory()
    {
        Debug.Log("On DestoryBlock");
    }
        
    public bool CheckState(emBlockState state)
    {
        return (BlockState == state)  ;
    }

    public void DestoryBlock()
    {
        Debug.Log("DestoryBlock");
        Destroy(gameObject);
        mat = null; 
    }
    
    

}
