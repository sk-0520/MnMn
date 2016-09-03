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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Wrapper
{
    /// <summary>
    /// 何かしらのプログラムを実行する。
    /// <para>運用上プログラムは固定したいので継承してプログラムを設定する。</para>
    /// </summary>
    public abstract class WrapperBase
    {
        protected WrapperBase(string applicationPath)
        {
            ApplicationPath = applicationPath;
        }

        #region property

        /// <summary>
        /// 実行するプログラム。
        /// </summary>
        string ApplicationPath { get; }
        /// <summary>
        /// プログラムの実行タスク。
        /// </summary>
        TaskCompletionSource<int> ExecuteTask { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 指定オプションでプログラムを実行する
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>終了コードのタスク。</returns>
        public Task<int> ExecuteAsync(string arguments)
        {
            ExecuteTask = new TaskCompletionSource<int>();

            var process = new Process();
            process.StartInfo.FileName = ApplicationPath;
            process.StartInfo.Arguments = arguments;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();

            return ExecuteTask.Task;
        }

        #endregion

        private void Process_Exited(object sender, EventArgs e)
        {
            var process = (Process)sender;
            ExecuteTask.SetResult(process.ExitCode);
        }


    }
}
