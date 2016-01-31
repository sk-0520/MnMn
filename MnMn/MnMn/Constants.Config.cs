using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// App.config関係。
    /// </summary>
    partial class Constants
    {
        #region niconico

        #region niconico-video

        public static long ServiceNicoNicoVideoVideoPlayLowestSize { get { return long.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-PlayLowestSize"]); } }
        public static int ServiceNicoNicoVideoVideoReceiveBuffer { get { return int.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-ReceiveBuffer"]); } }
        
        #endregion

        #endregion
    }
}
