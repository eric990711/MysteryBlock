using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//팝업과 관련된 모든것은 여기서 컨트롤 한다. 

public enum ActionType
{
    link = 0,  // m/w 페이지를 호출한다.. 
    Web = 100,   //웹이동 
    App = 200,      //어플이동
    Screen = 300,   //화면이동
    Guide = 400,    //가이드
}


// - 팝업 데이터 리스트로 관리 한다. 
// - 씬 또는 윈 실패시 계속 팝업정보는 초기화및 데이터 갱신이 된다.. 
[Serializable]
public class popupInfo
{
    public int num;    //고유값 
    public bool bJacpot;    //잭팟 팝업이냐 ? 
    public string title;   //제목 
    public string imageLink;   //이미지주소
    public string link;        // 링크가 존재하면 웹페이로 바로뛰운다. 
    public ActionType actionType;  //액션 100 : 웹이동 200 , 어플이동 300 , 화며이동 400 , 가이드
    public string actionLink;  // 액션 링크 주소
    public string language;    // kr 한국어 en 영어
    public string tUrl;         // 티스토어용 PID 
    public string iUrl;         // 아이폰 url
    public string g_num;         // 게임번호
    public string go_num;        // 게임 머신번호
    public bool todayUse;


    public bool bShow;          //한번이라두 보여준거면 true 한다.. 중복해서 띄우지 않기 위해 !!


    public popupInfo()
    {
        num = 0;
        bJacpot = false;
        bShow = false;
        title = "";
        imageLink = "";
        link = "";
        actionType = ActionType.link;
        language = "kr";
        todayUse = false;
    }

}

// - 크게 모바일웹과 인텐트로 구분지은다.. 
// -액션에 따라서 나뉘어 준다..  화면이동은 플랫폼에서만 사용가능하니 일단 패스
// -인테트 로 어플실행시 액션링크를 값을 안드로이드로 넘겨준다. 

public class CPopupMgr : MonoBehaviour
{


    //인텐트 실행할때는 웹컨트롤를 끈다. 
    public WebControl pWebContorl;

    public bool bOnWebview = false; //웹 오픈했는지 체크

    public bool bCheckToday = false;

    public popupInfo CurPopupInfo = null;

    public List<popupInfo> mPopupList = new List<popupInfo>();

    GameObject pWebPopup = null;

    bool pJacpot = false;


    //한번 본 팝업은 리스트 정보에서 제외 시키기떄문에 초기화를 팝업의 새로운 정보가 들어오면 초기화 시키고 새로뿌려준다. 
    public void Init()
    {
        mPopupList.Clear();
        CurPopupInfo = null;

    }

    public void Add(popupInfo info)
    {
        //같은 데이터가 있는지 체크 ? 
        foreach (var data in mPopupList)
        {
            if (data.num == info.num)
                return;
        }

        Debug.Log(info.num);

        mPopupList.Add(info);
    }

    //순차적으로 팝업정보를 넘겨준다. 
    public popupInfo GetPopupData(bool bJackpot = false)
    {

        //같은나라가 아니면 
        string lang = TextManager.GetInstance().GetLanguage(SystemLanguage.Korean) ? "kr" : "en";

        Debug.Log("GetPopupdata " + lang);

        foreach (var p in mPopupList)
        {
            //잭팟 팝업인지 아닌지 같은것으로만 검사
            Debug.Log("GetPopupData bJacjackpot : " + bJackpot + " p.bJackpot : " + p.bJacpot + " CheckBlockPopup : " + CheckBlockPopup(p.num) + " p.bshow : " + p.bShow + " p.language.equals : " + p.language.Equals(lang));

            if (p.bJacpot == bJackpot)
            {

                Debug.Log("p.bjackpot equal bjackpot");

                //블럭된 팝업진 , 한번보여준 팝업 , 폰어 언어체크해서 같은것으로 뿌려줌
                if (CheckBlockPopup(p.num) == false && p.bShow == false && p.language.Equals(lang) == true)
                {
                    p.bShow = true;

                    Debug.Log("PopupList Foreach : " + p);

                    return p;


                }
            }
        }

        return null;
    }

    // Use this for initialization
    void Start()
    {

        Init();

        LoadPopupInfo();

    }


    [Serializable]
    public class PopupEntry
    {
        public int num;
    }

    public List<PopupEntry> mblockList = new List<PopupEntry>();


    public bool PopupBlockAdd()
    {
        foreach (var popup in mblockList)
        {
            if (popup.num == CurPopupInfo.num)
                return false;
        }

        //추가하고 
        mblockList.Add(new PopupEntry { num = CurPopupInfo.num });

        //저장한다.
        SavePopupInfo();

        return true;
    }

    //true 팝업이 저장되어 있다 .. 없으면 한번도 보여주지 않은것으로 판명
    public bool CheckBlockPopup(int num)
    {
        foreach (var obj in mblockList)
        {
            Debug.Log(string.Format("popup num {0} ", obj.num));

            if (obj.num == num)
                return true;
        }

        return false;
    }

    public void SavePopupInfo()
    {
        Debug.Log("SavePopupInfo");

        var b = new BinaryFormatter();

        var m = new MemoryStream();

        b.Serialize(m, mblockList);

        PlayerPrefs.SetInt("CurDay", System.DateTime.Now.Day);

        PlayerPrefs.SetString("PopUpList", System.Convert.ToBase64String(m.GetBuffer()));

        PlayerPrefs.Save();

    }

    public void LoadPopupInfo()
    {
        int Day = PlayerPrefs.GetInt("CurDay");

        //날짜가 다르면 초기화 한다. 
        if (Day != System.DateTime.Now.Day)
        {
            mblockList.Clear();
            SavePopupInfo();
            return;
        }

        //같은날 다시 실행했을경우 기존정보 로드
        var data = PlayerPrefs.GetString("PopUpList");

        if (!string.IsNullOrEmpty(data))
        {
            var b = new BinaryFormatter();

            var m = new MemoryStream(System.Convert.FromBase64String(data));

            mblockList.Clear();
            mblockList = (List<PopupEntry>)b.Deserialize(m);
            Debug.Log("Load");
        }
    }

    //{"result":"ok","popupBeforeData":[{"content":"오프로더 게임팝업","title":"오프로더 게임 팝업","num":7,"location":"bg","link":"http://naver.com","buttonLink":"http://daum.net"}]}

    //sdh 20140704 팝업작업 
    public void OpenWebPopupView(GameObject web, bool bJacpot = false)
    {

        if (GameClient.instance.isVisitor == true || (GameClient.instance.isVisitor == false && GameClient.instance.isIngame == true))
            return;

        Debug.Log("OpenWebPopupView " + web + " bJacpot  " + bJacpot + "PopupListCnt " + mPopupList.Count);

        foreach (var data in mPopupList)
        {
            Debug.Log(mPopupList.Count);
        }

        Debug.Log("팝업리스트 갯수 : "+mPopupList.Count);

        //팝업정보가 없으면 리턴시킨다. 
        if (mPopupList.Count == 0)
        {
            Debug.Log("return mPopupList.Count " + mPopupList.Count);
            return;
        }
        if (web == null)
        {
            Debug.Log("return Web null " + web);
            return;
        }

        Debug.Log("OpenWebPopupView return through");

        //기억한후에 리오픈할때 쓴다. 
        pWebPopup = web;
        pJacpot = bJacpot;

        bOnWebview = false;
        bCheckToday = false;

        Transform ShowBtnObj = pWebPopup.transform.Find("PopUpShowBtn");

        if (ShowBtnObj != null)
        {
            ShowBtnObj.Find("NormalButton").gameObject.SetActive(true);
            ShowBtnObj.Find("PressButton").gameObject.SetActive(false);
        }


        //잭팟일때와 아닐때 같은정보를 포함 하기때문에 
        CurPopupInfo = GetPopupData(bJacpot);

        if (CurPopupInfo != null)
        {

            Debug.Log("오늘안보기 : " + CheckBlockPopup(CurPopupInfo.num));

            //오늘안보기 기능
            if (CheckBlockPopup(CurPopupInfo.num) == true) { Debug.Log("today show off"); return; }


            Debug.Log("today show through");
            //여기서 팝업 특성에 맞게 분기처리해준다. 
            //이미지 출력 해준다. 
            Debug.Log("오픈 이미지 URL : " + CurPopupInfo.imageLink);
            web.SetActive(true);
            web.GetComponent<WebControl>().mTexture.gameObject.SetActive(false);
            web.GetComponent<WebControl>().OpenImgUrl(CurPopupInfo.imageLink);

            //web.GetComponent<WebControl>().OpenWebView(CurPopupInfo.imageLink );
        }
        else
        {
            Debug.Log("CurpopupInfo null");
            web.SetActive(false);
            return;
        }

        bOnWebview = true;

        //web.GetComponent<WebControl>().SetMargins(6, 80, 6, 130);
    }

    // sdh 20140704 팝업관련 OpenWebView

    public void OpenWebView(bool on, GameObject web)
    {
        bOnWebview = on;
        web.transform.Find("WebCloseBtn/Label").gameObject.GetComponent<UILabel>().text = TextManager.GetInstance().GetText(emString.Ok);

        if (bOnWebview == true)
        {
            web.GetComponent<WebControl>().OpenWebView("http://211.43.222.157/TS/mw_coin.php?mode=1");
        }

        web.SetActive(bOnWebview);
    }


    public void PopupAction()
    {
        Debug.Log("PopupRun ActionType " + CurPopupInfo.actionType);

        if (CurPopupInfo.actionType == ActionType.Web)
        {
#if UNITY_ANDROID
            CAndroidManager.GetInstance().goURL(CurPopupInfo.actionLink);
#elif UNITY_IOS
			iOSManager.GetInstance().goUrl(CurPopupInfo.actionLink);
#endif
        }
        else if (CurPopupInfo.actionType == ActionType.App)
        {

            //어디서든 머신이동으로 처리
            if (CurPopupInfo.go_num != string.Empty)
            {
                Global.USER_GONUM = CurPopupInfo.go_num;
                GameClient.mGameState = GameState.Mystery;
                SceneManager.LoadScene("LoadingScene");
            }
            else
            {

                bool tstore = (GameApplication.GetInstance().market == Market.TSTORE) ? true : false;

#if UNITY_ANDROID
                CAndroidManager.GetInstance().goApp(CurPopupInfo.actionLink, CurPopupInfo.tUrl, tstore);
#elif UNITY_IOS
				iOSManager.GetInstance().goApp(CurPopupInfo.actionLink , CurPopupInfo.iUrl );
#endif
            }
        }
        else if (CurPopupInfo.actionType == ActionType.Guide)
        {
            //가이드 문서이무로 아무런 행동을 안함.. 
        }
        else
        {
            CallErrBox.Instance.OpenErrMsg("Error Action Type + " + CurPopupInfo.actionType);
        }
    }


    public void Close()
    {
        bOnWebview = false;

        if (bCheckToday == true)
            CPopupMgr.instance.PopupBlockAdd();


        //리스트에서 제거한다. 
        //mPopupList.Remove(CurPopupInfo);

        //다음 팝업 오픈 
        Invoke("DelayOpenPopup", 0.01f);

    }

    void DelayOpenPopup()
    {
        OpenWebPopupView(pWebPopup, pJacpot);
    }

    public static CPopupMgr instance
    {
        get
        {
            if (minstance == null)
            {
                minstance = FindObjectOfType<CPopupMgr>();
            }
            return minstance;
        }
    }
    private static CPopupMgr minstance = null;


}
