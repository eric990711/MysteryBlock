using UnityEngine;
using System.Collections;

public class EffBGColor : MonoBehaviour {
	
	public Color[] SetColor;
	
	
	
	public float Duration = 7; 
	
	public int BackGroundStep = 1; 
	Light mLight = null ;
		
	float fColorTime = 0 ; 
	
	
	void Awake() 
	{
		mLight = light; 
		fColorTime = 0 ; 
		
	}
	
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
		if(BackGroundStep == 0 )		 
			return; 
		
		fColorTime += Time.deltaTime; 
		float lerp = fColorTime/Duration;
		
		if(BackGroundStep <= 2 )
		{
			mLight.color = Color.Lerp(SetColor[BackGroundStep-1] , SetColor[BackGroundStep] , lerp );	
		}
		else 
		{
			mLight.color = Color.Lerp(SetColor[2] , SetColor[0] , lerp );	
		}
				
				
		if( lerp > 1.0f )
		{
			BackGroundStep++;
			fColorTime = 0 ; 
		
			if( BackGroundStep == 4 ) 
			BackGroundStep =1; 
					
		}
		
		
				
		
		
		//Vector3 vec = new Vector3(Mathf.Clamp01(Time.time), 0, 0);
		
		
	}
}















