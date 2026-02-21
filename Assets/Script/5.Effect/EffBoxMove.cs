using UnityEngine;
using System.Collections;


public class EffBoxMove : MonoBehaviour {

    Vector3 pos = Vector3.zero;
    Vector3 Rot = Vector3.zero; 

    public float NearWall = 0.0f;
    public float fSpeed = 50.0f;

    int RotMode = 0;
    public float fAngleSpeed = 5.0f; 

	// Use this for initialization
	void Start () {
        RotMode = Random.Range(0, 4);
	}
    
	// Update is called once per frame
	void Update () {

        pos = transform.localPosition;

        pos += Vector3.back * (Time.fixedDeltaTime * fSpeed );
        
        if (pos.z < 0 )
        {
            pos = new Vector3(Random.Range(-7f, 7f), Random.Range(-7f, 15f), 30) ;
            RotMode = Random.Range(0, 4);
        }
        
        transform.localPosition = pos;
        
        switch (RotMode)
        {
            case 0: transform.Rotate(Vector3.up * fAngleSpeed); break;
            case 1: transform.Rotate(Vector3.right * fAngleSpeed); break;
            case 2: transform.Rotate(Vector3.forward * fAngleSpeed); break;
            case 3: transform.Rotate(Vector3.back * fAngleSpeed); break;
            case 4: transform.Rotate(Vector3.left * fAngleSpeed ); break;

        }

	}
}
