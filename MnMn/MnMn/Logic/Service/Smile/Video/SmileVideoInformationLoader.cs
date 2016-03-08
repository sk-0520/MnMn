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
using System.Net.Http;
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
            var groups = List
                .GroupBy(i => i.ThumbnailUri.Host)
                .OrderByDescending(g => g.Count())
                .ToArray()
            ;
            foreach(var group in groups) {
                var groupTask = Task.Run(() => {
                    var client = new HttpClient();

                    var childTasks = new List<Task>();
                    foreach(var item in group) {
                        var task = item.LoadThumbnaiImageAsync(imageCacheSpan, client);
                        childTasks.Add(task);
                    }

                    return Task.WhenAll(childTasks).ContinueWith(_ => {
                        client.Dispose();
                    });
                });
                tasks.Add(groupTask);

                // 地味に待つ
                var wait = TimeSpan.FromMilliseconds(250);
                Thread.Sleep(wait);
            }

            return Task.WhenAll(tasks);
        }

        public Task LoadInformationAsync(CacheSpan cacheSpan)
        {
            var tasks = new List<Task>();
            
            //return Task.Run(() => {
            //Parallel.ForEach(List, item => {
            foreach(var item in List) {
                    Cancel.Token.ThrowIfCancellationRequested();

                var task = item.LoadInformationAsync(cacheSpan);
                tasks.Add(task);
                }
                //});
            //});
            return Task.WhenAll(tasks);
        }

    }
}
