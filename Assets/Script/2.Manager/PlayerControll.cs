using UnityEngine;
using System.Collections;

class cBlockInfoPos
{
	public Vector3 LeftPos;
	public Vector3 CenterPos;
	public Vector3 RightPos;
		
	public void Init()
	{
		LeftPos = Vector3.zero;
		CenterPos = Vector3.zero;
		RightPos = Vector3.zero; 
	}
};

enum DragDirection
{
    LEFT, RIGHT, NONE
}


public class PlayerControll : MonoBehaviour {
	
	public float fTestHeight =1.2f;
    float fDragTime = 0;
	int	blockCount =1; 
	cBlockInfoPos mBlockInfo = new cBlockInfoPos();
	Vector3 VecTouchMove = Vector3.zero;
	Vector3 VecMoveDelta = Vector3.zero;

    DragDirection eDrag = DragDirection.NONE;
	
	public float fMoveDeltaX = 0.05f; 
	
	
	float fBarMove = 0;
	Transform CameraTrans = null; 
	bool bOneRun = false;

    Vector3 FromBarPos = Vector3.zero;
    

	// Use this for initialization
    void Awake()
    {
        FromBarPos = transform.position;
        CameraTrans = transform.parent.Find("MCamera");
    }
    

	void Start () {
        Init();
	}

    public void Init()
    {
        mBlockInfo.Init();
        fBarMove = 0;
        transform.position = FromBarPos;
        transform.rotation = Quaternion.identity;

        //Debug.Log(mDefaultBar.position + " " + mDefaultBar.rotation);
        
        //Debug.Log("PlayerControll Init");

    }

	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("OnTriggerEnter");		
		//col.transform.position = transform.position; 
	}

    
// 
//     IEnumerator DragMove()
//     {
//         bool bLoop = true;
//         
//         while (bLoop)
//         {
//             if (fDragTime > 0.1f)
//             {
//                 fDragTime -= Time.deltaTime * 200;
//             }
//             else if (fDragTime < -0.1f)
//             {
//                 fDragTime += Time.deltaTime * 200;
//             }
//             else
//             {
//                 fDragTime = 0;
//                 bLoop = false;
//             }
//             VecMoveDelta.x = fDragTime;
//         }
// 
//         yield return null;
//         
//     }
//   
    //0.1810583 ,  26.48573 -8.360594
    // Update is called once per frame
    float fdragdist = 0;
    float eDragVecMove()
    {
        
        float fMaxDrag = 1;
        switch (eDrag)
        {
            case  DragDirection.RIGHT:
                fdragdist += Time.deltaTime * 3;
                if (fdragdist > fMaxDrag) fdragdist = fMaxDrag;
                break;
            case DragDirection.LEFT:
                fdragdist -= Time.deltaTime * 3;
                if (fdragdist < -fMaxDrag) fdragdist = -fMaxDrag;
                break;
            case DragDirection.NONE:
                if (fdragdist > 0.1f) fdragdist -= Time.deltaTime * 0.2f;
                else if (fdragdist < -0.1f) fdragdist += Time.deltaTime * 0.2f;
                else fdragdist = 0;
                
                break;
        }
        return fdragdist;
    }

	void Update () 
	{
		//float fMove = Input.GetAxis("Horizontal") * 0.04f;        
		//float UpDownMove = Input.GetAxis("Vertical") * 0.04f;    

        if (GameClient.instance.IsPause == true)
            return;

        if (GameClient.instance.mMysteryState != MysteryState.InGame) return;


        if (GameClient.instance.isVisitor == true) return;

		VecMoveDelta = Vector3.zero; 
		
		float dist = Vector3.Distance(CameraTrans.position ,transform.position) * 0.15f;

        if (GameClient.instance.mbTilt == true)
        {
            //float fDrag = 0;
            if (Input.acceleration.x > 0.1f)
            {
                eDrag = DragDirection.RIGHT;
                fDragTime = eDragVecMove();
                VecMoveDelta.x = 1;
            }


            else if (Input.acceleration.x < -0.1f)
            {
                //fDrag += Input.acceleration.x * 3f;
                eDrag = DragDirection.LEFT;
                fDragTime = eDragVecMove();
                VecMoveDelta.x = -1;
            }

            else
            {
                eDrag = DragDirection.NONE;
                fDragTime = eDragVecMove();
                VecMoveDelta.x = fDragTime;
            }
                
                //VecMoveDelta.x = fDrag;

        }
        else
        {

            if (Input.GetKey(KeyCode.A))
            {
                eDrag = DragDirection.LEFT;
                fDragTime = eDragVecMove();
                VecMoveDelta.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                eDrag = DragDirection.RIGHT;
                fDragTime = eDragVecMove();
                VecMoveDelta.x = 1;
                
            }
            else
            {
                eDrag = DragDirection.NONE;
                fDragTime = eDragVecMove();
                VecMoveDelta.x = fDragTime;

                //VecMoveDelta.x = fDrag;
                //if (fDragTime > 0) fDrag -= Time.deltaTime * 200;
                //else fDragTime += Time.deltaTime * 200;
            }

            

            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Input.mousePosition;
                
                if (VecTouchMove.x != pos.x)
                {
                    if (pos.x > VecTouchMove.x)
                    {
                        //fMoveDelta = pos.x - fTouchMove; 
                        VecMoveDelta.x = 1;
                    }
                    else if (pos.x < VecTouchMove.x)
                    {
                        //fMoveDelta = pos.x - fTouchMove; 
                        VecMoveDelta.x = -1;
                    }
                    else
                    {
                        VecMoveDelta.x = 0.0f;
                    }

                    VecTouchMove.x = pos.x;
                }
            }
        }
		
        if ( Input.GetMouseButton(0) && GameClient.instance.mMysteryState == MysteryState.InGame  )
        {
			//fBarMove = 0 ; 
			Vector3 pos = Input.mousePosition;        
			
			//1:1매칭 16:9
			if( Input.mousePosition.x > Screen.width  || Input.mousePosition.x < 0  ) 
			{
				return; 
			}
			
			if( bOneRun == false ) 
			{
				VecTouchMove = pos;
				bOneRun= true; 
			}
	
			//float rx = pos.x - (Screen.width/2);
			//fMove = (rx/(Screen.width/2))*1.43f;
			//fBarMove = (rx/(Screen.width/2))*(dist * 0.226f);
			
			//Debug.Log( Input.mousePosition.x +  "  "+ Input.mousePosition.x/Screen.width);
            if (VecTouchMove.y != pos.y)
            {
                if (pos.y > VecTouchMove.y)
                {
                    //VecMoveDelta.y = pos.y - VecTouchMove.y; 
                    //VecMoveDelta.y = 1;
                    if (GameClient.instance.isVisitor == false)
                    {
                        VecMoveDelta.y = 1;
                    }
                                       
                }
                else if (pos.y < VecTouchMove.y)
                {
                    //VecMoveDelta.y = pos.y - VecTouchMove.y; 
                    //VecMoveDelta.y = -1;
                    if (GameClient.instance.isVisitor == false)
                    {
                        VecMoveDelta.y = -1;
                                       
                    }
                }
                else
                {
                    VecMoveDelta.y = 0.0f;
                }
                if (GameClient.instance.isVisitor == false)
                {
                    VecTouchMove.y = pos.y;
                }
                
            }

            /*
            if (GameClient.instance.mbTilt == false)
            {
                if (VecTouchMove.x != pos.x)
                {
                    if (pos.x > VecTouchMove.x)
                    {
                        //fMoveDelta = pos.x - fTouchMove; 
                        VecMoveDelta.x = 1;
                    }
                    else if (pos.x < VecTouchMove.x)
                    {
                        //fMoveDelta = pos.x - fTouchMove; 
                        VecMoveDelta.x = -1;
                    }
                    else
                    {
                        VecMoveDelta.x = 0.0f;
                    }

                    VecTouchMove.x = pos.x;
                }
            }*/
           // MysteryMgr.Instance.UpdateBlock((VecMoveDelta.x * 0.03f) * dist);
            
        }

            
        //자이로센서
        
        
	
	    //Debug.Log("fMove " + fMoveDelta);

        
        Vector3 newpos = transform.position;
        Vector3 OldPos = transform.position; 


		float fMovedelta = 0.04f; 

#if UNITY_ANDROID
        fMovedelta = 0.03f;
#endif
			newpos.x += ((VecMoveDelta.x * fMovedelta) * dist);

		transform.position = newpos;

        Vector3 ScreenPos = CameraTrans.GetComponent<Camera>().WorldToScreenPoint(transform.position);


        //화면 양사이드로 빠질때
        if ((ScreenPos.x < 0) || (ScreenPos.x > Screen.width))
        {
            transform.position = OldPos;
        }
        else 
        {
			MysteryMgr.Instance.UpdateBlock((VecMoveDelta.x * fMovedelta) * dist);
        }
        
        
        if (GameClient.instance.mbTilt)
        {
           // transform.Rotate(transform.forward * (VecMoveDelta.y * 0.2f));
        }
        else
        {
            transform.Rotate(transform.forward * (VecMoveDelta.y * 0.4f));
        }
		
		Vector3 eulerAngle = transform.eulerAngles; 
				
		
		if( eulerAngle.z > 20 && transform.up.x < 0 )
		{
			eulerAngle.z = 20 ; 
		}
		else if(eulerAngle.z < 340   && transform.up.x > 0  )
		{
			eulerAngle.z = 340; 
		}
		
		transform.eulerAngles = eulerAngle; 
		
		//Debug.Log( transform.eulerAngles.z + "  " + UpDownMove + " " + transform.up  );
		
				
		//바구니 위에 올려있는 블럭들을 갱신한다.. 
        //CollBox(fMove);
	}

    void UpdateTouchMove(ref Vector3 vec)
    { 
        
    
    
    }


    void CollBox(float fMove)
    {

        //Debug.DrawLine(transform.position , (transform.forward)*10 );
				
        mBlockInfo.LeftPos = transform.position + (Vector3.left * 0.8f);
        mBlockInfo.CenterPos = transform.position;
        mBlockInfo.RightPos = transform.position + (Vector3.up * fTestHeight );

        //Debug.DrawRay(mBlockInfo.LeftPos, transform.up * fTestHeight);
        Debug.DrawRay(mBlockInfo.CenterPos, transform.up * fTestHeight);
        //Debug.DrawRay(mBlockInfo.RightPos, transform.up * fTestHeight);

        //Debug.DrawLine(mBlockInfo.CenterPos, mBlockInfo.RightPos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(mBlockInfo.CenterPos, transform.up, fTestHeight);
        //hits = Physics.CapsuleCastAll(mBlockInfo.CenterPos, mBlockInfo.RightPos, 1, transform.up, fTestHeight );
		
		
        

        Vector3 PrevMove = Vector3.zero;
        int i = 0;

        if (hits.Length > 0)
        {
           // Debug.Log(hits.Length); 
            fTestHeight = hits.Length * 1.32f;
            blockCount = hits.Length;

            MysteryMgr.Instance.mBlockStack = hits.Length; 
        }
        else
        {
            fTestHeight = 1.2f;
        }

        while (i < hits.Length)
        {
						
            RaycastHit hit = hits[i];
						
            if (hit.collider.gameObject != gameObject)
            {
                PrevMove = hit.collider.transform.position;
                PrevMove.x += fMove*0.7f ;
                hit.collider.transform.position = PrevMove;
            }

            Transform gameobj =  hit.collider.transform.Find("Model_Block01");

            if (gameobj != null)
            {
                Renderer renderer = gameobj.GetComponent<Renderer>(); 

                if (renderer)
                {
                    renderer.material.shader = Shader.Find("Transparent/Diffuse");
                    renderer.material.color = Color.blue;
                }
            }
            
            i++;
        }



    }

	
	
	void Destory()
	{
		mBlockInfo = null ; 
	}
	
}
