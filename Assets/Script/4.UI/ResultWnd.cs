using UnityEngine;
using System.Collections;

public class ResultWnd : MonoBehaviour {

    
    public GameObject ResultLabel;
    public GameObject CoinLabel;
    public GameObject JackpotLabel; 
    public GameObject MysteryBlockIcon;
    public GameObject TotalLabel;
    

	// Use this for initialization
	void Start () {


	}

	// Update is called once per frame
    void OnEnable()
    {

        TweenScale scale = GetComponent<TweenScale>();
        if (scale)
        {
            scale.Reset();
            scale.duration = 0.2f;
            scale.enabled = true; 
        }

        if (MysteryMgr.Instance.mMissionSuccess == true)
        {
            NGUITools.PlaySound(GameClient.instance.mSnd_Win);
            ResultLabel.GetComponent<UILabel>().text = "WIN";

        }
        else
        {
            NGUITools.PlaySound(GameClient.instance.mSnd_Lose);
            ResultLabel.GetComponent<UILabel>().text = "LOSE";
        }


        if (MysteryMgr.Instance.mUseCoinx2 == true)
        {
            CoinLabel.GetComponent<UILabel>().text = "x 2";
        }
        else
        {
            CoinLabel.GetComponent<UILabel>().text = "x 1";
        }


        if (MysteryMgr.Instance.mGetItem[(int)ItemType.DoubbleJackPot] == true)
            MysteryBlockIcon.GetComponent<UISprite>().spriteName = "img_777";
        else if (MysteryMgr.Instance.mGetItem[(int)ItemType.DoubbleTicket] == true)
            MysteryBlockIcon.GetComponent<UISprite>().spriteName = "img_X2";
        else if (MysteryMgr.Instance.mGetItem[(int)ItemType.SpeedDown] == true)
            MysteryBlockIcon.GetComponent<UISprite>().spriteName = "img_SlowDown";


        JackpotLabel.GetComponent<UILabel>().text = MysteryMgr.Instance.mJackpotTicket.ToString(); 

        TotalLabel.GetComponent<UILabel>().text = MysteryMgr.Instance.mTotalTicket.ToString();
         

    }

}
