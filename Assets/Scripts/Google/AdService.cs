using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class AdService : MonoBehaviour
{
    // singleton instance
    public static AdService Instance { get; private set; }

    // event for reward callback
    public static event Action E_RewardedAdCompleted;

    private string _bannerUnitId = "ca-app-pub-3940256099942544/6300978111";
    private string _rewardedUnitId = "ca-app-pub-3940256099942544/5224354917";

    private string _interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";


    private BannerView _bannerView;
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;

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
            LoadRewardedAd();
            LoadInterstitialAd();
            HandleBannerForScene(SceneManager.GetActiveScene().buildIndex);
            // CreateBannerAd(AdPosition.Bottom);

        });
    }

    private void LoadInterstitialAd()
    {
        CustomLogger.Log("Loading Interstitial ad");
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        InterstitialAd.Load(_interstitialUnitId, new AdRequest(), (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                CustomLogger.Log("Failed to load interstitial ad with error" + error?.GetMessage());
            }
            CustomLogger.Log("Interstitial Ad loaded successfully");
            _interstitialAd = ad;
            RegisterInterstitialAdEventHandlers(ad);
        });
    }

    private void RegisterInterstitialAdEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () => CustomLogger.Log("Interstial Ad opened");
        ad.OnAdFullScreenContentFailed += (err) => CustomLogger.Log("Failed to show ad with error: " + err.GetMessage());
        ad.OnAdFullScreenContentClosed += () =>
        {
            CustomLogger.Log("Interstitial Ad Closed");
            LoadInterstitialAd();
        };
    }

    public bool IsInterstitialAdReady()
    {
        if (_interstitialAd == null || !_interstitialAd.CanShowAd())
            return false;
        else
            return true;

    }
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            CustomLogger.LogError("Interstitial Ad not ready to show");
        }
    }
    private void LoadRewardedAd()
    {
        CustomLogger.Log("Loading rewarded ad.");
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        RewardedAd.Load(_rewardedUnitId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                CustomLogger.LogError("Failed to load rewarded ad with error: " + error?.GetMessage());
            }
            CustomLogger.Log("Rewarded ad loaded successfully");
            _rewardedAd = ad;
            RegisterRewardedAdEventHandlers(ad);
        });
    }
    private void RegisterRewardedAdEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentOpened += () => CustomLogger.Log("Rewarded ad opened.");
        ad.OnAdFullScreenContentFailed += (err) =>
        {
            CustomLogger.Log("Failed to show ad with error: " + err.GetMessage());
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            CustomLogger.Log("Rewarded Ad closed");
            E_RewardedAdCompleted?.Invoke();
            LoadRewardedAd();

        };
    }
    public bool IsRewardedAdReady()
    {
        if (_rewardedAd == null || !_rewardedAd.CanShowAd())
            return false;
        else
            return true;
    }
    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) => { CustomLogger.Log($"User earned reward: {reward.Amount} {reward.Type}"); });
        }
        else
        {
            CustomLogger.LogError("AdService: Rewarded ad is not ready to be shown.");
        }
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
