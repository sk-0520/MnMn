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
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class DelegateDragAndDrop: DragAndDropBase
    {
        #region property

        public Func<UIElement, MouseEventArgs, bool> CanDragStartFunc { get; set; }
        public Func<UIElement, MouseEventArgs, CheckResultModel<DragParameterModel>> GetDragParameterFunc { get; set; }
        public Action<UIElement, DragEventArgs> DragEnterAction { get; set; }
        public Action<UIElement, DragEventArgs> DragLeaveAction { get; set; }
        public Action<UIElement, DragEventArgs> DragOverAction { get; set; }
        public Action<UIElement, DragEventArgs> DropAction { get; set; }

        #endregion

        #region DragAndDropBase

        protected override bool CanDragStart(UIElement sender, MouseEventArgs e)
        {
            if(CanDragStartFunc != null) {
                return CanDragStartFunc(sender, e);
            }

            return false;
        }

        protected override CheckResultModel<DragParameterModel> GetDragParameter(UIElement sender, MouseEventArgs e)
        {
            if(GetDragParameterFunc != null) {
                return GetDragParameterFunc(sender, e);
            }

            return CheckResultModel.Failure<DragParameterModel>();
        }

        public override void DragEnter(UIElement sender, DragEventArgs e)
        {
            DragEnterAction?.Invoke(sender, e);
        }

        public override void DragLeave(UIElement sender, DragEventArgs e)
        {
            DragLeaveAction?.Invoke(sender, e);
        }

        public override void DragOver(UIElement sender, DragEventArgs e)
        {
            DragOverAction?.Invoke(sender, e);
        }

        public override void Drop(UIElement sender, DragEventArgs e)
        {
            DropAction?.Invoke(sender, e);
        }

        #endregion
    }
}
