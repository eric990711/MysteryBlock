using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{


    public static SceneController Instance 
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SceneController>();
            }
		    return m_instance;
        }
    }
	
	//송동현 

	public GameObject TicketMachineObj;
	public GameObject MysteryObj;
	public GameObject UIMystery;
	public GameObject UIOption;

    /*
    public AudioClip mLobbyBGM = null;
    private AudioSource mLobbyBGMPlayer = null;

    public AudioClip mBGM = null;
    private AudioSource mBGMPlayer = null;
	*/
    
    private static SceneController m_instance = null;
	

	// Use this for initialization
	void Start () {
		/*
		mLobbyBGMPlayer = gameObject.AddComponent<AudioSource>();
        mLobbyBGMPlayer.clip = mLobbyBGM;
        mLobbyBGMPlayer.volume = 1.0f;
        mLobbyBGMPlayer.loop   = true;

        if(mLobbyBGMPlayer != null && mLobbyBGMPlayer.isPlaying == false)
        {
            mLobbyBGMPlayer.Play();
			Debug.Log("mLobbyBGMPlayer");
        }
        
		mBGMPlayer = gameObject.AddComponent<AudioSource>();
        mBGMPlayer.clip = mBGM;
        mBGMPlayer.volume = 1.0f;
        mBGMPlayer.loop   = true;
		*/

		
	}
		
	public void GoMysteryGame()
	{
    	
        MysteryObj.SetActive(true);
		UIMystery.SetActive(true);

        TicketMachineObj.transform.root.Find("LobbyCamera").GetComponent<AudioListener>().enabled = false;
        TicketMachineObj.SetActive(false);
	}
	
	public void MysteryGameBGMPlay()
	{

        /*
		if(mBGMPlayer != null && mBGMPlayer.isPlaying == false)
        {
            mBGMPlayer.Play();
        }
        */

    }

    // Update is called once per frame
	void Update () {
	}
}
