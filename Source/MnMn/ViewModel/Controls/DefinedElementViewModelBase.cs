﻿/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    /// <summary>
    /// DefinedElementModelを表示のためになんかするViewModel。
    /// <para>DefinedElementModelの設計としてただ文字列を表示するだけなのでViewModelは不要だが、たまーに独自の何かを持たせることがあるのでその基底として使用する。</para>
    /// </summary>
    public class DefinedElementViewModelBase: SingleModelWrapperViewModelBase<IReadOnlyDefinedElement>, IReadOnlyKey
    {
        public DefinedElementViewModelBase(IReadOnlyDefinedElement model)
            : base(model)
        { }

        #region property

        public override string DisplayText
        {
            get
            {
                return Model.DisplayText;
            }
        }

        public IReadOnlyDictionary<string, string> Extends => Model.Extends;

        #endregion

        #region IReadOnlyKey

        public string Key
        {
            get
            {
                return Model.Key;
            }
        }

        #endregion
    }

    public class DefinedElementViewModelBase<TDefinedElementModel>: DefinedElementViewModelBase
        where TDefinedElementModel : IReadOnlyDefinedElement
    {
        public DefinedElementViewModelBase(TDefinedElementModel model) 
            : base(model)
        {
            DefinedModel = model;
        }

        #region property

        protected TDefinedElementModel DefinedModel { get; }

        #endregion
    }
}
