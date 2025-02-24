using Odeeo;
using Odeeo.Data;

namespace TheLegends.Base.AudioAds
{
    public interface IOdeeoEventCommon
    {
        public void OnAvailabilityChanged(bool availability, OdeeoAdData data);
        public void OnShow();
        public void OnShowFailed(string placementId, OdeeoAdUnit.ErrorShowReason reason, string description);
        public void OnClose(OdeeoAdUnit.CloseReason reason);
        public void OnClick();
        public void OnPause(OdeeoAdUnit.StateChangeReason reason);
        public void OnResume(OdeeoAdUnit.StateChangeReason reason);
        public void OnMute(bool isMuted);
        public void OnImpression(OdeeoImpressionData data);
    
    }
}

