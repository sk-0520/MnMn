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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Data
{
    public abstract class WebNavigatorEventDataBase
    {
        public WebNavigatorEventDataBase(WebNavigatorEngine engine, object sender, EventArgs e, object parameter)
        {
            Engine = engine;
            Sender = sender;
            BaseEventArgs = e;
            Parameter = parameter;
        }

        #region property

        /// <summary>
        /// 投げたエンジン。
        /// </summary>
        public WebNavigatorEngine Engine { get; }
        /// <summary>
        /// 投げた人。
        /// </summary>
        public object Sender { get; }
        /// <summary>
        /// イベント。
        /// </summary>
        public EventArgs BaseEventArgs { get; }
        /// <summary>
        /// コマンドに紐づけられたパラメータ。
        /// </summary>
        public object Parameter { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="ContentTypeTextNet.MnMn.MnMn.View.Controls.WebNavigator"/>のコマンドがイベントの際にいい感じにパラメータと投げたエンジンを橋渡しする。
    /// </summary>
    public class WebNavigatorEventData<TEventArgs>: WebNavigatorEventDataBase
        where TEventArgs : EventArgs
    {
        public WebNavigatorEventData(WebNavigatorEngine engine, object sender, TEventArgs e, object parameter)
            : base(engine, sender, e, parameter)
        {
            EventArgs = e;
        }

        #region property

        /// <summary>
        /// きちんとしたイベント。
        /// </summary>
        public TEventArgs EventArgs { get; }

        #endregion
    }

    /// <summary>
    /// ヘルパー。
    /// </summary>
    public static class WebNavigatorEventData
    {
        public static WebNavigatorEventData<TEventArgs> Create<TEventArgs>(WebNavigatorEngine engine, object sender, TEventArgs e, object parameter)
            where TEventArgs : EventArgs
        {
            return new WebNavigatorEventData<TEventArgs>(engine, sender, e, parameter);
        }
    }

}
