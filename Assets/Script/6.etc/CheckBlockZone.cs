using UnityEngine;
using System.Collections;

public class CheckBlockZone : MonoBehaviour
{


	void OnTriggerEnter(Collider col)
	{
        MysteryMgr.Instance.DeleteObject(col.gameObject);
		MysteryMgr.Instance.DropBlockProc();
        Destroy(col.gameObject);

	}
	
}
