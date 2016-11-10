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
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class FinderDragAndDrop: IDragAndDrop
    {
        #region IDragAndDrop

        public Action<UIElement, DragEventArgs> DragEnter
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Action<UIElement, DragEventArgs> DragLeave
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Action<UIElement, DragEventArgs> DragOver
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Action<UIElement, DragEventArgs> Drop
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Action<UIElement, MouseButtonEventArgs> MouseDown
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Action<UIElement, MouseEventArgs> MouseMove
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
