using UnityEngine;
using System.Collections;

public class LoadingBar : MonoBehaviour {

	// Use this for initialization
    public bool isLock = false;

    public GameObject loadingPanel;

    public static LoadingBar m_instance;
    
    public static LoadingBar GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = FindObjectOfType<LoadingBar>();
        }
        return m_instance;
    }
    

	void Start () {

	}
       

    public void SetLock(bool bflag)
    {

        Debug.Log("Loading SetLock" + bflag);
        
        loadingPanel.SetActive(bflag);
        isLock = bflag;
    }
    

    public bool GetLock()
    {
        return isLock;
    }


}
