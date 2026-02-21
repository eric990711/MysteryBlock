using UnityEngine;
using System.Collections;


public class HelpBox : MonoBehaviour {

    
    public static HelpBox Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<HelpBox>();

            }
            return m_instance;
        }
    }

    private static HelpBox m_instance = null;

    //내용

    public UILabel Titlelabel; 
    public UILabel DescTextLabel;
    public UILabel Btnlabel;
    
    public UIAnchor TermPage;
    public GameObject mView;
    public GameObject mTermView;

    void Awake()
    {
        Titlelabel.text = TextManager.GetInstance().GetText(emString.HowtoPlay);
        DescTextLabel.text = TextManager.GetInstance().GetText(emString.HowtoPlayDesc);
        Btnlabel.text = TextManager.GetInstance().GetText(emString.Ok);
    }
    
    void Start()
    {
        
    }


    public void MsgBoxActive(bool bActive)
    {
        mView.SetActive(bActive);
    }
    
    public bool IsBoxActive()
    {
        return mView.activeSelf;
    }

    public void OpenHelpBox()
    {
        GameClient.instance.IsPause = true;

        MsgBoxActive(true);
    }

    public void CloseHelpBox()
    {
        GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_Button);
        GameClient.instance.IsPause = false;
        MsgBoxActive(false);
    }

    public bool isTermActive()
    {
        return mTermView.activeSelf;
    }

    public void TermViewActive(bool bActive)
    {
        mTermView.SetActive(bActive);
    }

    public void OpenTermView()
    {
        TermViewActive(true);
    }

    public void CloseTermView()
    {
        GameClient.OneShotSound(Vector3.zero, GameClient.instance.mSnd_Button);
        GameClient.instance.IsPause = false;
        TermViewActive(false);
        Debug.Log("CloseTermview");
    }

}
