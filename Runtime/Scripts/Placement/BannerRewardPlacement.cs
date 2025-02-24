using System;
using Odeeo;

namespace TheLegends.Base.AudioAds
{
    public class BannerRewardPlacement : BannerPlacement
    {
        private PlacementReward reward;
        public BannerRewardPlacement(string placementID) : base(placementID)
        {
        }
        

        public void SetActionOnRewardClaimed(Action action)
        {
            reward.SetActionOnRewardClaimed(action);
        }

        protected override void Create()
        {
            OdeeoAdManager.CreateRewardedAudioBannerAd(placementID);
            placementType = OdeeoSdk.PlacementType.RewardedAudioBannerAd.ToString();
            reward = new PlacementReward(this, placementType, placementID);
            InitPlacement();
        }
    }
}