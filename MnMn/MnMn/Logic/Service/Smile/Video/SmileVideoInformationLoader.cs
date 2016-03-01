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
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoInformationLoader
    {
        public SmileVideoInformationLoader(IEnumerable<SmileVideoInformationViewModel> list)
        {
            List = list.ToArray();
        }

        #region property

        IEnumerable<SmileVideoInformationViewModel> List { get; }

        public CancellationTokenSource Cancel { get; } = new CancellationTokenSource();

        #endregion


        public Task LoadThumbnaiImageAsync(CacheSpan imageCacheSpan)
        {
            var tasks = new List<Task>();
            //return Task.Run(() => {

                //var option = new ParallelOptions() {
                //    MaxDegreeOfParallelism = 2,
                //};

                //Parallel.ForEach(List, option, item => {
                foreach(var item in List) {
                    var count = 0;
                    var max = 3;
                    var wait = TimeSpan.FromSeconds(1);
                    while(count++ <= max) {
                        Cancel.Token.ThrowIfCancellationRequested();

                        try {
                            var task = item.LoadThumbnaiImageAsync(imageCacheSpan);
                        tasks.Add(task);
                            break;
                        } catch(Exception ex) {
                            Debug.WriteLine($"{item}: {ex}");
                            if(Cancel.IsCancellationRequested) {
                                break;
                            } else {
                                Thread.Sleep(wait);
                                continue;
                            }
                        }
                    }
                    //});
                }
            //});
            return Task.WhenAll(tasks);
        }

        public Task LoadInformationAsync(CacheSpan cacheSpan)
        {
            return Task.Run(() => {
                //Parallel.ForEach(List, item => {
                foreach(var item in List) {
                    Cancel.Token.ThrowIfCancellationRequested();

                    item.LoadInformationAsync(cacheSpan).Wait();
                }
                //});
            });
        }

    }
}
