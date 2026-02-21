using UnityEngine;
using System.Collections;

public class Eff_StartButton : MonoBehaviour {

	// Use this for initialization
    GameObject EffSprite = null;

    bool bFlag = false;
    float NowTime = 0.0f;

    void Awake()
    {
        EffSprite = transform.Find("EffSprite").gameObject;
        EffSprite.SetActive(false);
    }
    
	// Update is called once per frame
	void Update () {
        
        if (MysteryMgr.Instance.mCreditCoin > 0)
        {
            NowTime += Time.deltaTime;

            if (NowTime > 0.5f)
            {
                bFlag ^= true;
                NowTime = 0;

                EffSprite.SetActive(bFlag);
            }
        }
	}
}
