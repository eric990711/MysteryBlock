using UnityEngine;
using System.Collections;

public class EffectDestory : MonoBehaviour {

	// Use this for initialization
    ParticleSystem effect = null; 
	void Start () {
        effect = gameObject.GetComponent<ParticleSystem>();
	}
    	
	// Update is called once per frame
	void Update () {
        if( effect.isStopped )
        {
            Destroy(gameObject);
        }
	}
}
