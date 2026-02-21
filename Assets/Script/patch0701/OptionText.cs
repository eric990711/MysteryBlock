using UnityEngine;
using System.Collections;

public class OptionText : MonoBehaviour {

    public UILabel SettingLabel; 
    public UILabel HowtoLabel;
    public UILabel LogoutLabel;
    public UILabel TermsLabel;

    public UILabel SoundLabel;
    public UILabel Effectlabel;
    public UILabel Vibration;
    public UILabel GSensor; 
    
	// Use this for initialization
    void Awake()
    {
        if (SettingLabel != null)
            SettingLabel.text = TextManager.GetInstance().GetText(emString.Setting);

        if (HowtoLabel != null)
            HowtoLabel.text = TextManager.GetInstance().GetText(emString.HowtoPlay);
        if (LogoutLabel != null)
            LogoutLabel.text = TextManager.GetInstance().GetText(emString.Logout);
        if (TermsLabel != null)
            TermsLabel.text = TextManager.GetInstance().GetText(emString.TermsNPolicies);

        if( SoundLabel != null)
            SoundLabel.text = TextManager.GetInstance().GetText(emString.Sound);
        if (Effectlabel != null)
            Effectlabel.text = TextManager.GetInstance().GetText(emString.EffectSound);
        if (Vibration != null)
            Vibration.text = TextManager.GetInstance().GetText(emString.Vibration);
        if (GSensor != null)
            GSensor.text = TextManager.GetInstance().GetText(emString.GSensor);
    }

	void Start () {
	        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
