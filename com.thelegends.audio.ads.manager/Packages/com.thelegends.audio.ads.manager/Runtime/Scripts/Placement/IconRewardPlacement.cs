using System;
using Odeeo;

namespace TheLegends.Base.AudioAds
{
    public class IconRewardPlacement : IconPlacement
    {
        private PlacementReward reward;
        public IconRewardPlacement(string placementID) : base(placementID)
        {
        }
        

        public void SetActionOnRewardClaimed(Action action)
        {
            reward.SetActionOnRewardClaimed(action);
        }
        
        protected override void Create()
        {
            OdeeoAdManager.CreateRewardedAudioIconAd(placementID);
            placementType = OdeeoSdk.PlacementType.RewardedAudioIconAd.ToString();
            reward = new PlacementReward(this, placementType, placementID);
            InitPlacement();
        }
    }
}

