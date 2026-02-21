using UnityEngine;
using System.Collections;

public class EffChangeSprite : MonoBehaviour {
	
	//public GameObject obj; 
    public GameObject offLED;
    public GameObject onLED;
	
	public float Duration = 0.3f; 
	public float DurationEnd = 0.3f; 
	
	float fNowTime;
	float fDelayTime; 
		
		
	// Use this for initialization
	void Start () {

     
	}
	
    public void Play()
    {
        StartCoroutine(UpdateLED());

    }


    /*public IEnumerator UpdateLED(float _Duration)
    {
        bool bOnOff = true;
       
        Duration = _Duration;

        float fTime = 0.0f;
        
        while (true)
        {
            fTime += Time.deltaTime;
            if (fTime > Duration)
            {
                break; 
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(bOnOff);
            }
            
            yield return null;
            bOnOff ^= true; 
        }
    }*/

    IEnumerator UpdateLED()
    {
        Debug.Log("LEDUPDATE!!!!!");
        bool bToggle = true;
        bool bLoop = true;
        int cnt = 0;

        while (bLoop)
        {
            
            bToggle = true;
            offLED.SetActive(bToggle);
            onLED.SetActive(!bToggle);

            yield return new WaitForSeconds(0.1f);

            bToggle = false;
            offLED.SetActive(bToggle);
            onLED.SetActive(!bToggle);

            yield return new WaitForSeconds(0.1f);

            cnt++;
            if (cnt > 10)
            {
                bToggle = true;
                offLED.SetActive(bToggle);
                onLED.SetActive(!bToggle);
                yield break;
            }
            
        }
        yield return null;
    }

}
