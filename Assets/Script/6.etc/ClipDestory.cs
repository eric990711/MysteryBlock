using UnityEngine;
using System.Collections;

public class ClipDestory : MonoBehaviour
{

	// Use this for initialization
    
	void Start () {

        AudioSource src = transform.GetComponent<AudioSource>();

        if (src)
        {
            DestroyObject(gameObject , src.clip.length);
            Debug.Log("DestroyObject");
        }
	}
    
}
