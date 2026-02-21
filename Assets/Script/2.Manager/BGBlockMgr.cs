using UnityEngine;
using System.Collections;



//배경블록 컨트롤한다.
public class BGBlockMgr : MonoBehaviour {

	// Use this for initialization
	public GameObject[] blockList;
	bool bFlagRotate = true; 
	bool bPingPong = true; 
	float fNowTime = 0; 
	
	
	void Awake()
	{
		//blockList.AddComponent		
		//blockList = GameObject.FindGameObjectsWithTag("BGBlock");
		fNowTime = 0 ; 
		blockList = GameObject.FindGameObjectsWithTag("BGBlock");
	}
	
	void Start () {
	}


    void RotateBGBlock()
    {

        foreach (GameObject go in blockList)
        {

            if ((int)go.transform.eulerAngles.y == 80)
            {
                bFlagRotate = false;
            }
            else if ((int)go.transform.eulerAngles.y == 280)
            {
                bFlagRotate = true;
            }

            if (bFlagRotate)
            {
                go.transform.Rotate(Vector3.up * Time.deltaTime * 5.0f);
            }
            else
            {
                go.transform.Rotate(Vector3.up * -(Time.deltaTime * 5.0f));
            }

        }
    }

	// Update is called once per frame
	void Update () {

        //if (GameClient.instance.mPause == true)
         //   return; 
   
		fNowTime += Time.deltaTime;
				
		if(fNowTime < 2 )
			return ;

        RotateBGBlock();
		
		//Debug.Log( blockList[0].transform.eulerAngles.y );
		/*
		for(int i =0 ; i < blockList.Length ; ++i)
		{
		
		}
		*/
		
	}
}
