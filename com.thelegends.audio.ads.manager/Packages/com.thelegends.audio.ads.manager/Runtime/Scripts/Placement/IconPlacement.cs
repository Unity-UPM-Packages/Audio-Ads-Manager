using System.Collections;
using System.Collections.Generic;
using Odeeo;
using Odeeo.Data;
using UnityEngine;

namespace TheLegends.Base.AudioAds
{
    public class IconPlacement : PlacementBase
    {
        public IconPlacement(string placementID) : base(placementID)
        {
        }

        protected override void Create()
        {
            OdeeoAdManager.CreateAudioIconAd(placementID);
            placementType = OdeeoSdk.PlacementType.AudioIconAd.ToString();
            InitPlacement();
        }

        #region Icon Ad

        public virtual void ShowIconAd(string placementID, OdeeoSdk.IconPosition position, int iconSize = 80, int xOffset = 10, int yOffset = 10)
        {
            if (!IsReady()) return;
            OdeeoAdManager.SetIconPosition(placementID, position, xOffset, yOffset);
            OdeeoAdManager.SetIconSize(placementID, iconSize);
            Show();
        }

        public virtual void ShowIconAd(string placementID, OdeeoIconAnchor iconAnchor)
        {
            if (!IsReady()) return;
            OdeeoAdManager.LinkToIconAnchor(placementID, iconAnchor);
            Show();
        }

        public virtual void ShowIconAd(string placementID, OdeeoSdk.IconPosition position, Canvas canvas, RectTransform rect,
            OdeeoSdk.AdSizingMethod sizingMethod = OdeeoSdk.AdSizingMethod.Flexible)
        {
            if (!IsReady()) return;
            OdeeoAdManager.LinkIconToRectTransform(placementID, position, rect, canvas, sizingMethod);
            Show();
        }

        #endregion
    }
}