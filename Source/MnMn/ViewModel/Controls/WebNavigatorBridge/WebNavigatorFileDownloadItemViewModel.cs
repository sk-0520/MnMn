using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorFileDownloadItemViewModel : DownloadItemViewModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="userAgentCreator"></param>
        public WebNavigatorFileDownloadItemViewModel(Mediation mediation, Uri uri, FileInfo downloadFile, ICreateHttpUserAgent userAgentCreator)
            : base(mediation, uri, downloadFile, userAgentCreator)
        { }
    }
}
