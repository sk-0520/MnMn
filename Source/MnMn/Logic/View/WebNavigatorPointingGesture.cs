using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ContentTypeTextNet.MnMn.MnMn.Define;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    public class WebNavigatorPointingGesture: PointingGesture
    {
        public WebNavigatorPointingGesture()
        {
            GeckoFxSuppressionRestoreTimer = new DispatcherTimer();
            GeckoFxSuppressionRestoreTimer.Tick += SuppressionRestoreTimer_Tick;
            GeckoFxSuppressionRestoreTimer.Interval = Constants.WebNavigatorGeckoFxGestureWaitTime;
        }

        #region property

        GeckoElement GeckoFxElement { get; set; }

        public bool GeckoFxSuppressionContextMenu { get; set; }
        public DispatcherTimer GeckoFxSuppressionRestoreTimer { get; }

        #endregion

        #region function

        public void StartPreparation(Point point, GeckoElement element)
        {
            GeckoFxSuppressionRestoreTimer.Stop();
            GeckoFxElement = element;
            StartPreparation(point);
        }

        #endregion

        #region PointingGesture

        protected override void OnChanged(PointingGestureChangeKind changeKind)
        {
            base.OnChanged(changeKind);

            if(changeKind == PointingGestureChangeKind.Start || changeKind == PointingGestureChangeKind.Add) {
                GeckoFxSuppressionContextMenu = true;
            } else {
                GeckoFxSuppressionRestoreTimer.Stop();
                GeckoFxSuppressionRestoreTimer.Start();
            }
        }

        #endregion

        private void SuppressionRestoreTimer_Tick(object sender, EventArgs e)
        {
            GeckoFxSuppressionContextMenu = false;
            GeckoFxSuppressionRestoreTimer.Stop();
        }
    }
}
