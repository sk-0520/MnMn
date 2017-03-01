﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public class CrashReportSettingModel: ModelBase
    {
        #region property

        public string CacheDirectoryPath { get; set; }
        public TimeSpan CacheLifeTime { get; set; }

        #endregion
    }
}
