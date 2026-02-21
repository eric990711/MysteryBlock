using UnityEngine;
using System.Collections;

public enum LightMode
{
	Stop = 0 , 

	Up ,
	Down, 
	Stack,
    ZigZag,
    ZigZagReverse, 
    Twinkle, 
}
//	Snake,


public class EffSliderbar : MonoBehaviour {
	
	
	public GameObject[] mLSliderList;	//조명리스트

	public GameObject[] mRSliderList;	//조명리스트	
	
	public LightMode mMode = LightMode.Stop; 
		
	float NowTime = 0.0f; 
	float ChangeTime =0.0f;
    float SubTime = 0.0f; 

	int iCount = 0; 
	int iMaxCount;
    int iAllMaxCount = 16; 
	bool bShow = true;
    bool bShowTime = false;
    bool bLeftShow = false;
    bool bRightShow = false; 
 


    float mDuration = 10.0f;
    int iSideLCount = 0;
    int iSideRCount = 0; 

	void Start () {
		Reset();
	}
	
	void Reset()
	{
		bShow = false; 
		iMaxCount = 8; 
		NowTime  = 0 ;
        SubTime = 0; 

        bLeftShow = false;
        bRightShow = false; 
 

		for(int i =0 ; i <= iMaxCount; i++)
		{
			mLSliderList[i].GetComponent<TweenAlpha>().enabled = false;
			mLSliderList[i].SetActive(bShow);
			
			mRSliderList[i].GetComponent<TweenAlpha>().enabled = false;
			mRSliderList[i].SetActive(bShow);
			
		}

		if( mMode == LightMode.Down )
		{
			NowTime  = 0 ; 
			iCount = 0 ; 
		}
		else if( mMode == LightMode.Up )
		{
			NowTime  = 0 ; 
			iCount = iMaxCount ; 
		}
		else if (mMode == LightMode.Stack)
		{
			iCount = 0 ; 
		}
        else if (mMode == LightMode.ZigZag)
        {
            bLeftShow = true;
            bRightShow = true;
            iSideRCount = 0;
            iSideLCount = 0;
            bShow = true; 

        }else if (mMode == LightMode.ZigZagReverse)
        {
            bLeftShow = true;
            bRightShow = true;
            iSideRCount = 8;
            iSideLCount = 8;
            bShow = true; 
        }
        



		
	}
    
    // LightMode 타입 설정 , 시간설정
    public void Play(LightMode _mode, float t)
    {
        Debug.Log("Light Mode Play ");

        Reset();
        mMode = _mode;
        bShowTime = true;
        ChangeTime = 0.0f;
        mDuration = t;


    }



	// Update is called once per frame
	void Update () {
		NowTime += Time.deltaTime; 
		ChangeTime += Time.deltaTime;
        SubTime += Time.deltaTime;
        
        if (ChangeTime > mDuration )	
		{
			ChangeTime = 0 ; 
			Reset();

            if (bShowTime == true)
            {
                mDuration = 4.0f;
                bShowTime = false; 
            }
            
            mMode = (LightMode)Random.Range((int)LightMode.Up, (int)LightMode.ZigZagReverse);
		}
        		
		if( mMode == LightMode.Down ) 
		{

			if( iCount > iMaxCount) 
			{
				bShow ^=true; 
				iCount  =0; 
				return; 
			}
						
			if( NowTime > 0.1f ) 
			{
				NowTime = 0.0f; 
				mLSliderList[iCount].SetActive(bShow);
				mRSliderList[iCount].SetActive(bShow);
				iCount++;
			}
		}
		else if( mMode == LightMode.Up ) 
		{
			if( NowTime > 0.1f ) 
			{
				NowTime	= 0 ; 
				
				mLSliderList[iCount].SetActive(bShow);
				mRSliderList[iCount].SetActive(bShow);
								
				iCount--;
			}
			
			if( iCount < 0) 
			{
				bShow ^=true; 
				iCount  = iMaxCount; 
			}
			
			
		}
        else if (mMode == LightMode.ZigZag)
        {
            if (NowTime > 0.2f )
            {
                NowTime = 0;

                if (bLeftShow)
                {
                    mLSliderList[iSideLCount].SetActive(bShow);
                    iSideLCount++;
                }
                else
                {
                    
                    mRSliderList[iSideRCount].SetActive(bShow);
                    iSideRCount++;
                }

                bLeftShow ^= true; 

            }
         
            if (iSideRCount > iMaxCount)
            {
                bShow ^= true; 
                iSideRCount = 0;
                iSideLCount = 0;
            }
        }
        else if (mMode == LightMode.ZigZagReverse)
        {
            
            if (NowTime > 0.2f)
            {
                NowTime = 0;

                if (bRightShow)
                {
                    mRSliderList[iSideRCount].SetActive(bShow);
                    iSideRCount--;
                    
                }
                else
                {
                    mLSliderList[iSideLCount].SetActive(bShow);
                    iSideLCount--;
                  
                }

                bRightShow ^= true;

            }

            if (iSideLCount < 0 )
            {
                bShow ^= true;
                iSideRCount = 8;
                iSideLCount = 8;
            }


        }
        else if (mMode == LightMode.Stack)
        {
            if (NowTime > 0.05f)
            {
                NowTime = 0.0f;

                mLSliderList[iCount].SetActive(true);
                mRSliderList[iCount].SetActive(true);

                if (iCount > 0)
                {
                    mLSliderList[iCount - 1].SetActive(false);
                    mRSliderList[iCount - 1].SetActive(false);
                }


                //for(int i =0 ; i <= iMaxCount; i++)

                //mRSliderList[i].SetActive(bShow);

                iCount++;

                if (iCount > iMaxCount)
                {
                    iCount = 0;
                    iMaxCount--;
                    //Debug.Log("MaxCount" + iMaxCount + " Count " + iCount) ;				

                    if (iMaxCount < 0)
                    {
                        Reset();
                    }
                }
            }
        }
        else if (mMode == LightMode.Twinkle)
        {

            if (NowTime > 0.08f)
            {
                NowTime = 0.0f;

                for (int i = 0; i <= iMaxCount; i++)
                {

                    mLSliderList[i].SetActive(bShow);
                    mRSliderList[i].SetActive(bShow);

                    /*
                    TweenAlpha Ta = mLSliderList[i].GetComponent<TweenAlpha>();
                    if( Ta ) 
                    {
                        Ta.enabled = true; 
                        Ta.from = 0.0f;
                        Ta.to = 1.0f; 
                        Ta.duration =0.05f;
                        Ta.style = UITweener.Style.PingPong;
				
                    }
					
                    TweenAlpha Taa = mRSliderList[i].GetComponent<TweenAlpha>();
                    if( Taa ) 
                    {
                        Taa.enabled = true; 
                        Taa.from = 0.0f;
                        Taa.to = 1.0f; 
                        Taa.duration =0.05f;
                        Taa.style = UITweener.Style.PingPong;
                    }
                    */
                }
                bShow ^= true;

            }
        }
	}
	
		
	
	
}
