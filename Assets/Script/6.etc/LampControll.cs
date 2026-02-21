using UnityEngine;
using System.Collections;

public class LampControll : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//transform.RotateAround( Vector3.zero ,  Vector3.up  ,  Time.deltaTime*100.0f );
		transform.RotateAroundLocal(Vector3.up , Time.deltaTime*10.0f);
		
		
		
	}
}
