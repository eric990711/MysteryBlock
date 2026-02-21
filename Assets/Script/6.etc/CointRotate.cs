using UnityEngine;
using System.Collections;

public class CointRotate : MonoBehaviour {

    public float _fRotateSpeed = 150.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0,0,_fRotateSpeed * Time.deltaTime));
	}
}
