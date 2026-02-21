using UnityEngine;
using System.Collections;

public enum MachineGrade
{ 
    _01,
    _02,
    _03,
}

public class MachinePoint : MonoBehaviour {

    public MachineGrade mGrade; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

    void OnEnable()
    {
        Debug.Log("What What What ~~~");

        //transform.GetComponent<UILabel>().text = GameClient.instance.mMachineGrade[(int)mGrade].ToString();
    }

}
