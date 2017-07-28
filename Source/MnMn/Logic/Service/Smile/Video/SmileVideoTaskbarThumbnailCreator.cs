using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Logic.View;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoTaskbarThumbnailCreator : TaskbarThumbnailCreatorBase<SmileVideoPlayerWindow>
    {
        public SmileVideoTaskbarThumbnailCreator(SmileVideoPlayerWindow element, Action<Thickness> receiveMethod)
            : base(element, receiveMethod)
        { }

        #region function

        Size Size { get; set; }
        bool IsClosed { get; set; }

        #endregion

        #region function

        public void SetSize(Size size)
        {
            Size = size;
        }

        #endregion

        #region TaskbarThumbnailCreatorBase

        protected override void AttacheEvent()
        {
            base.AttacheEvent();

            Element.Closing += Element_Closing;
        }

        protected override void DetachEvent()
        {
            base.DetachEvent();

            Element.Closing -= Element_Closing;
        }

        protected override Thickness GetThickness()
        {
            var result = new Thickness();
            if(IsClosed) {
                return result;
            }

            var viewLocation = Element.PointToScreen(new Point(0, 0));
            var layerLocation = Element.layer.PointToScreen(new Point(0, 0));

            result.Left = layerLocation.X - viewLocation.X;
            result.Top = layerLocation.Y - viewLocation.Y;

            result.Right = Element.Width - Size.Width - result.Left;
            result.Bottom = Element.Height - Size.Height - result.Top;

            return result;
        }

        private void Element_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Closed まで待てない
            IsClosed = true;
        }

        #endregion
    }
}
