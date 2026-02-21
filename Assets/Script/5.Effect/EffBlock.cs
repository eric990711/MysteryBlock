using UnityEngine;
using System.Collections;

public class EffBlock : MonoBehaviour {
	
	Color lerpedColor = Color.red; 
	bool flag = true ; 
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		/*
		if( flag  ) 
		{
			lerpedColor = Color.Lerp(Color.red , Color.yellow, Time.time);
			
			if(lerpedColor.Equals(Color.yellow)) 
			{
				flag = false ; 
			}
			
		}
		else 
		{
			lerpedColor = Color.Lerp(Color.yellow , Color.red , Time.time);

			if(lerpedColor.Equals(Color.red)) 
			{
				flag = true ; 
			}
		}
		*/
		
		//renderer.material.color = new Color(Random.Range(0,255)/255.0f , Random.Range(0,255)/255.0f , Random.Range(0,255)/255.0f , 1.0f);
	
		
	}
}
