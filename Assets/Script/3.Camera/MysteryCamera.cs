using UnityEngine;
using System.Collections;

public class MysteryCamera : MonoBehaviour {
	
	
	float mDistance = 0.0f;
    float fMaxDistance = 0.0f;  
    
    float Height = 0.0f;
    float mPrevDistance = 0.0f; 

	Vector3 PrevCamrePos;
    int iMaxCount = 3;
    
	bool bReady = false  ;
    public GameObject WinZoneBox;
    public bool bCameraMove = false; 

    
    public float ReverseDuration = 3.0f ; 
    float ReverseTime;
    
    
    		
	/*
	void OnDrawGizmosSelected()
	{  
		Vector3 pos = Input.mousePosition;
		
		Vector3 p = camera.ScreenToWorldPoint(new Vector3(pos.x , pos.y, camera.nearClipPlane));  
		Gizmos.color = Color.yellow; 
		Gizmos.DrawSphere(p, 0.1F);  
		
	}
	*/
	// Use this for initialization
	void Start () {
        
        transform.position = new Vector3(0, 6.73f , 3.4f);
        PrevCamrePos = transform.position; 
        
        if (WinZoneBox == null)
            Debug.Log("CollisionBox not exist ");
	}

    public float fRate = 4.5f;


    RaycastHit hit;

	void Update()	
	{		 
		//Init();
        UpdateWinzoneBar();
        
	}
	
	public void Init()
	{
        fMaxDistance =0; 
	}
    
    void FixedUpdate()
    {
        
        if (GameClient.instance.mMysteryState == MysteryState.Stay )
        {
            ReverseCamera();
        }
        else 
        {
            CameraMove();
        }
        
        //CameraMoveTest();

    }

    void UpdateWinzoneBar()
    {
        Vector3 RandPos = new Vector3(Screen.width >> 1, Screen.height - (Screen.height / fRate), 0);

        Ray ray = camera.ScreenPointToRay(RandPos);
        
        if (Physics.Raycast(ray, out hit, 3000))
        {
            //  if (hit.transform.tag == "WinZoneWall")

            Debug.DrawLine(ray.origin, hit.point, Color.red);

            Vector3 pos = WinZoneBox.transform.position;

            pos.y = hit.point.y;
            pos.z = 10.14f;     //z축은 고정으로한다. 


            WinZoneBox.transform.position = pos;
        }
    }


    public void SetReverseCamera()
    {
        mPrevDistance = mDistance;
        ReverseTime = 0.0f; 

        bCameraMove = true; 
    }


    void CameraMove()
    {
        
        if (MysteryMgr.Instance.mBlockStep == 1)
        {
            fMaxDistance = 5.74f;
        }
        else if (MysteryMgr.Instance.mBlockStep == 2)
        {
            fMaxDistance = 10.8f;
        }
        else if (MysteryMgr.Instance.mBlockStep == 3)
        {
            fMaxDistance = 12.68f;
        }
        if (mDistance <= fMaxDistance)
        {
            mDistance += Time.deltaTime * 2.5f;
        }
        
        //Debug.Log(   mDistance + " " + MysteryMgr.Instance.mBlockCount );

        Height = mDistance * 0.4595f;

        Vector3 targetPos = PrevCamrePos + Vector3.up * (Height);
        targetPos.z -= mDistance;

        transform.position -= (transform.position - targetPos);


    }


    
    void ReverseCamera()
    {
        if (mDistance <= 0.0f)
        {
            bCameraMove = false; 
            return;
        }
        
        ReverseTime += Time.deltaTime;
        float t = ReverseTime  / ReverseDuration;
        
        mDistance = Mathf.Lerp(mPrevDistance, 0.0f, t );
               
        //Debug.Log("Distance  " + mDistance);


        Height = mDistance * 0.4595f;
        
        Vector3 targetPos = PrevCamrePos + Vector3.up * (Height);
        targetPos.z -= mDistance;
        
        transform.position -= (transform.position - targetPos);

        if( t > 1.0f )
        {
            bCameraMove = false; 
        }


    }    




    void CameraMoveTest()
    {
        
        //Debug.Log(   mDistance + " " + MysteryMgr.Instance.mBlockCount );
        
        Height = fMaxDistance * 0.4595f;

        Vector3 targetPos = PrevCamrePos + Vector3.up * (Height);
        targetPos.z -= fMaxDistance;

        transform.position -= (transform.position - targetPos);
    
    }

}
