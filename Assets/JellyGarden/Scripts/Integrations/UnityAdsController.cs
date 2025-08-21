using UnityEngine;
using UnityEngine.Advertisements;

#if UNITY_ADS
namespace JellyGarden.Scripts.Integrations
{
    public class UnityAdsController : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener,
        IUnityAdsShowListener
    {
        #region UnityAdsController

        public static UnityAdsController Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        public void InitAds(string androidID,string iOSID)
        {
#if UNITY_ANDROID
            Advertisement.Initialize(androidID, false, this);
#elif UNITY_IOS
            Advertisement.Initialize(iOSID, false, this);
#endif
        }

        public void ShowAds(string loadAdType)
        {
            Advertisement.Show(loadAdType, this);
        }

        public void LoadUnityRewardedAd()
        {
            string videoAdsZone = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? InitScript.Instance.unityAds.unityRewardedIOS
                : InitScript.Instance.unityAds.unityRewardedAndroid;
            Advertisement.Load(videoAdsZone, this);
        }

        public void LoadUnityInterstitialAd()
        {
            string interstitialAdsZone =
                (Application.platform == RuntimePlatform.IPhonePlayer)
                    ? InitScript.Instance.unityAds.unityInterstitialIOS
                    : InitScript.Instance.unityAds.unityInterstitialAndroid;
            Advertisement.Load(interstitialAdsZone, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("OnInitializationComplete!");
            LoadUnityRewardedAd();
            LoadUnityInterstitialAd();
        }

        [HideInInspector]
        public bool isLoaded;

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            isLoaded = false;
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            isLoaded = true;
            Debug.Log("OnUnityAdsAdLoaded!  placementId = " + placementId);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            isLoaded = false;
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            isLoaded = false;
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("OnUnityAdsShowStart!  placementId = " + placementId);
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log("OnUnityAdsShowClick!  placementId = " + placementId);
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            isLoaded = false;
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("OnUnityAdsShowComplete!  placementId = " + placementId);
                InitScript.Instance.CheckRewardedAds();
            }
        }

        #endregion
    }
}
#endif