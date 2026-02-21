using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

    public GameObject g_IntroSprite;

	// Use this for initialization
	void Start () {
        if(MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
            g_IntroSprite.GetComponent<UISprite>().spriteName = "Loading_Kor";
        else
            g_IntroSprite.GetComponent<UISprite>().spriteName = "Loading_Eng";

	}
    

	// Update is called once per frame
	void Update () {
	
	}
}
