using System;
using System.Collections;
using System.Collections.Generic;
using Odeeo;
using TheLegends.Base.AudioAds;
using UnityEngine;
using UnityEngine.UI;

public class DemoManager : MonoBehaviour
{
    public Button initBtn;
    public Button showIconBtn;
    public Button hideIconBtn;
    public Button showIconRewardBtn;
    public Button hideIconRewardBtn;
    public Button showBannerBtn;
    public Button hideBannerBtn;
    public Button showBannerRewardBtn;
    public Button hideBannerRewardBtn;
    
    private void OnEnable()
    {
        initBtn.onClick.AddListener(Init);
        showIconBtn.onClick.AddListener(ShowIcon);
        hideIconBtn.onClick.AddListener(HideIcon);
        showIconRewardBtn.onClick.AddListener(ShowIconReward);
        hideIconRewardBtn.onClick.AddListener(HideIconReward);
        showBannerBtn.onClick.AddListener(ShowBanner);
        hideBannerBtn.onClick.AddListener(HideBanner);
        showBannerRewardBtn.onClick.AddListener(ShowBannerReward);
        hideBannerRewardBtn.onClick.AddListener(HideBannerReward);
    }

    private void OnDisable()
    {
        initBtn.onClick.RemoveListener(Init);
        showIconBtn.onClick.RemoveListener(ShowIcon);
        hideIconBtn.onClick.RemoveListener(HideIcon);
        showIconRewardBtn.onClick.RemoveListener(ShowIconReward);
        hideIconRewardBtn.onClick.RemoveListener(HideIconReward);
        showBannerBtn.onClick.RemoveListener(ShowBanner);
        hideBannerBtn.onClick.RemoveListener(HideBanner);
        showBannerRewardBtn.onClick.RemoveListener(ShowBannerReward);
        hideBannerRewardBtn.onClick.RemoveListener(HideBannerReward);
    }
    
    private void Init()
    {
        StartCoroutine(AudioAdManager.Instance.DoInit());
    }
    
    private void ShowIcon()
    {
        AudioAdManager.Instance.ShowIconAd(OdeeoSdk.IconPosition.Centered);
    }

    private void HideIcon()
    {
        AudioAdManager.Instance.RemoveIconAd();
    }
    
    private void ShowIconReward()
    {
        AudioAdManager.Instance.ShowIconRewardAd(() =>
        {
            AudioAdManager.Instance.ShowLog("Icon Reward claimed");
        }, OdeeoSdk.IconPosition.Centered);
    }
    
    private void HideIconReward()
    {
        AudioAdManager.Instance.RemoveIconRewardAd();
    }
    
    private void ShowBanner()
    {
        AudioAdManager.Instance.ShowBannerAd(OdeeoSdk.BannerPosition.BottomCenter);
    }
    
    private void HideBanner()
    {
        AudioAdManager.Instance.RemoveBannerAd();
    }
    
    private void ShowBannerReward()
    {
        AudioAdManager.Instance.ShowBannerRewardAd(() =>
        {
            AudioAdManager.Instance.ShowLog("Banner Reward claimed");
        }, OdeeoSdk.BannerPosition.BottomCenter);
    }
    
    private void HideBannerReward()
    {
        AudioAdManager.Instance.RemoveBannerRewardAd();
    }

    

    

  

   
    
}
