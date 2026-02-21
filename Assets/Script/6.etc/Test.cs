using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		//오브젝트 찾는 방법.
		//tranform.Find("test")
		//GetComponent(OtherScript)
		//GameObject.Find ("test");
		//GameObject.FindWithTag("test");
		
		
	}
	
	// Update is called once per frame
	void Update () {

        KeyUpdate();
		DrawFps fps = gameObject.GetComponent("DrawFps") as DrawFps;
		if( fps  ) 
		{
			string str = "asdfasdf";
			fps.SetText(str);
			
		}
		//Layer.SetText()		
	}

    public void KeyUpdate()
    {
        string[] str = Input.GetJoystickNames(); 
                
        
        if (Input.GetKeyUp("1"))
        {
        //    WorldManager WorldMgr = (WorldManager)GameObject.Find("WorldManager").GetComponent<WorldManager>();
        //   WorldMgr.m_CurMisson = MopleClient.Instance.GetMissionTotalCount();;
        //   WorldMgr.SetMissionData();
        }
        else if (Input.GetKeyUp("2"))
        {

            GameObject Cameraobj = GameObject.Find("Main Camera");
            ShakeCamera Shake = Cameraobj.GetComponent<ShakeCamera>();

            if (Cameraobj && Shake)
            {
                Debug.Log("ShakeShake");
                Shake.enabled = true;
                Shake.InitShake();

                //     Shake.StartLeftRightShake(9.0f);
                //   Shake.StartUpDownShake(9.0f);
            }


            //     ÃÑŸË ¶Ôž®ŽÂ°Å Å×œºÆ® 
            //     WorldManager WorldMgr = (WorldManager)GameObject.Find("WorldManager").GetComponent<WorldManager>();
            //     WorldMgr.CreateOnesBulletholes();
            //     m_MyShip.SetItemPower();


        }
        else if (Input.GetKeyUp("3"))
        {
       //         MopleClient.Instance.GetPlayer().Booster();
    //        m_MyShip.SetItemShield();
        }
        else if (Input.GetKeyUp("4"))
        {
        }

		
		   /*
        if(Input.GetKeyUp("1"))
        {
            CameraCnt++;
            
            if(CameraCnt > 3 ) 
            {
                CameraCnt = 0 ; 
            }

        }
		*/
		
        
    }
}
