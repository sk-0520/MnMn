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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class WebNavigatorUtility
    {
        #region function

        /// <summary>
        /// 戻り値を有する処理の実施。
        /// </summary>
        /// <typeparam name="TResult">戻り値</typeparam>
        /// <param name="engine">エンジン。</param>
        /// <param name="defaultFunction">標準ブラウザ使用時の処理。</param>
        /// <param name="geckoFxFunction">Gecko版使用時の処理。</param>
        /// <returns></returns>
        public static TResult DoFunction<TResult>(WebNavigatorEngine engine, Func<TResult> defaultFunction, Func<TResult> geckoFxFunction)
        {
            CheckUtility.DebugEnforceNotNull(defaultFunction);
            CheckUtility.DebugEnforceNotNull(geckoFxFunction);

            switch(engine) {
                case Define.WebNavigatorEngine.Default:
                    return defaultFunction();

                case Define.WebNavigatorEngine.GeckoFx:
                    return geckoFxFunction();

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 戻り値の無い処理の実施。
        /// </summary>
        /// <param name="defaultAction">標準ブラウザ使用時の処理。</param>
        /// <param name="geckoFxAction">Gecko版使用時の処理。</param>
        public static void DoAction(WebNavigatorEngine engine, Action defaultAction, Action geckoFxAction)
        {
            CheckUtility.DebugEnforceNotNull(defaultAction);
            CheckUtility.DebugEnforceNotNull(geckoFxAction);

            var dmy = -1;

            DoFunction(
                engine,
                () => { defaultAction(); return dmy; },
                () => { geckoFxAction(); return dmy; }
            );
        }


        public static void OpenNewWindowWrapper(WebNavigatorEventDataBase data, ILogger logger)
        {
            switch(data.Engine) {
                case WebNavigatorEngine.GeckoFx: {
                        var exData = (WebNavigatorEventData<GeckoCreateWindowEventArgs>)data;
                        exData.EventArgs.Cancel = true;
                        try {
                            Process.Start(exData.EventArgs.Uri);
                        } catch(Exception ex) {
                            logger.Error(ex);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}
