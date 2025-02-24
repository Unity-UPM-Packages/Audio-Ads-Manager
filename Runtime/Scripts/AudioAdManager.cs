using System;
using System.Collections;
using System.Collections.Generic;
using Odeeo;
using TheLegends.Base.Firebase;
using TheLegends.Base.UnitySingleton;
using TheLegends.Odeeo;
using UnityEngine;

namespace TheLegends.Base.AudioAds
{
    public class AudioAdManager : PersistentMonoSingleton<AudioAdManager>
    {
        [SerializeField] private OdeoAdUnitId androidIds;
        [SerializeField] private OdeoAdUnitId iosIds;
        [SerializeField] private bool isTest = false;
        [SerializeField] private OdeeoSdk.LogLevel logLevel = OdeeoSdk.LogLevel.Debug;
        private OdeoAdUnitId unitIds = new OdeoAdUnitId();

        private IconPlacement _iconPlacement;
        private IconRewardPlacement _iconRewardPlacement;
        private BannerPlacement _bannerPlacement;
        private BannerRewardPlacement _bannerRewardPlacement;

        private void Awake()
        {
            SetUnitId();
        }

        private void SetUnitId()
        {
            if (isTest)
            {
#if UNITY_IOS
                unitIds.APP_KEY = "6736cfbf-dccc-4def-a022-16b6686639e2";
                unitIds.BANNER_PLACEMENT_ID = "334872774";
                unitIds.BANNER_REWARDED_PLACEMENT_ID = "385528236";
                unitIds.ICON_PLACEMENT_ID = "302988162";
                unitIds.ICON_REWARDED_PLACEMENT_ID = "397800061";
#elif UNITY_ANDROID
                unitIds.APP_KEY = "f0f492d3-34ea-47ad-b98f-84640bd2f36a";
                unitIds.BANNER_PLACEMENT_ID = "340339090";
                unitIds.BANNER_REWARDED_PLACEMENT_ID = "300632690";
                unitIds.ICON_PLACEMENT_ID = "376373450";
                unitIds.ICON_REWARDED_PLACEMENT_ID = "353360647";
#endif
            }
            else
            {
#if UNITY_IOS
                unitIds = iosIds;
#elif UNITY_ANDROID
                unitIds = androidIds;
#endif
            }
        }

        private void Start()
        {
            OdeeoSdk.OnInitializationSuccess += OnInitializationFinished;
            OdeeoSdk.OnInitializationFailed += OnInitializationFailed;
            // Initialization();
        }
        
        public IEnumerator DoInit()
        {
            if (OdeeoSdk.IsInitialized()) yield break;
            Initialization();
            while (!OdeeoSdk.IsInitialized()) yield return null;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OdeeoSdk.onApplicationPause(pauseStatus);
        }

        private void Initialization()
        {
            OdeeoSdk.Initialize(unitIds.APP_KEY);
            OdeeoSdk.SetLogLevel(logLevel);
            OdeeoSdk.ForceRegulationType(OdeeoSdk.GetRegulationType());
            OdeeoSdk.SetDoNotSellPrivacyString("privacyString");
            OdeeoSdk.SetIsChildDirected(false);

#if UNITY_IOS 
            //Wrapped native IOS requestTrackingAuthorization function, to be able fetch advertiser id
            OdeeoSdk.RequestTrackingAuthorization();
#endif
            
        }

        private void OnInitializationFailed(int errorParam, string error)
        {
            ShowLog("Initialization failed: " + error);
        }

        private void OnInitializationFinished()
        {
            ShowLog("Initialization successfully");
            CreateAdUnits();
        }

        private void CreateAdUnits()
        {
            if (IsAdIDValid(unitIds.ICON_PLACEMENT_ID))
            {
                _iconPlacement = new IconPlacement(unitIds.ICON_PLACEMENT_ID);
            }
            
            if (IsAdIDValid(unitIds.ICON_REWARDED_PLACEMENT_ID))
            {
                _iconRewardPlacement = new IconRewardPlacement(unitIds.ICON_REWARDED_PLACEMENT_ID);
            }
            
            if (IsAdIDValid(unitIds.BANNER_PLACEMENT_ID))
            {
                _bannerPlacement = new BannerPlacement(unitIds.BANNER_PLACEMENT_ID);
            }
            
            if (IsAdIDValid(unitIds.BANNER_REWARDED_PLACEMENT_ID))
            {
                _bannerRewardPlacement = new BannerRewardPlacement(unitIds.BANNER_REWARDED_PLACEMENT_ID);
            }
        }
        
        private bool IsAdIDValid(string placementID)
        {
            bool isAvailable = false;
            
            if (!string.IsNullOrEmpty(placementID))
            {
                isAvailable = true;
            }
            else
            {
                isAvailable = false;
                Debug.LogError($"Ad ID:{placementID} is not available.");

            }

            return isAvailable;
        }
        
       

        #region Icon Ad
    
        public void ShowIconAd(OdeeoSdk.IconPosition position, int iconSize = 80, int xOffset = 10, int yOffset = 10)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.ICON_PLACEMENT_ID))
            {
                _iconPlacement.ShowIconAd(unitIds.ICON_PLACEMENT_ID, position, iconSize, xOffset, yOffset);
            }
        }

        public void ShowIconAd(OdeeoIconAnchor iconAnchor)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.ICON_PLACEMENT_ID))
            {
                _iconPlacement.ShowIconAd(unitIds.ICON_PLACEMENT_ID, iconAnchor);
            }
        }

        public void ShowIconAd(OdeeoSdk.IconPosition position, Canvas canvas, RectTransform rect,
            OdeeoSdk.AdSizingMethod sizingMethod = OdeeoSdk.AdSizingMethod.Flexible)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.ICON_PLACEMENT_ID))
            {
                _iconPlacement.ShowIconAd(unitIds.ICON_PLACEMENT_ID, position, canvas, rect, sizingMethod);
            }
        }

        public void ChangeIconAdVisual(Color progressBarColor = default, Color audioOnlyBackground = default, Color audioOnlyAnimation = default)
        {
            if (IsAdIDValid(unitIds.ICON_PLACEMENT_ID))
            {
                _iconPlacement.ChangeAdVisual(progressBarColor, audioOnlyBackground, audioOnlyAnimation);
            }
        }

        public void RemoveIconAd()
        {
            if(OdeeoAdManager.IsPlacementExist(unitIds.ICON_PLACEMENT_ID) && OdeeoAdManager.IsAdPlaying(unitIds.ICON_PLACEMENT_ID))
                OdeeoAdManager.RemoveAd(unitIds.ICON_PLACEMENT_ID);
        }

        public bool IsIconAdReady()
        {
            return OdeeoAdManager.IsAdAvailable(unitIds.ICON_PLACEMENT_ID);
        }

        public bool IsIconAdPlaying()
        {
            return OdeeoAdManager.IsAdPlaying(unitIds.ICON_PLACEMENT_ID);
        }

        #endregion
        
        #region Icon Reward

        public void ShowIconRewardAd(Action OnReward, OdeeoSdk.IconPosition position, int iconSize = 80, int xOffset = 10, int yOffset = 10)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.ICON_REWARDED_PLACEMENT_ID))
            {
                _iconRewardPlacement.SetActionOnRewardClaimed(OnReward);
                _iconRewardPlacement.ShowIconAd(unitIds.ICON_REWARDED_PLACEMENT_ID, position, iconSize, xOffset, yOffset);
            }
        }

        public void ShowIconRewardAd(Action OnReward, OdeeoIconAnchor iconAnchor)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.ICON_REWARDED_PLACEMENT_ID))
            {
                _iconRewardPlacement.SetActionOnRewardClaimed(OnReward);
                _iconRewardPlacement.ShowIconAd(unitIds.ICON_REWARDED_PLACEMENT_ID, iconAnchor);
            }
        }

        public void ShowIconRewardAd(Action OnReward, OdeeoSdk.IconPosition position, Canvas canvas, RectTransform rect,
            OdeeoSdk.AdSizingMethod sizingMethod = OdeeoSdk.AdSizingMethod.Flexible)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.ICON_REWARDED_PLACEMENT_ID))
            {
                _iconRewardPlacement.SetActionOnRewardClaimed(OnReward);
                _iconRewardPlacement.ShowIconAd(unitIds.ICON_REWARDED_PLACEMENT_ID, position, canvas, rect, sizingMethod);
            }
        }
        
        public void ChangeIconRewardAdVisual(Color progressBarColor = default, Color audioOnlyBackground = default, Color audioOnlyAnimation = default)
        {
            if (IsAdIDValid(unitIds.ICON_REWARDED_PLACEMENT_ID))
            {
                _iconRewardPlacement.ChangeAdVisual(progressBarColor, audioOnlyBackground, audioOnlyAnimation);
            }
        }
        
        public void RemoveIconRewardAd()
        {
            if(OdeeoAdManager.IsPlacementExist(unitIds.ICON_REWARDED_PLACEMENT_ID) && OdeeoAdManager.IsAdPlaying(unitIds.ICON_REWARDED_PLACEMENT_ID))
                OdeeoAdManager.RemoveAd(unitIds.ICON_REWARDED_PLACEMENT_ID);
        }

        public bool IsIconRewardReady()
        {
            return OdeeoAdManager.IsAdAvailable(unitIds.ICON_REWARDED_PLACEMENT_ID);
        }

        public bool IsIconRewardPlaying()
        {
            return OdeeoAdManager.IsAdPlaying(unitIds.ICON_REWARDED_PLACEMENT_ID);
        }

        #endregion
        
        #region Banner Ad
        
        public void ShowBannerAd(OdeeoSdk.BannerPosition position)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.BANNER_PLACEMENT_ID))
            {
                _bannerPlacement.ShowBannerAd(unitIds.BANNER_PLACEMENT_ID, position);
            }
        }
        
        public void ChangeBannerAdVisual(Color progressBarColor = default, Color audioOnlyBackground = default, Color audioOnlyAnimation = default)
        {
            if (IsAdIDValid(unitIds.BANNER_PLACEMENT_ID))
            {
                _bannerPlacement.ChangeAdVisual(progressBarColor, audioOnlyBackground, audioOnlyAnimation);
            }
        }
        
        public void RemoveBannerAd()
        {
            if(OdeeoAdManager.IsPlacementExist(unitIds.BANNER_PLACEMENT_ID) && OdeeoAdManager.IsAdPlaying(unitIds.BANNER_PLACEMENT_ID))
                OdeeoAdManager.RemoveAd(unitIds.BANNER_PLACEMENT_ID);
        }

        public bool IsBannerPlaying()
        {
            return OdeeoAdManager.IsAdPlaying(unitIds.BANNER_PLACEMENT_ID);
        }
        
        public bool IsBannerReady()
        {
            return OdeeoAdManager.IsAdAvailable(unitIds.BANNER_PLACEMENT_ID);
        }
        
        #endregion

        #region Banner Reward

        public void ShowBannerRewardAd(Action OnReward, OdeeoSdk.BannerPosition position)
        {
            StartCoroutine(DoInit());
            
            if (IsAdIDValid(unitIds.BANNER_REWARDED_PLACEMENT_ID))
            {
                _bannerRewardPlacement.SetActionOnRewardClaimed(OnReward);
                _bannerRewardPlacement.ShowBannerAd(unitIds.BANNER_REWARDED_PLACEMENT_ID, position);
            }
        }
        
        public void ChangeBannerRewardAdVisual(Color progressBarColor = default, Color audioOnlyBackground = default, Color audioOnlyAnimation = default)
        {
            if (IsAdIDValid(unitIds.BANNER_REWARDED_PLACEMENT_ID))
            {
                _bannerRewardPlacement.ChangeAdVisual(progressBarColor, audioOnlyBackground, audioOnlyAnimation);
            }
        }
        
        public void RemoveBannerRewardAd()
        {
            if(OdeeoAdManager.IsPlacementExist(unitIds.BANNER_REWARDED_PLACEMENT_ID) && OdeeoAdManager.IsAdPlaying(unitIds.BANNER_REWARDED_PLACEMENT_ID))
                OdeeoAdManager.RemoveAd(unitIds.BANNER_REWARDED_PLACEMENT_ID);
        }
        
        public bool IsBannerRewardReady()
        {
            return OdeeoAdManager.IsAdAvailable(unitIds.BANNER_REWARDED_PLACEMENT_ID);
        }

        public bool IsBannerRewardPlaying()
        {
            return OdeeoAdManager.IsAdPlaying(unitIds.BANNER_REWARDED_PLACEMENT_ID);
        }

        #endregion

        public void ShowLog(string message)
        {
            Debug.Log("AudioAdManager------: " + message);
        }

        public void SetStatus(string placementType, string placementID, OdeeoAdEvent adEvent)
        {
            string eventName = $"{placementType}_{adEvent.ToString()}";
            
            FirebaseManager.Instance.LogEvent(eventName, new Dictionary<string, object>()
            {
                {"placementType", placementType},
                {"placementID", placementID},
            });
        }
    }
    
}