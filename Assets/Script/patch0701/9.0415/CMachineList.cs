using UnityEngine;
using System.Collections;


//로비에서 머신에 잭팟이나 코인 값을 변경한다. 

public class CMachineList : MonoBehaviour {

	// Use this for initialization
    public Transform[] MachineObjs = new Transform[9];


    private static CMachineList minstance = null;
    //처음 로비로 접속
    public static CMachineList GetInstance()
    {
        if (minstance == null)
        {
            minstance = FindObjectOfType<CMachineList>();
        }
        return minstance;
    }
// 
// 	void Start () {
//         for(int i = 0 ; i < 9; i++)
//         {
//             int Num = i+1;
//             MachineObjs[i] = transform.Find("m0" + Num.ToString());
//             MachineData machinedata = GameClient.instance.GetMachineList(i);
//             //MachineObjs[i].Find("CoinNumber").GetComponent<UILabel>().text = machinedata.theNeedCoin.ToString();
//             //MachineObjs[i].Find("JacpotNumber").GetComponent<UILabel>().text = machinedata.theJackPot.ToString();
//             //MachineObjs[i].Find("JacpotNumber").GetComponent<>
//            // Debug.Log(machinedata.theNeedCoin); 
//             //machinedata.theJackPot;
//         }
// 	}
    

    public void SetBoxColl(bool bflag)
    {
        for(int i= 0; i < MachineObjs.Length ; i++)
        {
            MachineObjs[i].GetComponent<Collider>().enabled = bflag; 
        }
    }

}
