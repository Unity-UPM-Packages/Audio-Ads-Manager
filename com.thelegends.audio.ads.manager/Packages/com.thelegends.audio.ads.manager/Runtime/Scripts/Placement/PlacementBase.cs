using System.Collections.Generic;
using Odeeo;
using Odeeo.Data;
using TheLegends.Base.Firebase;
using UnityEngine;

namespace TheLegends.Base.AudioAds
{
    public abstract class PlacementBase : IOdeeoEventCommon
    {
        protected string placementID = "";
        protected string placementType = "";

        private bool isCreated = false;
        
        private static readonly Color defaultAudioOnlyAnimation = Color.white;
        private static readonly Color defaultAudioOnlyBackground = new Color(0.62f, 0.063f, 0.99f, 1f);
        private static readonly Color defaultprogressBarColor = Color.white;

        protected OdeeoAdEvent status = OdeeoAdEvent.None;

        public OdeeoAdEvent Status
        {
            get => status;
            internal set
            {
                if (status != value)
                {
                    status = value;
                    AudioAdManager.Instance.SetStatus(placementType, placementID, status);
                }
            }
        }
        
        public PlacementBase(string placementID)
        {
            this.placementID = placementID;
            Create();
        }

        #region Callback

        public virtual void OnAvailabilityChanged(bool availability, OdeeoAdData data)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} availability: {availability}");
            if (availability)
            {
                Status = OdeeoAdEvent.LoadAvailable;
            }
        }

        public virtual void OnShow()
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} show success");
            Status = OdeeoAdEvent.ShowSuccess;
        }

        public virtual void OnShowFailed(string placementId, OdeeoAdUnit.ErrorShowReason reason, string description)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} show failed, reason: {reason}, description: {description}");
            Status = OdeeoAdEvent.ShowFail;
        }

        public virtual void OnClose(OdeeoAdUnit.CloseReason reason)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} close, reason: {reason}");
            switch (reason)
            {
                case OdeeoAdUnit.CloseReason.AdCompleted:
                    Status = OdeeoAdEvent.ShowComplete;
                    break;
                case OdeeoAdUnit.CloseReason.UserClose:
                    Status = OdeeoAdEvent.Close;
                    break;
            }

            Status = OdeeoAdEvent.LoadAvailable;
        }

        public virtual void OnClick()
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} click");
            Status = OdeeoAdEvent.Click;
        }

        public virtual void OnPause(OdeeoAdUnit.StateChangeReason reason)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} pause, reason: {reason}");
            Status = OdeeoAdEvent.Pause;
        }

        public virtual void OnResume(OdeeoAdUnit.StateChangeReason reason)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} resume, reason: {reason}");
            Status = OdeeoAdEvent.Resume;
        }

        public virtual void OnMute(bool isMuted)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} mute, isMuted: {isMuted}");
            Status = OdeeoAdEvent.Mute;
        }

        public virtual void OnImpression(OdeeoImpressionData data)
        {
            AudioAdManager.Instance.ShowLog($"{placementType}:{placementID} impression: \n"                 
                                            + " SessionID: " + data.GetSessionID() + "\n"
                                            + " PlacementType: " + data.GetPlacementType() + "\n"
                                            + " PlacementID: " + data.GetPlacementID() + "\n"
                                            + " Country: " + data.GetCountry() + "\n"
                                            + " PayableAmount: " + data.GetPayableAmount() + "\n"
                                            + " TransactionID: " + data.GetTransactionID() + "\n"
                                            + " CustomTag: " + data.GetCustomTag());
            
            FirebaseManager.Instance.LogEvent("Odeeo_Impression", new Dictionary<string, object>()
            {
                {"sessionID", data.GetSessionID()},
                {"placementType", data.GetPlacementType()},
                {"placementID", data.GetPlacementID()},
                {"country", data.GetCountry()},
                {"payableAmount", data.GetPayableAmount()},
                {"transactionID", data.GetTransactionID()},
                {"customTag", data.GetCustomTag()},
                
            });
        }

        #endregion
        
        #region Visual
        
        public void SetAdProgressBarColor(Color color)
        {
            OdeeoAdManager.SetProgressBar(placementID, color);
        }
        
        public void SetAdBackgroundColor(Color color)
        {
            OdeeoAdManager.SetAudioOnlyBackground(placementID, color);
        }
        
        public void SetAdAnimationColor(Color color)
        {
            OdeeoAdManager.SetAudioOnlyAnimation(placementID, color);
        }
        
        public void ChangeAdVisual(Color progressBarColor = default, Color audioOnlyBackground = default, Color audioOnlyAnimation = default)
        {
            SetAdProgressBarColor(progressBarColor == default ? defaultprogressBarColor : progressBarColor);
            SetAdBackgroundColor(audioOnlyBackground == default ? defaultAudioOnlyBackground : audioOnlyBackground);
            SetAdAnimationColor(audioOnlyAnimation == default ? defaultAudioOnlyAnimation : audioOnlyAnimation);
        }

        #endregion
        

        public virtual void Show()
        {
            OdeeoAdManager.ShowAd(placementID);
        }

        public virtual void Remove()
        {
            OdeeoAdManager.RemoveAd(placementID);
        }

        protected virtual void Create()
        {
        }

        protected void InitPlacement()
        {
            if (Status == OdeeoAdEvent.LoadRequest)
            {
                AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} is loading --> return");
                return;
            }

            if (Status == OdeeoAdEvent.ShowSuccess)
            {
                AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} is showing --> return");
                return;
            }

            isCreated = true;
            Status = OdeeoAdEvent.LoadRequest;
            SubscribeEvents();
        }

        public bool IsReady()
        {
            if (!isCreated)
            {
                AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} is not created yet. Creating Ad Unit --> return");
                Create();
                return false;
            }
            
            if (!OdeeoAdManager.IsAdAvailable(placementID) || Status != OdeeoAdEvent.LoadAvailable || OdeeoAdManager.IsAdPlaying(placementID))
            {
                AudioAdManager.Instance.ShowLog($"{placementType}_{placementID} is not ready yet --> return");
                return false;
            }

            return true;
        }

        protected virtual void SubscribeEvents()
        {
            if (!OdeeoAdManager.IsPlacementExist(placementID))
                return;

            OdeeoAdManager.AdUnitCallbacks(placementID).OnAvailabilityChanged += OnAvailabilityChanged;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnShow += OnShow;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnShowFailed += OnShowFailed;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnClose += OnClose;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnClick += OnClick;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnPause += OnPause;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnResume += OnResume;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnMute += OnMute;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnImpression += OnImpression;
        }

        protected virtual void UnSubscribeEvents()
        {
            if (!OdeeoAdManager.IsPlacementExist(placementID))
                return;
            
            OdeeoAdManager.AdUnitCallbacks(placementID).OnAvailabilityChanged -= OnAvailabilityChanged;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnShow -= OnShow;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnShowFailed -= OnShowFailed;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnClose -= OnClose;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnClick -= OnClick;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnPause -= OnPause;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnResume -= OnResume;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnMute -= OnMute;
            OdeeoAdManager.AdUnitCallbacks(placementID).OnImpression -= OnImpression;
        }
        
        
    }
}