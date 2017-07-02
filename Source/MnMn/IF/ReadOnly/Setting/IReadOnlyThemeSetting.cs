using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting
{
    public interface IReadOnlyThemeSetting
    {
        #region property

        bool IsRandom { get; }

        string ApplicationTheme { get; }

        string BaseTheme { get; }

        string Accent { get; }

        #endregion
    }
}
