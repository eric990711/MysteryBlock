using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoogleAnalytics : MonoBehaviour {
	
	public string propertyID;
	
	public static GoogleAnalytics instance;
	
	public string bundleID;
	public string appName;
	public string appVersion;
	
	private string screenRes;
	private string clientID;

    public string url;
    string sceneName = null;

//     public string SceneName()
//     {
//         switch (GameClient.mGameState)
//             {
//                 case GameState.Mystery: sceneName = "/InGame"; break;
//                 default: sceneName = "/Lobby"; break;
//             }
//         return sceneName;
    //}

    public string gs_SceneName ()
    {
        switch (GameClient.mGameState)
        {
            case GameState.Mystery: sceneName = "/����"; break;
            case GameState.Lobby: sceneName = "/�κ�"; break;
            default: sceneName = "/��Ʈ��"; break;
        }
        return sceneName;
    }

	
	void Awake()
	{
		if(instance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	void Start() 
	{
		screenRes = Screen.width + "x" + Screen.height;
		
		clientID = SystemInfo.deviceUniqueIdentifier;
	}

	/*public void LogScreen(string title)
	{
		
		title = WWW.EscapeURL(title);



        url = "http://www.google-analytics.com/collect?v=1&ul=" + langue + "&t=appview&sr=" + screenRes + "&an=" + WWW.EscapeURL(appName) + "&a=448166238&tid=" + propertyID + "&aid=" + bundleID + "&cid=" + WWW.EscapeURL(clientID) + "&_u=.sB&av=" + appVersion + "&_v=ma1b3&cd=" + title + "&qt=2500&z=185";
		
		WWW request = new WWW(url);
		
		if(request.error == null)
		{
			if (request.responseHeaders.ContainsKey("STATUS"))
			{
				if (request.responseHeaders["STATUS"] == "HTTP/1.1 200 OK")	
				{
					Debug.Log ("GA Success");
				}else{
					Debug.LogWarning(request.responseHeaders["STATUS"]);	
				}
			}else{
				Debug.LogWarning("Event failed to send to Google");	
			}
		}else{
			Debug.LogWarning(request.error.ToString());	
		}

        Debug.Log(url);
	}*/

    public IEnumerator LogScreenWWW()
    {
        // Google Analytics 구버전 - HTTP 비보안 연결 차단됨 (Unity 6), 비활성화
        yield break;

        url = "http://www.google-analytics.com/collect?v=1&ul=en-us&t=appview&sr=" + screenRes + "&an=" + WWW.EscapeURL(appName) + "&a=448166238&tid=" + propertyID + "&aid=" + bundleID + "&cid=" + appName+"_"+WWW.EscapeURL(clientID) + "&_u=.sB&av=" + appVersion + "&_v=ma1b3&cd=" + WWW.EscapeURL(gs_SceneName()) + "&qt=2500&z=185";

        WWW request = new WWW(url);

        Debug.Log(gs_SceneName());

        yield return request;



        if (request.error == null)
        {
            if (request.responseHeaders.ContainsKey("STATUS"))
            {
                if (request.responseHeaders["STATUS"] == "HTTP/1.1 200 OK")
                {
                    Debug.Log("GA Success");
                }
                else
                {
                    Debug.LogWarning(request.responseHeaders["STATUS"]);
                }
            }
            else
            {
                Debug.LogWarning("Event failed to send to Google");
            }
        }
        else
        {
            Debug.LogWarning(request.error.ToString());
        }

        Debug.Log(url);
        Debug.Log(request.ToString());

        
    }

    

	
	
}