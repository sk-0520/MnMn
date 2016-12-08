using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Data
{
    public class DescriptionContextMenuItem
    {
        public DescriptionContextMenuItem(string headerText, string command)
        {
            HeaderText = headerText;
            Command = command;
        }

        #region proeprty

        public string HeaderText { get; }
        public string Command { get; }

        #endregion
    }
}
