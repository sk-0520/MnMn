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
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoManagerViewModelBase: ViewModelBase
    {
        #region variable

        bool _visible;

        #endregion

        public SmileVideoManagerViewModelBase(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        protected Mediation Mediation { get; }

        public bool Visible
        {
            get { return this._visible; }
            set
            {
                if(SetVariableValue(ref this._visible, value)) {
                    if(this._visible) {
                        ShowView();
                    } else {
                        HideView();
                    }
                }
            }
        }

        #endregion

        #region function

        protected virtual void ShowView()
        {

        }

        protected virtual void HideView()
        {

        }

        #endregion
    }
}
