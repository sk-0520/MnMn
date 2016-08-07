/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public class ShowViewRequestModel: ViewRequestModelBase
    {
        ShowViewRequestModel(RequestKind requestKind, ServiceType serviceType, object parameter, ShowViewState showViewState)
            : base(requestKind, serviceType, parameter)
        {
            ShowViewState = showViewState;
        }

        public ShowViewRequestModel(RequestKind requestKind, ServiceType serviceType, ViewModelBase viewModelParameter, ShowViewState showViewState)
            : base(requestKind, serviceType, viewModelParameter)
        {
            ParameterIsViewModel = true;
            ViewModel = viewModelParameter;
        }

        public ShowViewRequestModel(RequestKind requestKind, ServiceType serviceType, ShowParameterModelBase showRequestParameter, ShowViewState showViewState)
            : base(requestKind, serviceType, showRequestParameter)
        {
            ParameterIsViewModel = false;
            ShowRequestParameter = showRequestParameter;
        }

        #region property

        public ShowViewState ShowViewState { get; }

        public bool ParameterIsViewModel { get; }

        public ViewModelBase ViewModel { get;}
        public ShowParameterModelBase ShowRequestParameter { get; }

        #endregion
    }
}
