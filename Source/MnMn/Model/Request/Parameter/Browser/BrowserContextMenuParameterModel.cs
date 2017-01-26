﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.Browser
{
    public class BrowserContextMenuParameterModel: BrowserClickParameterModel
    {
        public BrowserContextMenuParameterModel(Uri currentUri, EventArgs e, WebNavigatorEngine engine)
            : base(currentUri, e, engine)
        {
        }
    }
}
