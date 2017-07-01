/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public abstract class DragAndDropBase: IDragAndDrop
    {
        #region property

        Point DragStartPosition { get; set; }

        public bool IsDragging { get; private set; }

        /// <summary>
        /// ドラッグ開始とみなす距離。
        /// </summary>
        [PixelKind(Px.Device)]
        public Size DragStartSize = new Size(10, 10);

        #endregion

        #region function

        protected abstract bool CanDragStart(UIElement sender, MouseEventArgs e);
        protected abstract IReadOnlyCheckResult<DragParameterModel> GetDragParameter(UIElement sender, MouseEventArgs e);

        void MouseDownCore(UIElement sender, MouseEventArgs e)
        {
            DragStartPosition = e.GetPosition(null);
        }

        void MouseMoveCore(UIElement sender, MouseEventArgs e)
        {
            var nowPosition = e.GetPosition(null);

            var isDragX = Math.Abs(nowPosition.X - DragStartPosition.X) > DragStartSize.Width;
            var isDragY = Math.Abs(nowPosition.Y - DragStartPosition.Y) > DragStartSize.Height;
            if(isDragX || isDragY) {
                var parameterResult = GetDragParameter(sender, e);
                if(parameterResult.IsSuccess) {
                    var parameter = parameterResult.Result;
                    IsDragging = true;
                    DragDrop.DoDragDrop(parameter.Element, parameter.Data, parameter.Effects);
                    IsDragging = false;
                }
            }
        }

        #endregion

        #region IDragAndDrop

        public void MouseDown(UIElement sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) {
                MouseDownCore(sender, e);
            }
        }

        public void MouseMove(UIElement sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            if(IsDragging) {
                return;
            }

            if(!CanDragStart(sender, e)) {
                return;
            }

            MouseMoveCore(sender, e);
        }

        public abstract void DragEnter(UIElement sender, DragEventArgs e);

        public abstract void DragOver(UIElement sender, DragEventArgs e);

        public abstract void DragLeave(UIElement sender, DragEventArgs e);

        public abstract void Drop(UIElement sender, DragEventArgs e);


        #endregion
    }
}
