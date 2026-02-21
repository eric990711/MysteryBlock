using UnityEngine;
using System.Collections;

public class WinZoneArea : MonoBehaviour {

	// Use this for initialization
	
    void OnTriggerStay(Collider other)
    {

        

        if (other.gameObject.tag == "BlockBox")
        {
            if (GameClient.instance.isVisitor == false)
            {
                if (MysteryMgr.Instance.isMysteryEff == true) return;
            }
            

            BaseBlock block = other.gameObject.GetComponent<BaseBlock>();
            
            if (block.bCheckStayBlock  && block.bCollision == false)
            {
                //Step Increase
                MysteryMgr.Instance.mBlockStep++;

                if (MysteryMgr.Instance.mBlockStep > 4)
                    MysteryMgr.Instance.mBlockStep = 4;

                Vector3 pos = other.gameObject.transform.position;

                pos.z -= 1.0f;

                MysteryMgr.Instance.Eff_Bomb(pos);
                
                Debug.Log("WinZone BlockStep" + MysteryMgr.Instance.mBlockStep);

                


                block.bCollision = true; 
            }
        }

    }

}
