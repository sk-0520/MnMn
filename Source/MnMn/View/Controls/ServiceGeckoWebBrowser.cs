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
using System.Windows.Forms;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    public class ServiceGeckoWebBrowser: GeckoWebBrowser
    {
        #region define

        const int WM_XBUTTONUP = 0x020C;
        const int XBUTTON1 = 0x10000;
        const int XBUTTON2 = 0x20000;

        #endregion

        public ServiceGeckoWebBrowser(Mediation mediation)
            : base()
        {
            Mediation = mediation;
        }

        #region property

        /// <summary>
        /// このブラウザの生成目的。
        /// <para>OnInitializeで設定できないから無理やりやるべ。</para>
        /// </summary>
        public ServiceType ServiceType { get; private set; }

        public Mediation Mediation { get; private set; }

        #endregion

        #region function

        #endregion

        #region GeckoWebBrowser

        public override bool PreProcessMessage(ref Message msg)
        {
            if(msg.Msg == WM_XBUTTONUP) {
                var wParam = msg.WParam.ToInt32();

                if(wParam == XBUTTON1 && CanGoBack) {
                    GoBack();
                } else if(wParam == XBUTTON2 && CanGoForward) {
                    GoForward();
                }
            }

            return base.PreProcessMessage(ref msg);
        }

        #endregion
    }
}
