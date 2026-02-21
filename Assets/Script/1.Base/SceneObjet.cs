using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SceneObjet : MonoBehaviour {
	
	// Use this for initialization
	protected virtual void Start () {

    //    GameManager.Instance.AddObject(this);
        Debug.Log("GameManager.Instance.AddObject instant");
	}
		
	// Update is called once per frame
	virtual protected void Update () {
	
	}

    protected void DestroySceneObject()
    {
        Debug.Log("DestroySceneObject AddObject instant");
      //  GameManager.Instance.DeleteObject(this);
    }

}
