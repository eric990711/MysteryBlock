using UnityEngine;
using System.Collections;

public class UVSprite : MonoBehaviour {

    private Material    _material;
    private Vector3     _offset;

    void Start()
    {
        _material = gameObject.renderer.material;

        _offset = _material.GetTextureOffset("_MainTex");
    }

	
	// Update is called once per frame
	void Update () 
    {
        _offset.x += (Time.deltaTime *0.1f);
        _offset.y += (Time.deltaTime *0.1f); 
        
        _material.SetTextureOffset("_MainTex", _offset);
	}
    
}

