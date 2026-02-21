using UnityEngine;
using System.Collections;

public class WinZoneProc : MonoBehaviour {

    public UILabel label = null; 

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {

        if (MysteryMgr.Instance.mBlockStep < 3)
        {
            label.text = "TICKET ZONE";
        }
        else
        {
            label.text = "WIN ZONE";
        }


	}
}
