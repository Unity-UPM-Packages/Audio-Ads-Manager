using System.Collections;
using Odeeo;

namespace TheLegends.Base.AudioAds
{
    public class BannerPlacement : PlacementBase
    {
        public BannerPlacement(string placementID) : base(placementID)
        {
        }
        
        protected override void Create()
        {
            OdeeoAdManager.CreateAudioBannerAd(placementID);
            placementType = OdeeoSdk.PlacementType.AudioBannerAd.ToString();
            InitPlacement();
        }

        #region Banner Ad

        public virtual void ShowBannerAd(string placementID, OdeeoSdk.BannerPosition position)
        {
            if (!IsReady()) return;
            OdeeoAdManager.SetBannerPosition(placementID, position);
            Show();
        }

        #endregion
    }
}

