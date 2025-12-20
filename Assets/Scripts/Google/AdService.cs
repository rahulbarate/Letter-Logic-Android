using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class AdService : MonoBehaviour
{
    // singleton instance
    public static AdService Instance { get; private set; }

    private string _bannerUnitId = "ca-app-pub-3940256099942544/6300978111";

    private BannerView _bannerView;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void OnDestroy()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }

    void Start()
    {
        MobileAds.Initialize((InitializationStatus status) =>
        {
            CustomLogger.Log("Google mobile Ads initialized");
            HandleBannerForScene(SceneManager.GetActiveScene().buildIndex);
            // CreateBannerAd(AdPosition.Bottom);

        });
    }

    void HandleBannerForScene(int index)
    {

        CustomLogger.Log("Handling banners for scene index " + index);
        if (index == 0)
        {
            HideBannerAd();
            CreateBannerAd(AdPosition.Bottom);
        }
        else if (index > 0)
        {
            HideBannerAd();
            CreateBannerAd(AdPosition.Top);
        }
        // else
        // {
        //     HideBannerAd();
        // }
    }

    void CreateBannerAd(AdPosition position)
    {
        CustomLogger.Log("Creating banner ad");
        if (_bannerView != null)
            _bannerView.Destroy();
        AdSize adSize = AdSize.Banner;
        _bannerView = new BannerView(_bannerUnitId, adSize, position);
        _bannerView.OnBannerAdLoaded += () => CustomLogger.Log("Banner Ad Loaded");
        _bannerView.OnBannerAdLoadFailed += (err) => CustomLogger.Log("Banner Ad failed to load: " + err.GetMessage());
        _bannerView.LoadAd(new AdRequest());
    }

    void HideBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    void OnSceneChanged(Scene currnet, Scene next)
    {
        HandleBannerForScene(next.buildIndex);
    }

}
