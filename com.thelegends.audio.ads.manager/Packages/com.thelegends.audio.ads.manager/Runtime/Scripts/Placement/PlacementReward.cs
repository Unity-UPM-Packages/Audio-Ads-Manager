using System;
using Odeeo;

namespace TheLegends.Base.AudioAds
{
    public class PlacementReward : IOdeeoEventReward
    {
        private PlacementBase placementBase = null;
        
        private string placementID = "";
        private string placementType = "";
        
        
        private Action OnRewardClaimed = null;
        
        public PlacementReward(PlacementBase placementBase, string placementType, string placementID)
        {
            this.placementBase = placementBase;
            this.placementType = placementType;
            this.placementID = placementID;
            SubscribeEvents();
        }

        #region Callback

        public void OnShow()
        {
            OdeeoAdManager.TrackRewardedOffer(placementID);
        }

        public void OnReward(float amount)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} reward claimed");
            OnRewardClaimed?.Invoke();
            OnRewardClaimed = null;
        }

        public void OnRewardedPopupAppear()
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} reward appear");
            placementBase.Status = OdeeoAdEvent.PopupAppear;
        }

        public void OnRewardedPopupClosed(OdeeoAdUnit.CloseReason reason)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} reward closed, reason: {reason}");
            switch (reason)
            {
                case OdeeoAdUnit.CloseReason.UserCancel:
                    placementBase.Status = OdeeoAdEvent.PopupSkip;
                    break;
            }
        }

        #endregion
        

        public void SetActionOnRewardClaimed(Action action)
        {
            OnRewardClaimed = action;
        }

        public void SubscribeEvents()
        {
            if (!OdeeoAdManager.IsPlacementExist(placementID))
                return;

            OdeeoAdManager.AdUnitCallbacks(placementID).OnShow += OnShow;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnReward += OnReward;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnRewardedPopupAppear += OnRewardedPopupAppear;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnRewardedPopupClosed += OnRewardedPopupClosed;
        
        }
        
        public void UnSubscribeEvents()
        {
            if (!OdeeoAdManager.IsPlacementExist(placementID))
                return;
        
            OdeeoAdManager.AdUnitCallbacks(placementID).OnShow -= OnShow;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnReward -= OnReward;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnRewardedPopupAppear -= OnRewardedPopupAppear;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnRewardedPopupClosed -= OnRewardedPopupClosed;
        
        }
    
    }
}

