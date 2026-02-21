using UnityEngine;
using System.Collections;

public class TicketPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Animation>().Play("Run");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
