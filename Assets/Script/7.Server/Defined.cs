using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//미스테리 인게임에대한 정의 
public enum MysteryState
{
    Stay,   //대기 
    Start,  //시작 
    InGame, //인게임 
    Result, //결과 
};

//게임상태 
public enum GameState
{
    DirectStart,
    Start,
    Login,
    Lobby,
    Mystery,
};

public enum ItemType
{
    SpeedDown, 
    DoubbleTicket,
    DoubbleJackPot, 
}


public enum OSType
{
    ios,
    android,
}

public delegate void EventHandler();

static class Global 
{
    //로그인
    //https://www.goticketspace.com:7443/TicketSpace/module/loginUser.jsp?U_EMAIL=test30@email.com&U_PW=111111
    //룸정보받아오기 

    //http://www.hello5.kr:8080/spaceticket/module/loginUser.jsp?U_EMAIL=test30@email.com&U_PW=111111
     //www.hello5.kr:8080/spaceticket/module/loginUser.jsp?U_EMAIL=test30@email.com&U_PW=111111
    //public const string HOST = "http://www.hello5.kr:8080/spaceticket";
    public const string HOST = "https://goticketspace.com:7443/TicketSpace";
    
    public static string HOSTDIR = "";
//UNITY_IOS , UNITY_ANDROID 
    
#if UNITY_IOS
    public static OSType APP_OS_TYPE = OSType.ios;
#else 
    public static OSType APP_OS_TYPE = OSType.android;
#endif 
      
    public static string GAME_NAME = "";
    public const  string G_PACK_NAME = "com.goticketspace.app.mysteryblock";
	public static string USER_ID = "";//"test30@email.com";
    public static string USER_PW = "";//"111111";
    public static string USER_NUM = "";
    
    public static string USER_CONTRY ="us";
    public static string PLAY_KEY = ""; //플레이할때 서버에서 난수생성해서 보내줌

    public static string USER_GONUM ="";
    
    public static int MAXLIFE = 3;
    public static int MAXMACHINE = 8;

    public static string USER_LANGUAGE()
    {
        SystemLanguage sl_userLanguage = MPUtil.GetSystemLanguage();

        string s_uLang;

        if (sl_userLanguage == SystemLanguage.Korean) s_uLang = "kr";
        else s_uLang = "en";

        return s_uLang;
    }
    
    public static void MPDebug(string str)
    {
         Debug.Log(str);
    }
}





/*
public enum emObjName
{
    DropSprite,
    HandleSprite, 
    LeftSliderBar,
    Result,
    RightBarProc,
    TicketBarSprite,
    TicketText,
    TicketValueText, 
}

public class StringManager
{
    private static StringManager instance = null;

    private Dictionary<emObjName, string> m_String = new Dictionary<emObjName, string>()
    {
        { emObjName.DropSprite, "DropSprite" } , 

     
    };


    public string GetText(emObjName index)
    {
        return m_String[index];
    }

    public static StringManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StringManager();

            }
            return instance;
        }
    }
}

*/

