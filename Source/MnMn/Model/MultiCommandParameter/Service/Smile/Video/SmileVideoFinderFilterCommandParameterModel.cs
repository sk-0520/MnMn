using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter.Service.Smile.Video
{
    public class SmileVideoFinderFilterCommandParameterModel: MultiCommandParameterModelBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="values">
        /// <list type="list">
        ///     <item>
        ///         <term>0</term>
        ///         <description><see cref="SmileVideoFinderFilteringTarget"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>値</description>
        ///     </item>
        /// </list>
        /// </param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        public SmileVideoFinderFilterCommandParameterModel(object[] values, Type targetType, CultureInfo culture)
            : base(values, targetType, culture)
        {
            var paramCount = 2;
            if(values.Length != paramCount) {
                throw new ArgumentException($"{nameof(values)}.{nameof(values)} != {paramCount}");
            }

            FilteringTarget = (SmileVideoFinderFilteringTarget)values[0];
            Source = values[1] as string;
        }

        #region property

        public SmileVideoFinderFilteringTarget FilteringTarget { get; }

        public string Source { get; }

        #endregion
    }
}
