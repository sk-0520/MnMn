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
    public class Ffmpeg
    {
        public Ffmpeg(string applicationPath)
        {
            ApplicationPath = applicationPath;
        }

        public Ffmpeg()
            : this(Constants.FfmpegApplicationPath)
        { }

        #region property

        string ApplicationPath { get; }
        TaskCompletionSource<bool> ConvertTask { get; set; }

        #endregion

        #region function

        public Task ExecuteAsync(string arguments)
        {
            ConvertTask = new TaskCompletionSource<bool>();

            var process = new Process();
            process.StartInfo.FileName = ApplicationPath;
            process.StartInfo.Arguments = arguments;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();

            return ConvertTask.Task;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            var process = (Process)sender;
            ConvertTask.SetResult(true);
        }

        #endregion


    }
}
