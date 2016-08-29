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
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    /// <summary>
    /// カーソルを隠そうとする人。
    /// <para>http://stackoverflow.com/questions/744980/hide-mouse-cursor-after-an-idle-time?answertab=votes#tab-top</para>
    /// </summary>
    public class CursorHider: DisposeFinalizeBase
    {
        /// <summary>
        /// 対象UIを登録する。
        /// </summary>
        /// <param name="element"></param>
        public CursorHider(FrameworkElement element)
        {
            Element = element;

            Element.MouseMove += Element_MouseMove;
            HideWaitTimer.Tick += HideWaitTimer_Tick;
        }

        #region property

        /// <summary>
        /// 標準カーソル。
        /// </summary>
        public Cursor DefaultCursor { get; set; } = Cursors.Arrow;
        /// <summary>
        /// 隠れた状態のカーソル。
        /// </summary>
        public Cursor HiddenCursor { get; set; } = Cursors.No;

        /// <summary>
        /// 判定用タイマー。
        /// </summary>
        DispatcherTimer HideWaitTimer { get; } = new DispatcherTimer();

        /// <summary>
        /// 対象UI。
        /// </summary>
        protected FrameworkElement Element { get; }

        /// <summary>
        /// カーソルを隠すまでの猶予。
        /// </summary>
        public TimeSpan HideTime { get; set; } = Constants.PlayerCursorHideTime;

        /// <summary>
        /// 最後にカーソルが動いた時間。
        /// </summary>
        protected DateTime LastCursorMoveTime { get; private set; }

        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHidden { get; private set; }

        #endregion

        private void Element_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HideWaitTimer.Stop();
            LastCursorMoveTime = DateTime.Now;

            if(IsHidden) {
                Element.Cursor = DefaultCursor;
            } else {
                HideWaitTimer.Interval = HideTime;
                HideWaitTimer.Start();
            }
        }

        private void HideWaitTimer_Tick(object sender, EventArgs e)
        {
            if(HideTime <= DateTime.Now - LastCursorMoveTime) {
                Element.Cursor = HiddenCursor;
                IsHidden = true;
                HideWaitTimer.Stop();
            }
        }

    }
}
