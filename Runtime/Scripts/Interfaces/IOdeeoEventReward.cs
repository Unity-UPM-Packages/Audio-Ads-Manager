using System;
using Odeeo;

namespace TheLegends.Base.AudioAds
{
    public interface IOdeeoEventReward
    {
        public void OnReward(float amount);
        public void OnRewardedPopupAppear();
        public void OnRewardedPopupClosed(OdeeoAdUnit.CloseReason reason);
        public void SetActionOnRewardClaimed(Action action);
    }

}
