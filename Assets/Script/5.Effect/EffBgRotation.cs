using UnityEngine;
using System.Collections;

public class EffBgRotation : MonoBehaviour {
	
	
	Transform[] SubTransform;
	
	float fSpeed = 90.0f; 
	bool bRotate = false; //비용이 비싸서 처리한다. 
	
	// Use this for initialization
	void Start () 
	{
		SubTransform = GetComponentsInChildren<Transform>();

        for (int i = 1; i < SubTransform.Length; ++i)
		{
			//SubTransform[i].Rotate(Vector3.right * 2f);
			Quaternion rot = SubTransform[i].rotation;
			SubTransform[i].rotation = Quaternion.Euler( 270 , rot.eulerAngles.y  , rot.eulerAngles.z );
		}

        gameObject.GetComponent<TweenRotation>().duration = 96;
        gameObject.GetComponent<TweenRotation>().from.y = -80f;
        gameObject.GetComponent<TweenRotation>().to.y = 80f;
	}
	
	// Update is called once per frame
	void Update ()
	{
        
// 		if( SubTransform.Length == 0)
// 			return;
// 
//         if (GameClient.instance.mMysteryState == MysteryState.Result && bRotate == true)
//             bRotate = false;
// 
//         if (bRotate == true)
// 			return;

        if (GameClient.instance.mMysteryState == MysteryState.InGame && bRotate == false)
        {
            if (SubTransform[1].eulerAngles.x >= 265 && SubTransform[1].eulerAngles.x <= 280)
            {
                for (int i = 1; i < SubTransform.Length; ++i)
                    SubTransform[i].rotation = Quaternion.Euler(270, SubTransform[i].eulerAngles.y, SubTransform[i].eulerAngles.z);

                bRotate = true;
                return;
            }
        }


        for (int i = 1; i < SubTransform.Length; ++i)
        {
            SubTransform[i].Rotate(Vector3.right * (fSpeed * Time.deltaTime));

            //SubTransform[i].rotation;
            //Quaternion.Lerp (from.rotation, to.rotation, Time.time * speed);
        }
        

        
	}
}














