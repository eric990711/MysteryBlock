using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//by sdh 팝업 캐시 한번 불러온 이미지는 저장하여 재활용한다. 
public class PopupCacheData
{
    public int num; //팝업 고유번호
    public Texture2D texture;

    public PopupCacheData(int _num, Texture2D _texture)
    {
        num = _num;
        texture = _texture;
    }
}


public class CPopupCache : MonoBehaviour
{

    public List<PopupCacheData> mPopupCacheList = new List<PopupCacheData>();

    public void AddData(Texture2D texture)
    {

        int num = CPopupMgr.instance.CurPopupInfo.num;

        //같은게 이미 있으면 리턴 시킴
        foreach (var obj in mPopupCacheList)
        {
            if (obj.num == num)
                return;
        }

        mPopupCacheList.Add(new PopupCacheData(num, texture));

    }

    public Texture2D GetData()
    {
        int num = CPopupMgr.instance.CurPopupInfo.num;

        foreach (var obj in mPopupCacheList)
        {
            if (obj.num == num)
                return obj.texture;
        }

        return null;
    }

    void Awake()
    {
        mPopupCacheList.Clear();

        DontDestroyOnLoad(this);
    }



    // Use this for initialization
    void Start()
    {

    }


    private static CPopupCache minstance = null;
    //처음 로비로 접속

    public static CPopupCache instance
    {
        get
        {
            if (minstance == null)
            {
                minstance = FindObjectOfType<CPopupCache>();

                if (minstance == null)
                {
                    minstance = new GameObject("CPopupCache").AddComponent<CPopupCache>();
                }
            }
            return minstance;
        }
    }

}
