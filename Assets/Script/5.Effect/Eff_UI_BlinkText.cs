using UnityEngine;
using System.Collections;

public class Eff_UI_BlinkText : MonoBehaviour
{
    public UILabel Label;

    public float mDelay = 0.5f;

    void OnEnable()
    {
        Label.gameObject.SetActive(false);
        StartCoroutine("UpdateText");
    }

    void OnDisable()
    {
        StopCoroutine("UpdateText");
    }

    

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator UpdateText()
    {
        bool bflag = true;

        while (true)
        {
            // Debug.Log("Coroutine UpdateText");

            if (MysteryMgr.Instance.mCreditCoin >= 1)
            {
                Label.text = "Start";
            }
            else
            {
                Label.text = "Insert Coin";
            }

            Label.gameObject.SetActive(bflag);
            yield return new WaitForSeconds(mDelay);
            bflag ^= true;
        }
    }

}
