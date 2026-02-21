using UnityEngine;
using System.Collections;

public class Eff_UI_Win : MonoBehaviour {

	// Use this for initialization
    public GameObject mScoreObj = null;
    TweenPosition[] TP;
    UILabel mLabel; 


    void Awake()
    {
        mLabel = mScoreObj.GetComponent<UILabel>();
    }

    void Update()
    {
        Debug.Log("AAA");
    }
    void OnEnable()
    {

        TP = GetComponentsInChildren<TweenPosition>();
        
        for (int i = 0; i < TP.Length; ++i)
        {
            TP[i].enabled = true; 
            TP[i].Reset();
        }
        
        StartCoroutine(UpdateText());


    }

    IEnumerator UpdateText()
    {
        bool bLoop = true;
        
        float from = 0f;
        float to = MysteryMgr.Instance.mScore;

        int score = 0;
        float t = 0f;
        float fNowtime = 0.0f;
        
        while (bLoop)
        {
            fNowtime  += Time.deltaTime;
            t = fNowtime / 2f;
            score = Mathf.RoundToInt(Mathf.Lerp(from, to, t));

            mLabel.text = score.ToString();

            if (Mathf.RoundToInt(score) == MysteryMgr.Instance.mScore)
                bLoop = false;

            yield return null;
        }
    }


}
