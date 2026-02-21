using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum LoginType
{
    Start, 
    Login,
    Term, 
    TermAccount, 

}


public class LoginMgr : MonoBehaviour {

    public GameObject StartView; 
    public GameObject LoginView;
    public GameObject TermView;
    public GameObject TermAccountView;

    public UITexture Term_UserIcon;
    public UILabel Term_contryLabel;
    public UILabel Term_idLabel;
    
    

    static private LoginMgr instance;
    public static LoginMgr GetInstance()
    {
        if (instance == null)
            instance = FindObjectOfType<LoginMgr>();

        return instance;
    }
    
	// Use this for initialization
	void Start () {

	}
        

    public void SetUserTexture(string url)
    {
        StartCoroutine(UpdateUserTexutre(url));
    }


    IEnumerator UpdateUserTexutre(string url)
    {
        if( url.Equals("") == true )
            yield break; 
        
        WWW www = new WWW(url);
        yield return www;

        Term_UserIcon.mainTexture = www.texture;
        

        yield break; 
    }



    public void SetType(LoginType type )
    {
        StartView.SetActive(false);
        LoginView.SetActive(false);
        TermView.SetActive(false);
        TermAccountView.SetActive(false);

        switch(type)
        {
            case LoginType.Start: 
                StartView.SetActive(true);
                break; 
            case LoginType.Login: 
                LoginView.SetActive(true);
                break; 
            case LoginType.Term:
                Term_idLabel.text = "ID: "+ Global.USER_ID;
                Term_contryLabel.text = "CONTRY: " + Global.USER_CONTRY;
                
                TermView.SetActive(true);
                break; 
            case LoginType.TermAccount:
                TermAccountView.SetActive(true);

                break; 

        }

        
        
        
        
    }
}
