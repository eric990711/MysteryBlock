using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum IntroStoryBoard
{
    None = 0,
    Black,
    SplashImage,
    LoaingImage,
    LoadingMainMenu,
}

public class IntroController : MonoBehaviour {

    ////----------------------------------------------------------------------------------------------------------------------------------------------------
    //// public
    ////----------------------------------------------------------------------------------------------------------------------------------------------------
    public static IntroController Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<IntroController>();
                if (m_instance == null)
                {
                    m_instance = new GameObject("IntroController").AddComponent<IntroController>();
                }
            }
            return m_instance;
        }
    }
    public GameObject UI_RootObj = null;

    public GameObject m_Black = null;
    public GameObject m_SplashImage = null;
    public GameObject m_LoadingImage = null;

    public string m_nextLevelName = "here name";

	

    ////----------------------------------------------------------------------------------------------------------------------------------------------------
    //// private
    ////----------------------------------------------------------------------------------------------------------------------------------------------------
    private static IntroController  m_instance = null;
    private IntroStoryBoard         m_storyBoard = IntroStoryBoard.Black;

    private float m_tElapTime = 0.0f;
    private float m_tMainImageElapTime = 0.0f;
    private bool m_bFadeOncer = true;
    private bool m_loadingVillageSceneOncer = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_storyBoard == IntroStoryBoard.Black)
        {
            m_tElapTime += Time.deltaTime;
            if (m_tElapTime > 0.5)
            {
                //GameObject.Find(m_UI_RootName).transform.Find("Camera/Anchor/Panel/Black").gameObject.SetActive(false);
                //GameObject.Find(m_UI_RootName).transform.Find("Camera/Anchor/Panel/SplashImage").gameObject.SetActive(true);
                m_Black.SetActive(false);
                m_SplashImage.SetActive(true);
                m_storyBoard = IntroStoryBoard.SplashImage;
                m_tElapTime = 0.0f;
            }
        }

        if (m_storyBoard == IntroStoryBoard.SplashImage)
        {
            m_tElapTime += Time.deltaTime;
            if (m_tElapTime > 2.0f)
            {
                //GameObject.Find(m_UI_RootName).transform.Find("Camera/Anchor/Panel/SplashImage").gameObject.SetActive(false);
                //GameObject.Find(m_UI_RootName).transform.Find("Camera/Anchor/Panel/IntroImage").gameObject.SetActive(true);
                m_SplashImage.SetActive(false);
                m_LoadingImage.SetActive(true);
                m_storyBoard = IntroStoryBoard.LoaingImage;
                m_tElapTime = 0.0f;
            }
        }

        if (m_storyBoard == IntroStoryBoard.LoaingImage)
        {
            if (m_bFadeOncer)
            {
                UI_RootObj.transform.Find("Camera").gameObject.GetComponent<CameraWhiteFadeInOut>().FadeIn();
                m_bFadeOncer = false;
                m_storyBoard = IntroStoryBoard.LoadingMainMenu;
            }
        }

        if (m_storyBoard == IntroStoryBoard.LoadingMainMenu)
        {
            m_tMainImageElapTime += Time.deltaTime;

            if (m_tMainImageElapTime > 3f)
            {
                if (m_loadingVillageSceneOncer)
                {
                    SceneManager.LoadScene(m_nextLevelName);
                    m_loadingVillageSceneOncer = false;
                }
            }
        }
	}
}
