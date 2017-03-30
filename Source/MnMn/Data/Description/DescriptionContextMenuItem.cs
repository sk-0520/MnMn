using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ContentTypeTextNet.MnMn.MnMn.Data.Description
{
    public class DescriptionContextMenuItem : DescriptionContextMenuBase
    {
        public DescriptionContextMenuItem(bool isDefault, string headerText, string command, string commandParameter)
        {
            IsDefault = isDefault;
            HeaderText = headerText;
            Command = command;
            CommandParameter = commandParameter;
        }

        public DescriptionContextMenuItem(bool isDefault, string headerText, string command, string commandParameter, string iconKey, string iconStyle)
            : this(isDefault, headerText, command, commandParameter)
        {
            IconKey = iconKey;
            IconStyle = iconStyle;
        }

        public DescriptionContextMenuItem(bool isDefault, string headerText, string command, string commandParameter, string iconImage)
            : this(isDefault, headerText, command, commandParameter)
        {
            IconImage = iconImage;
        }

        #region proeprty

        public bool IsDefault { get; }

        public string HeaderText { get; }
        public string Command { get; }

        public string CommandParameter { get; }

        public string IconKey { get; }
        public string IconStyle { get; }

        public string IconImage { get; }

        public bool HasIcon => (IconKey != null && IconStyle != null) || IconImage != null;

        #endregion
    }
}
