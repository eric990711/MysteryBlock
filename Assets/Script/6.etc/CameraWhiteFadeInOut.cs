using UnityEngine;
using System.Collections;



[AddComponentMenu("JI/Camera/Fade/White fade")]
public class CameraWhiteFadeInOut : MonoBehaviour {

    // ---------------------------------------- 
    // 	PUBLIC FIELDS
    // ----------------------------------------
    public string m_MainCameraName;

    // Texture used for fading
    public Texture2D fadeTextureWhite = null;


    // Default time a fade takes in seconds
    public float fadeDuration = 2;

    // Depth of the gui element
    public int guiDepth = -1000;

    private float fadeSpeed = 0.3f;
    public ACTIVESTATE3 fadeInState { get; set; }
    public ACTIVESTATE3 fadeOutState { get; set; }
   




    // ---------------------------------------- 
    // 	PRIVATE FIELDS
    // ----------------------------------------
    private bool Fade = false;
    private bool FadeInStart = false;
    private bool FadeOutStart = false;

    // Current alpha of the texture
    private float currentAlpha = 1;
    public float CurrentAlpha
    {
        get { return currentAlpha; }
        set { currentAlpha = value; }
    }

    // Current duration of the fade
    private float currentDuration;

    // Direction of the fade
    private int fadeDirection = -1;

    // Fade alpha to
    private float targetAlpha = 0;

    // Alpha difference
    private float alphaDifference = 0;

    // Style for background tiling
    private GUIStyle backgroundStyle = new GUIStyle();
    private Texture2D dummyTex;
    private bool hide { get; set; }


    // Color object for alpha setting
    Color alphaColor = new Color();

    // ---------------------------------------- 
    // 	FADE METHODS
    // ----------------------------------------

    public void FadeIn(float duration, float to)
    {
        fadeInState = ACTIVESTATE3.RUN;
		
		currentAlpha = 1f;
        Fade = true;
        FadeInStart = true;
        hide = false;

        // Set fade duration
        currentDuration = duration;
        // Set target alpha
        targetAlpha = to;
        // Difference
        alphaDifference = Mathf.Clamp01(currentAlpha - targetAlpha);
        // Set direction to Fade in
        fadeDirection = -1;
    }

    public void FadeIn()
    {
        FadeIn(fadeDuration, 0);
    }

    public void FadeIn(float duration)
    {
        FadeIn(duration, 0);
    }

    public void FadeOut(float duration, float to)
    {
        fadeOutState = ACTIVESTATE3.RUN;
		
		currentAlpha = 0f;
        Fade = true;
        FadeOutStart = true;
        hide = false;

        // Set fade duration
        currentDuration = duration;
        // Set target alpha
        targetAlpha = to;
        // Difference
        alphaDifference = Mathf.Clamp01(targetAlpha - currentAlpha);
        // Set direction to fade out
        fadeDirection = 1;
    }

    public void FadeOut()
    {
        FadeOut(fadeDuration, 1);
    }

    public void FadeOut(float duration)
    {
        FadeOut(duration, 1);
    }

    // ---------------------------------------- 
    // 	STATIC FADING FOR MAIN CAMERA
    // ----------------------------------------

    public static void FadeInMain(float duration, float to)
    {
        GetInstance().FadeIn(duration, to);
    }

    public static void FadeInMain()
    {
        GetInstance().FadeIn();
    }

    public static void FadeInMain(float duration)
    {
        GetInstance().FadeIn(duration);
    }

    public static void FadeOutMain(float duration, float to)
    {
        GetInstance().FadeOut(duration, to);
    }

    public static void FadeOutMain()
    {
        GetInstance().FadeOut();
    }

    public static void FadeOutMain(float duration)
    {
        GetInstance().FadeOut(duration);
    }

    // Get script fom Camera
    public static CameraWhiteFadeInOut GetInstance()
    {
        // Get Script
        CameraWhiteFadeInOut fader = (CameraWhiteFadeInOut)Camera.main.GetComponent("CameraWhiteFadeInOut");
        // Check if script exists
        if (fader == null)
        {
            Debug.LogWarning("No FadeInOut attached to the main camera.");
        }
        return fader;
    }

    // ---------------------------------------- 
    // 	SCENE FADEIN
    // ----------------------------------------

    public void Awake()
    {
        Fade = false;

        fadeInState = ACTIVESTATE3.READY;
        fadeOutState = ACTIVESTATE3.READY;

        hide = true;
        dummyTex = new Texture2D(1, 1);
        dummyTex.SetPixel(0, 0, Color.clear);

        backgroundStyle.normal.background = fadeTextureWhite;

        //if (fadeIntoScene)
        //{
        //    FadeIn();
        //}
        //else
        //{
        //    DBInputController.Instatnce.isEndIntroFadeIn = true;
        //}
    }

    // ---------------------------------------- 
    // 	FADING METHOD
    // ----------------------------------------

    public void OnGUI()
    {
        if (hide)
        {
            //			FadeIn(0.5f);

            // Draw texture at depth
            alphaColor = GUI.color;
            alphaColor.a = 0;
            currentAlpha = 0;
            GUI.color = alphaColor;
            GUI.depth = guiDepth;
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex, backgroundStyle);
            //GUI.DrawTexture(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex);

            //GetComponent<CameraFadeInOut>().enabled = false;

            //GetComponent<CameraFadeInOut>().enabled = false;

            return;
        }//hide

        //if (hide) return;

        // Fade alpha if active
        //        if ((fadeDirection == -1 && currentAlpha > targetAlpha) ||
        //            (fadeDirection == 1 && currentAlpha < targetAlpha))
        if (Fade)
        {
            // Advance fade by fraction of full fade time
            currentAlpha += fadeDirection * (Time.deltaTime / currentDuration);
            //currentAlpha += (fadeDirection * alphaDifference) * (Time.deltaTime / currentDuration);
            //currentAlpha += fadeDirection * fadeSpeed * Time.deltaTime;	
            // Clamp to 0-1
            currentAlpha = Mathf.Clamp01(currentAlpha);

        }

        // Draw only if not transculent
        if (currentAlpha > 0)
        {
            // Draw texture at depth
            alphaColor = GUI.color;
            alphaColor.a = currentAlpha;
            GUI.color = alphaColor;
            GUI.depth = guiDepth;
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex, backgroundStyle);
            //GUI.DrawTexture(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex);
        }

        if (currentAlpha == 0f)
        {
            switch (fadeInState)
            {
                case ACTIVESTATE3.RUN:
                    Fade = false;
                    fadeInState = ACTIVESTATE3.END;
                break;
            }
        }

        if (currentAlpha == 1f)
        {
            switch (fadeOutState)
            {
                case ACTIVESTATE3.RUN:
                    Fade = false;
                    fadeOutState = ACTIVESTATE3.END;
                break;
            }

            Camera camera = (Camera)GameObject.Find(m_MainCameraName).GetComponent<Camera>();
            camera.backgroundColor = Color.white;

            //if (IntroController.Instance.fadeOutStart)
            //{
            //    IntroController.Instance.fadeOutStart = false;
            //    IntroController.Instance.fadeOutEnd = true;
            //}

            //if (IntroController.Instance.movieFadeOutStart)
            //{
            //    IntroController.Instance.movieFadeOutStart = false;
            //    IntroController.Instance.movieFadeOutEnd = true;
            //}

            hide = true;

        }

        
    }



    public void Update()
    {
        //if (fadeIntoScene && currentAlpha == 0)
        //{
        //    DBInputController.Instatnce.isEndIntroFadeIn = true;
        //}
    }

    public void Hide()
    {
#if DIBO_DEBUG
        Debug.Log("Fade Hide");
#endif
        hide = true;

        //dummyTex = new Texture2D(1, 1);
        //dummyTex.SetPixel(0, 0, Color.clear);
        currentAlpha = 0;

        //		FadeIn ();
    }//Hide


    //public void ChangeFadecolor(FADECOLOR color)
    //{
    //    fadeColor = color;
    //    switch (color)
    //    {
    //        case FADECOLOR.WHITE:
    //            backgroundStyle.normal.background = fadeTextureWhite;
    //            break;

    //        case FADECOLOR.BLAKC:
    //            backgroundStyle.normal.background = fadeTextureBlack;
    //            break;

    //        case FADECOLOR.DEFAULT:
    //            backgroundStyle.normal.background = fadeTextureBlack;
    //            break;
    //    }
    //}

}//class