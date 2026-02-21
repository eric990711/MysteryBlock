using UnityEngine;
using System.Collections;


public class Eff_Texture_Ani : MonoBehaviour {

    public enum eStyle{
        Once,
        Loop,
    }

    public eStyle mStyle = eStyle.Once;
    public string mResourcePath; 
    
    
    Object[] mTexture;
    public float fDuration = 0.01f;
    public float mSize = 3.0f; 

    float fNowTime = 0.0f;
    int index = 0;

    UITexture pTexture = null; 
    
	// Use this for initialization
    void Awake()
    {
        mTexture = Resources.LoadAll( mResourcePath , typeof(Texture));

       // Debug.Log("mTexture " + mTexture.Length);
        
    }

    void Reset()
    {
        index = 0;
        fNowTime = 0.0f; 
    }

	void Start () {
        pTexture = GetComponent<UITexture>();
        transform.localScale = new Vector3(mSize, mSize , 0f);
	}
        
    void OnEnable()
    {
        fNowTime = 0.0f; 
    }
    
	// Update is called once per frame
	void Update () 
    {
	
        fNowTime += Time.deltaTime; 
        if( fNowTime >= fDuration )
        {
            fNowTime = 0.0f; 

            index++;

            if (index >= mTexture.Length)
            {
                if (mStyle == eStyle.Once)
                {
                    index = mTexture.Length;
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    index = 0; 
                }
            }

            pTexture.mainTexture = (Texture)mTexture[index];
        }
	}
}
