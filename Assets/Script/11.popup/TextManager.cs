using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum emString
{
    Close, //닫기
    Start,              //시작
    Yes,                //예
    No,                 //아니오
    Ok,                 //확인
    HowtoPlay,          //게임방법
    HowtoPlayDesc,      //게임설명
    LogoutMsg,             //로그아웃
    IngameExitMsg,         //인게임아웃
    GetTicketMsg,          //티켓얻기
    PlatformMsg,           //플랫폼이동 
    CoinChargeMsg,         //코인충전
    CoinNotEnoughMsg,      //코인이 부족
    NoPlatformMsg,         //플랫폼 없을때
    BackbuttonMsg,         //뒤로가기 버튼
    Setting,            //설정
    Sound,              //사운드
    EffectSound,        //이펙트사운드
    GSensor,            //센서
    Vibration,          //진동
    Logout,             //로그아웃
    TermsNPolicies,      //이용약관
    TermsofService,      //이용약관
    PrivacyStatement,    //정책
    Err_ConnetError,    //인터넷연결 이 안될때
  

    ExitTutoLobby,
    ExitTutoIngame,
    EndTuto,
    IOS_Platform,
    MysteryBlock,
    MysteryOffRoder,
    MysteryDOD,
    MysteryPlinko,
    MysteryHotFive,
    MysteryColour,
    MysteryWinnerBall,
    GetTicketAndJackPot,
    GetTicketAndResult,
    FBSHARETEXT,
    FBFULLREWARD,
    FBSHARERESULT,
    FBSHAREWIN,
    TodayClose,         //오늘은 그만보기

    MaxCount        //스트링길이
}

public class TextManager
{
    private static TextManager instance = null;

    string howtoText_kr = "";
    string howtoText_eng = "";
    SystemLanguage mLanguage;


    private Dictionary<int, string> m_StringKor = new Dictionary<int, string>()
    {
        {(int)emString.Close, "닫기" },
        {(int)emString.Start, "시작" },
        {(int)emString.Yes, "예" },
        {(int)emString.No, "아니오" },
        {(int)emString.Ok, "확인" },
        {(int)emString.HowtoPlay, "게임 방법"},
        {(int)emString.HowtoPlayDesc, ""},
        {(int)emString.LogoutMsg, "로그아웃 하시겠습니까?"},
        {(int)emString.IngameExitMsg, "게임 로비로 이동하시겠습니까?"},
        {(int)emString.GetTicketMsg, "획득하신 티켓을 받으시겠습니까? \n\n티켓 스페이스로 이동 됩니다."},
        {(int)emString.PlatformMsg, "게임을 종료 하시겠습니까? \n\n티켓 스페이스로 이동 됩니다."},
        {(int)emString.CoinChargeMsg, "코인을 충전 하시겠습니까?\n\n티켓 스페이스로 이동 됩니다."},
        {(int)emString.CoinNotEnoughMsg, "코인이 부족 합니다.! \n\n코인을 충전 하시겠습니까? \n\n티켓 스페이스로 이동 됩니다. "},
        {(int)emString.NoPlatformMsg, "티켓 스페이스 설치가 필요 합니다. \n\n설치 페이지로 이동 하시겠습니까?"},
        {(int)emString.BackbuttonMsg, "게임을 종료 하시겠습니까?"},
        {(int)emString.Setting, "설정"},
        {(int)emString.Sound, "배경음"},
        {(int)emString.EffectSound, "효과음"},
        {(int)emString.GSensor, "자이로센서"},
        {(int)emString.Vibration, "진동"},
        {(int)emString.Logout, "로그아웃"},
        {(int)emString.TermsNPolicies, "이용약관"},
        {(int)emString.TermsofService, "이용약관"},
        {(int)emString.PrivacyStatement, "개인정보취급방침"},
        {(int)emString.Err_ConnetError, "네트워크 연결중 문제가 발생했습니다. \n\n다시 시도해 주세요"},
        {(int)emString.ExitTutoLobby, "튜토리얼을 종료하시겠습니까?"},
        {(int)emString.ExitTutoIngame, "튜토리얼을 종료하고 로비로 이동하시겠습니까?"},
        {(int)emString.EndTuto, "보상을 확인하러 플랫폼으로 이동하시겠습니까?"},
        {(int)emString.MysteryBlock, "미스터리 블럭"},
        {(int)emString.MysteryOffRoder, "미스터리 오프로더"},
        {(int)emString.MysteryDOD, "미스터리 빅 딜"},
        {(int)emString.MysteryPlinko, "미스터리 플링코"},
        {(int)emString.MysteryHotFive, "미스터리 핫파이브"},
        {(int)emString.MysteryColour, "미스터리 컬러"},
        {(int)emString.MysteryWinnerBall, "미스터리 위너볼"},
        {(int)emString.GetTicketAndJackPot, "저는 티켓 스페이스 {0} 게임을 즐기고 있으며,어려운 Win ({1} Ticket) 달성 을 했습니다. 축하해주세요"},
        {(int)emString.GetTicketAndResult, "저는 티켓 스페이스 {0} 게임을 즐기고 있으며, 티켓 ({1} Ticket) 을 받았습니다. 제 최고점에 도전해 보세요"},        {(int)emString.FBSHARETEXT, "페이스북에 WIN 점수를 공유하시고 \n무료게임 즐기세요"},        {(int)emString.FBFULLREWARD, "일일 최대 공유 횟수를 초과하셨습니다.\n무료게임이 적용되지 않습니다."},
        {(int)emString.FBSHARERESULT, "공유하셨습니다. 로비로 이동합니다."},
        //20140902 이재우 수정작업진행
        {(int)emString.FBSHAREWIN, "축하합니다. 페이스북 공유 보상으로\n무료게임 1회가 지급되었습니다.\n\n[007fff]'확인버튼'[-]을 누르면 바로 무료게임이 진행됩니다.\n({0}회공유 / 최대{1}회)"},
        {(int)emString.TodayClose, "오늘은 그만보기"}
      
    };

    private Dictionary<int, string> m_StringEng = new Dictionary<int, string>()
    {
        {(int)emString.Close, "CLOSE" },
        {(int)emString.Start, "START" },
        {(int)emString.Yes, "YES" },
        {(int)emString.No, "NO" },
        {(int)emString.Ok, "OK" },
        {(int)emString.HowtoPlay, "How to Play"},
        {(int)emString.HowtoPlayDesc, ""},
        {(int)emString.LogoutMsg, "Do you want to logout?"},
        {(int)emString.IngameExitMsg, "Do you want to go to lobby?"},
        {(int)emString.GetTicketMsg, "Are you sure to receive tickets ?\n\n Finish the Game And Return to 'Ticket Space'"},
        {(int)emString.PlatformMsg, "Are you sure to exit this Game? \n\nMove to Ticket Space?"},
        {(int)emString.CoinChargeMsg, "Do you want to get more coins? \n\nMove to Ticket Space."},
        {(int)emString.CoinNotEnoughMsg, "Coins are not enough! \n\nDo you want to get coins? \n\nMove to 'Ticket Space'"},
        {(int)emString.NoPlatformMsg, "Go to download Ticket Space App."},
        {(int)emString.BackbuttonMsg, "Are you sure to exit this Game?"},
        {(int)emString.Setting, "Setting"},
        {(int)emString.Sound, "Sound"},
        {(int)emString.EffectSound, "Effect"},
        {(int)emString.GSensor, "GSensor"},
        {(int)emString.Vibration, "Vibration"},
        {(int)emString.Logout, "Logout"},
        {(int)emString.TermsNPolicies, "Terms and Policies"},
        {(int)emString.TermsofService, "Terms of Service"},
        {(int)emString.PrivacyStatement, "Pricacy Statement"},
        {(int)emString.Err_ConnetError, "Network is not Connected, \nPlease Check your internet connection and try again"},
        {(int)emString.ExitTutoLobby, "Do you want to finish up the tutorial?"},
        {(int)emString.ExitTutoIngame, "Do you want to finish up the tutorial and move to lobby?"},
        {(int)emString.EndTuto, "Do you want to move to the 'Ticket Space' platform to check reward?"},
		{(int)emString.IOS_Platform, "Are you sure to exit this Game?"},
        {(int)emString.MysteryBlock, "Mystery Block"},
        {(int)emString.MysteryOffRoder, "Mystery OffRoder"},
        {(int)emString.MysteryDOD, "Mystery Big Deal"},
        {(int)emString.MysteryPlinko, "Mystery PLINKO"},
        {(int)emString.MysteryHotFive, "Mystery HotFive"},
        {(int)emString.MysteryColour, "Mystery Colour"},
        {(int)emString.MysteryWinnerBall, "Mystery WinnerBall"},
        {(int)emString.GetTicketAndJackPot, "Ticket space {0} Game, Win ({1} Ticket) congratulations"},
        {(int)emString.GetTicketAndResult, "Ticket space {0} Game, Win ({1} Ticket)"},
        {(int)emString.FBSHARETEXT, "Share at facebook your WIN \nYou can play free game."},        {(int)emString.FBFULLREWARD, "You've got all rewards \noffered at today."},
        {(int)emString.FBSHARERESULT, "Share at Facebook.\nMove to Lobby"},
        {(int)emString.FBSHAREWIN, "Share at Facebook.\nYou can challenge free game\n({0}share / {1} maxshare)"},
        {(int)emString.TodayClose, "Don’t show today"}
        
    };

    public string GetText(emString _string)
    {

        int index = (int)_string;

        if (mLanguage == SystemLanguage.Korean)
        {
            //    return m_StringEng[index];
            return m_StringKor[index];
        }
        else
        {
            return m_StringEng[index];
        }

    }



    TextManager()
    {
        mLanguage = MPUtil.GetSystemLanguage();

        Debug.Log("TextManager Create" + mLanguage);
        TextAsset howtoTextEng = Resources.Load("text/howtoplay_eng") as TextAsset;
        if (howtoTextEng != null) m_StringEng[(int)emString.HowtoPlayDesc] = howtoTextEng.text;

        TextAsset howtoTextkr = Resources.Load("text/howtoplay_kr") as TextAsset;
        if (howtoTextkr != null) m_StringKor[(int)emString.HowtoPlayDesc] = howtoTextkr.text;
    }

    public bool GetLanguage(SystemLanguage lang)
    {
        return mLanguage == lang ? true : false;
    }

    public static TextManager GetInstance()
    {
        if (instance == null)
        {

            Debug.Log("MPLang " + MPUtil.GetSystemLanguage().ToString());
            instance = new TextManager();
        }
        return instance;
    }

}
