using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour
{
	float updownDecaytime = 0.0f;
	float leftrightDecaytime = 0.0f;
	Vector3 offset = new Vector3(0,0,0);
	Vector3 position = new Vector3(0,0,0);
	float startTime;
		
	float amp = 0.01f; // 진폭 . 상하로 이동하는 정도. 값이 클수록 더 크게 진동 .
    //float amp2 = 0.005f;
	float amp2 = 0.01f;

	float freq = 9f; // 진동수. 초당 상하 운동 횟수. 값이 클수록 더 빨 진동 .
	float phase = 1f; // 위상 . 함수의 시작 포지ㅅㄴ.. 모르겠다 꾀꼬리 복붙은 위대해 
	
	bool updown;
	bool leftright;
	// Use this for initialization
	void Start ()
	{
	}

    public void InitShake()
    {
		offset = new Vector3(0,0,0);
      //  this.enabled = true; 
    	StartLeftRightShake(15.0f);
		StartUpDownShake(15.0f);
	  
    }

	public void StartUpDownShake( float decay )
	{
		startTime = Time.time;
		updownDecaytime = decay + Time.fixedTime - startTime;
		position = this.gameObject.transform.localPosition;
		updown = true;
	}
	
	public void StartLeftRightShake( float decay )
	{
		startTime = Time.time;
		leftrightDecaytime = decay + Time.fixedTime - startTime;
		position = this.gameObject.transform.localPosition;
		leftright = true;
	}
	
	// Update is called once per frame
	public void LateUpdate ()
	{
       if (updown) UpDownShake();
       if (leftright) LeftRightShake();
	}
	
	public void FixedUpdate ()
	{
	
	}
	
	public void SetOption(float _amp, float _freq) // 진폭, 진동수 
	{
		amp = _amp;
		freq = _freq;
	}
	
	public void UpDownShake()
	{
		float totaltime = Time.fixedTime - startTime;
		if( totaltime < updownDecaytime )
		{
			Vector3 pos = this.gameObject.transform.localPosition;
			
			pos -= offset;
			
			offset.y = Mathf.Sin( 2 * 3.14159f * (totaltime * freq) + phase ) * amp2 * (updownDecaytime - totaltime) / updownDecaytime ; 
			
			pos += offset;
			
			this.gameObject.transform.localPosition = pos;
		}
		else
		{
			updown = false;
			this.gameObject.transform.localPosition = this.gameObject.transform.localPosition;
			offset = new Vector3(0,0,0);
         	this.enabled = false;
		}
	}
	public void LeftRightShake()
	{
		float totaltime = Time.fixedTime - startTime;
		if( totaltime < leftrightDecaytime )
		{
			Vector3 pos = this.gameObject.transform.localPosition;
			
			pos -= offset;
			
			offset.x = Mathf.Sin( 2 * 3.14159f * (totaltime * freq) + phase ) * amp * (leftrightDecaytime - totaltime) / leftrightDecaytime ; 
			
			pos += offset;
			
			this.gameObject.transform.localPosition = pos;
		}
		else
		{
			offset = new Vector3(0,0,0);
			this.gameObject.transform.localPosition = this.gameObject.transform.localPosition;
			leftright = false;
          	this.enabled = false;
				
		}
	}
}
