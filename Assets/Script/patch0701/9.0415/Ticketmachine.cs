using UnityEngine;
using System.Collections;

public class Ticketmachine : MonoBehaviour {

	// Use this for initialization
    
	void Start () {
        GameClient.instance.StopBGM();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnEnable()
    {
        if (MysteryMgr.Instance.AcumulSumScore > 0)
        {
            TweenRotation rot = transform.Find("Ticketing/TicketAnim").GetComponent<TweenRotation>();
            rot.Reset();
            rot.enabled = true;

            GameClient.OneShotSound(transform.position, GameClient.instance.mSnd_Ticketing);

            if (GameClient.mNetwork == true)
            {
                CNetwork.GetInstance().CS_SaveTicket();
            }
        }
    }

}
