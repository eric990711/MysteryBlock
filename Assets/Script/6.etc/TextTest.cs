using UnityEngine;
using System.Collections;

public class TextTest : MonoBehaviour {
	
		
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		UILabel Label = GetComponent<UILabel>();
		if( Label )
		{
			Label.text = Screen.width + " " + Screen.height ;
		}
	}
}
