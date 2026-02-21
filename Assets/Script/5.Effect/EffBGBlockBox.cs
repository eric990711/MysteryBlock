using UnityEngine;
using System.Collections;

//게임이 끝난고 뒤에서 박스그 무쟈게 나타나는 박스

public class EffBGBlockBox : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {

        Debug.Log("Eff Move Box");
        Vector3 pos = transform.position;
        float Height = pos.y/2;
        
        for(int i = 0 ; i < 100 ; i ++ )
        {
            GameObject go = Instantiate(GameClient.instance.mEffBlockBox, 
                new Vector3(Random.Range(-7f, 7f), 
                    pos.y + Random.Range(-Height, Height), 30), Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            yield return null;
        }
       
	}
	
	// Update is called once per frame
	void Update () {
	

        


	}
}
