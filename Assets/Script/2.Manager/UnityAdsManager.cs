using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Unity Ads 광고 매니저 (Android 전용)
/// 게임 오버 시 인터스티셜(전면) 광고를 표시합니다.
///
/// Android Game ID: 6050441
/// 씬의 빈 GameObject에 이 스크립트를 붙이거나, DontDestroyOnLoad 오브젝트에 추가하세요.
/// </summary>
public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    // Unity Dashboard Game ID
    private const string GAME_ID_ANDROID = "6050441";
    private const string GAME_ID_IOS = "YOUR_IOS_GAME_ID"; // iOS 미사용

    // 광고 Unit ID (Unity Dashboard 기본값)
    private const string INTERSTITIAL_AD_UNIT = "Interstitial_Android";
    private const string INTERSTITIAL_AD_UNIT_IOS = "Interstitial_iOS";

    private static UnityAdsManager mInstance = null;
    public static UnityAdsManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<UnityAdsManager>();
                if (mInstance == null)
                {
                    GameObject go = new GameObject("UnityAdsManager");
                    mInstance = go.AddComponent<UnityAdsManager>();
                }
            }
            return mInstance;
        }
    }

    private bool isAdLoaded = false;
    private bool isInitialized = false;

    // 광고 표시 후 호출될 콜백
    private System.Action onAdCompleted = null;

    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else if (mInstance != this)
        {
            Destroy(gameObject);
        }
    }

    void InitializeAds()
    {
        string gameId = "";
#if UNITY_ANDROID
        gameId = GAME_ID_ANDROID;
#elif UNITY_IOS
        gameId = GAME_ID_IOS;
#else
        gameId = GAME_ID_ANDROID;
#endif

        if (string.IsNullOrEmpty(gameId) || gameId.Contains("YOUR_"))
        {
            Debug.LogWarning("[UnityAds] Game ID가 설정되지 않았습니다. Unity Dashboard에서 Game ID를 발급받아 설정하세요.");
            return;
        }

        // testMode: 개발 중에는 true, 출시 시 false로 변경
        Advertisement.Initialize(gameId, testMode: false, this);
    }

    // 광고 초기화 완료 콜백
    public void OnInitializationComplete()
    {
        Debug.Log("[UnityAds] 초기화 완료");
        isInitialized = true;
        LoadInterstitialAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"[UnityAds] 초기화 실패: {error} - {message}");
    }

    /// <summary>
    /// 인터스티셜 광고를 미리 로드합니다.
    /// </summary>
    public void LoadInterstitialAd()
    {
        if (!isInitialized) return;

        string adUnit = GetAdUnitId();
        Debug.Log($"[UnityAds] 광고 로드 시작: {adUnit}");
        Advertisement.Load(adUnit, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"[UnityAds] 광고 로드 완료: {adUnitId}");
        isAdLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"[UnityAds] 광고 로드 실패: {error} - {message}");
        isAdLoaded = false;
    }

    /// <summary>
    /// 게임 오버 시 인터스티셜 광고를 표시합니다.
    /// </summary>
    /// <param name="onComplete">광고 종료 후 실행할 콜백 (null 가능)</param>
    public void ShowInterstitialAd(System.Action onComplete = null)
    {
        onAdCompleted = onComplete;

        if (!isInitialized)
        {
            Debug.LogWarning("[UnityAds] 아직 초기화되지 않았습니다.");
            onAdCompleted?.Invoke();
            onAdCompleted = null;
            return;
        }

        if (!isAdLoaded)
        {
            Debug.LogWarning("[UnityAds] 광고가 로드되지 않았습니다. 다음에 다시 시도합니다.");
            onAdCompleted?.Invoke();
            onAdCompleted = null;
            LoadInterstitialAd(); // 다음을 위해 미리 로드
            return;
        }

        string adUnit = GetAdUnitId();
        isAdLoaded = false;
        Advertisement.Show(adUnit, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"[UnityAds] 광고 표시 완료: {showCompletionState}");
        onAdCompleted?.Invoke();
        onAdCompleted = null;
        // 다음 광고를 미리 로드
        LoadInterstitialAd();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"[UnityAds] 광고 표시 실패: {error} - {message}");
        onAdCompleted?.Invoke();
        onAdCompleted = null;
        LoadInterstitialAd();
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log($"[UnityAds] 광고 시작: {adUnitId}");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log($"[UnityAds] 광고 클릭: {adUnitId}");
    }

    private string GetAdUnitId()
    {
#if UNITY_IOS
        return INTERSTITIAL_AD_UNIT_IOS;
#else
        return INTERSTITIAL_AD_UNIT;
#endif
    }
}
