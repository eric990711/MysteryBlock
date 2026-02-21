using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

enum ErrMsgType
{
    Common, Network, None
}

public class CallErrBox : MonoBehaviour {

    private static CallErrBox minst = null;

    //처음 로비로 접속

    public GameObject ErrBox;
    public UILabel ErrMsg;
    public UILabel ErrBtnLabel; 
    
    public GameObject NetErrBox;
    public UILabel NetErrBtnLabel;
    public UILabel NetErrDescLabel;
    
    ErrMsgType errtype = ErrMsgType.None;
    
    public static CallErrBox Instance
    {
        get
        {
            if (minst == null)
            {
                minst = FindObjectOfType<CallErrBox>();

                if (minst == null)
                {
                    minst = new GameObject("CallErrBox").AddComponent<CallErrBox>();
                    //m_instance = Instantiate(m_instance) as GameManager;
                }
            }
            return minst;
        }
    }

    void Awake()
    {
        ErrBox = gameObject.transform.Find("Camera/Anchor/ErrBox").gameObject;
        ErrMsg = ErrBox.transform.Find("Label").gameObject.GetComponent<UILabel>();
        ErrBtnLabel.text = TextManager.GetInstance().GetText(emString.Ok);
        NetErrBtnLabel.text = TextManager.GetInstance().GetText(emString.Ok);
        NetErrDescLabel.text = TextManager.GetInstance().GetText(emString.Err_ConnetError);

    }
    
    public void OpenErrMsg(string msg)
    {
        Debug.Log("OpenErrMSg" + msg);
        if (msg == "unknown error")
        {
            errtype = ErrMsgType.Network;
            OpenNetworkErrMsg();
        }
        else
        {
            errtype = ErrMsgType.Common;
            ErrBox.SetActive(true);
            ErrMsg.text = msg;
            GameClient.instance.IsPause = true;
        }

    }

    public void CloseErrMsg()
    {
      
        
        if (errtype == ErrMsgType.Common)
        {
            ErrBox.SetActive(false);
        }
        else if (errtype == ErrMsgType.Network)
        {
            NetErrBox.SetActive(false);
            Application.Quit();
            Debug.Log("Quit");
        }
        errtype = ErrMsgType.None;
        
        //ErrMsg.text = "";
        GameClient.instance.IsPause = false;
        LoadingBar.GetInstance().SetLock(false);

    }

    void OpenNetworkErrMsg()
    {
        NetErrBox.SetActive(true);
        GameClient.instance.IsPause = true;
    }


}
