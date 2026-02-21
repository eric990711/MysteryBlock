using UnityEngine;
using System.Collections;

public class CTermsMgr : MonoBehaviour
{


    public UILabel Termslabel;
    public UILabel Privacylabel;

    public UILabel TermsDesclabel;
    public UILabel PrivacyDesclabel;

    public UISprite AppIcon;

    public UILabel OkButton;

    void Awake()
    {
        TextAsset TermstextFile = null;
        TextAsset PrivacytextFile = null;

        //Debug.Log(Application.systemLanguage);



        if (MPUtil.GetSystemLanguage() == SystemLanguage.Korean)
        {
            TermstextFile = Resources.Load("text/terms_kr") as TextAsset;
            PrivacytextFile = Resources.Load("text/privacy_kr") as TextAsset;
        }
        else
        {
            TermstextFile = Resources.Load("text/terms_eng") as TextAsset;
            PrivacytextFile = Resources.Load("text/privacy_eng") as TextAsset;
        }

        if (TermsDesclabel != null)
        {
            TermsDesclabel.text = TermstextFile.text;
            PrivacyDesclabel.text = PrivacytextFile.text;
            Termslabel.text = TextManager.GetInstance().GetText(emString.TermsofService);
            Privacylabel.text = TextManager.GetInstance().GetText(emString.PrivacyStatement);

        }

        if (OkButton != null)
        {
            OkButton.text = TextManager.GetInstance().GetText(emString.Ok);
        }
    }

    void SwitchAppIcon()
    {

    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

