using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartLoad: MonoBehaviour {
	
	
	//public GameObject BarObj; 
    public Texture BgTexture;
    public Texture[] lodaingimg;
    
	public Rect BgRect  ;
	public Rect loadingimgRect;
		
    AsyncOperation async;
    string PercentStr = "100%";
    bool IsloadGame = false;
    float fNowTime = 0.0f;
    int count = 0;
    float Aspect = 10.0f;


    void Start() {
        
        //yield return new WaitForSeconds(0.1f);
        
        StartCoroutine("StartLoader", GameClient.SceneName);



	}
	// Update is called once per frame
	void Update () {

        /*
        fNowTime += Time.deltaTime;
		
        if (fNowTime > 0.1f)
        {
			count++;
            fNowTime = 0.0f; 
        }
		
		if( count >11 ) count = 0 ; 
		*/

	}
		
    void OnGUI() {
       // GUI.Label(new Rect(0, 0, 100, 20), PercentStr );
		//GUI.Label(new Rect(0, 40, 100, 20), fNowTime.ToString() );
		//GUI.DrawTexture( BgRect , BgTexture , ScaleMode.StretchToFill , false  ,Aspect );
		//GUI.DrawTexture( loadingimgRect , lodaingimg[count] , ScaleMode.StretchToFill , true ,Aspect );
		
    }
	
	
	
    public IEnumerator StartLoader(string strSceneName)
    {
        yield return new WaitForSeconds(0.5f);
        
        //Debug.Log("loading" + strSceneName);
        
		if (IsloadGame == false)
        {
            IsloadGame = true; 
			async = SceneManager.LoadSceneAsync(strSceneName);
       			
			while(true)
            {
			
                //float p = async.progress *100f;
				//int pRounded = Mathf.RoundToInt(p);
                //PercentStr  = "Loading..." + pRounded.ToString() ;
                //scriptPercent.sliderValue = async.progress ;
                yield return null; 
				
				if( async.isDone == true ) break; 				
				
            }
        }		
    }


	
		
}
