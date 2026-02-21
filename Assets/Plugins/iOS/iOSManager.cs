using UnityEngine;
//using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class iOSManager : MonoBehaviour {

    static iOSManager instance; 
    
    public static string strLog = "Unity3D ios Plugin Sample";

    //string TSOpenURL = "com.goticketspace.app.mysteryplinko://method=doPlay&unum=408&email=3F651426-2AD1-48C2-ABC7-C63D6EC2A96B&passwd=Z45dFtn42Ow56Cg7fhmb&gonum=76";
    string TSOpenURL = "com.goticketspace.app.mysteryplinko://method=doPlay&unum=408&email=3F651426-2AD1-48C2-ABC7-C63D6EC2A96B&passwd=Z45dFtn42Ow56Cg7fhmb&gonum=76";
    
    [DllImport("__Internal")]
    private static extern void iOSPluginHelloWorld(string strMessage);

    [DllImport("__Internal")]
	private static extern void iOSGoPlatform(); 

	[DllImport("__Internal")]
    private static extern void iOSCheckPlatform();

	[DllImport("__Internal")]
	private static extern void iOSLogout();

    [DllImport("__Internal")]
    private static extern void iOSStartGame();

	[DllImport("__Internal")]
	private static extern void iOSLogin(string str); 

	[DllImport("__Internal")]
	private static extern void iOSSetUserData(string str); 

	[DllImport("__Internal")]
	private static extern void iOSGetUserData(); 

	[DllImport("__Internal")]
	private static extern void iOSgoUrl(string str);
	
	[DllImport("__Internal")]
	private static extern void iOSgoApp(string pakageName , string iUrl);


    public static iOSManager GetInstance()
    {
        if(instance == null)
            instance = new GameObject("iOSManager").AddComponent<iOSManager>();

        return instance; 
    }

    public void Init(){}
    
	public void CheckPlatform()
	{
		//Debug.Log("UnityGetURL");
        iOSCheckPlatform(); 
	}
    
    public void StartGame()
    {
        iOSStartGame();
        Debug.Log("IOSManager StartGame");
    }
    
    public void LogoutGame()
    {
        iOSLogout();
        GameObject.Find("GameApplication").SendMessage("Logout", SendMessageOptions.DontRequireReceiver);
    }

    public void GoPlayform()
    {
        iOSGoPlatform();
    }
    
	public void GetUserData()
	{
		iOSGetUserData();
	}

	public void SetUserData(string str)
	{
#if UNITY_IOS
		iOSSetUserData(str);
#endif 

	}

	public void goUrl(string str)
	{
		iOSgoUrl(str);
	}

	public void goApp(string pakageName , string iUrl)
	{
		iOSgoApp(pakageName , iUrl );
	}



	public void Login()
	{
		//global
		//string str = Global.USER_ID + "&" + Global.USER_PW;
		//iOSLogin("");

	}

    //ios->unity 
    public void SetLog(string _strLog)
    {
        Debug.Log("Set UnityLog");
        strLog = _strLog;
		SceneManager.LoadScene("LoadingScene");

    }
    public void UserInfoMsg(string szMsg)
    {
		Debug.Log("iosmanager->UserInfoMsg" + szMsg );
        GameObject.Find("GameApplication").SendMessage("UserInfo", szMsg, SendMessageOptions.DontRequireReceiver);
    }


	public void AppReciveMsg(string szMsg)
    {
        Debug.Log("AppReciveMsg " + szMsg);

        if (szMsg.Contains("result:") == true)
		{
            GameObject.Find("GameApplication").SendMessage("TSReciveInfo", szMsg, SendMessageOptions.DontRequireReceiver);
			return; 
		}
        
        string[] components = (szMsg.Substring(szMsg.LastIndexOf("://") + 3)).Split('&');
		string cmdstr = Command("method", components); 

		GameObject.Find("GameApplication").SendMessage( "TSReciveInfo" , "result:"+cmdstr , SendMessageOptions.DontRequireReceiver  );

    }

	public void StartMode(string cmd )
	{
		SetLog(cmd);
	}

	
	public string Command(string str, string[] components)
	{
		string word;
		
		foreach (string s in components)
		{
			word = s.Substring(0, s.IndexOf('='));
			if (word.Equals(str))
			{
				int StartIndex = s.IndexOf('=')+1 ; // +1??-> ("=")
				return s.Substring(StartIndex, (s.Length - StartIndex)); 
			}
		}
		
		return ""; 
	}

	// Use this for initialization
    void Start() { }
	
}
