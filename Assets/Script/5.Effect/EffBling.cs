using UnityEngine;
using System.Collections;

public class EffBling : MonoBehaviour {

    public Material[] mat = null; 


	// Use this for initialization
	void Start () {

        StartCoroutine(ChangeMat());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator ChangeMat()
    {
        int i = 0; 
        while (true)
        {
            renderer.material = mat[i];            
            yield return new WaitForSeconds(0.5f);
            i++;
            if (i > 1) i = 0; 
        }
    }

}
