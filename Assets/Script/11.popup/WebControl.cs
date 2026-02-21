using UnityEngine;
using System.Collections;


public class WebControl : MonoBehaviour
{

    public UILabel CloseLabel;
    public UILabel CloseTodayLabel;
    public GameObject pShowToday; 
    public UITexture mTexture;
    public GameObject LoadingIcon; 

    WebViewObject webViewObject;
    
    // Use this for initialization
    void Start()
    {
        if (CloseLabel) CloseLabel.text = TextManager.GetInstance().GetText(emString.Close);

        if (CloseTodayLabel) CloseTodayLabel.text = TextManager.GetInstance().GetText(emString.TodayClose);
    }
  
    public void OpenImgUrl(string url)
    {
        LoadingIcon.SetActive(true);

        Texture2D texture = CPopupCache.instance.GetData();
        
        if( texture != null  )
        {
            mTexture.gameObject.SetActive(true);
            mTexture.mainTexture = texture;
            mTexture.width = texture.width;
            mTexture.height = texture.height;
            LoadingIcon.SetActive(false);
        }
        else 
        {
            StartCoroutine(UpdateUserSprite(url, mTexture ));
        }
        
    }

    IEnumerator UpdateUserSprite(string url, UITexture tex)
    {
        if (url == string.Empty)
        {
            yield break;
        }
        else
        {
            WWW www = new WWW(url);
            yield return www;
            tex.gameObject.SetActive(true);
            tex.mainTexture = www.texture;
            tex.width = www.texture.width;
            tex.height = www.texture.height;
            LoadingIcon.SetActive(false);
            
            CPopupCache.instance.AddData(www.texture);


        }
    }

    public void OpenWebView(string url, string titleStr = "")
    {

        /*
        string strUrl = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}"
         , TDP_URL
         , "AppID    this.AppID
         , "EncryptSecret", this.EncryptSecret
         , "DeviceID", this.DeviceID
         , "MobileAccountID", this.MobileAccountID
         , "Price", amount);
        */

        //Application.OpenURL(strUrl);

        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init((msg) =>
        {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        });

        webViewObject.SetMargins(0, 0, 0, 70);
        webViewObject.LoadURL(url);
        webViewObject.SetVisibility(true);
    }

    public void SetMargins(int left, int top, int right, int bottom)
    {
        webViewObject.SetMargins(left, top, right, bottom);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                //TrueManager.GetInstance().webViewObject.SetVisibility(false);
                Destroy(webViewObject);
                //Application.Quit();
                return;
            }
        }
    }

    void OnEnable()
    {
        //OpenWebView("http://211.43.222.157/TS/mw_coin.php?mode=1");

        if (pShowToday)
        {
            //오늘 보기 기능 
            if (CPopupMgr.instance.CurPopupInfo != null)
            {
                if (CPopupMgr.instance.CurPopupInfo.todayUse == true)
                {
                    pShowToday.SetActive(true);
                }
                else
                {
                    pShowToday.SetActive(false);
                }
            }
        }
    
    }


    void OnDisable()
    {
        
        if( webViewObject )
            Destroy(webViewObject);
        
    }

}
