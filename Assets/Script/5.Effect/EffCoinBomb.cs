using UnityEngine;
using System.Collections;

public class EffCoinBomb : MonoBehaviour {

    public GameObject BombEffect; 

    UISpriteAnimation CoinSpriteAni = null;
    UISpriteAnimation BombEffectAni = null;

	// Use this for initialization
	void Start () {
        CoinSpriteAni = GetComponent<UISpriteAnimation>();
        BombEffectAni = BombEffect.GetComponent<UISpriteAnimation>();

	}
	
	// Update is called once per frame

    void OnEnable()
    {
        if (CoinSpriteAni != null)
            CoinSpriteAni.Reset();
       
    }
    
	void Update () 
    {
        
        if (CoinSpriteAni == null) 
            return;
        
        if (CoinSpriteAni.isPlaying == false)
        {
            gameObject.SetActive(false); 
            BombEffect.SetActive(true);
            BombEffectAni.Reset();
            
        }
	}
}
