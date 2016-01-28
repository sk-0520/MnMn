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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        public MainViewModel()
        {
            Mediation = new Mediation();

            NicoNicoManager = new NicoNicoManagerViewModel(Mediation);
        }
        #region property

        Mediation Mediation { get; set; }

        public NicoNicoManagerViewModel NicoNicoManager { get; private set; }

        #endregion

        #region command

        public ICommand Temp_OpenNicoNicoPlayerCommand
        {
            get
            {
                return CreateCommand(
                    o => {

                        NicoNicoManager.VideoManager.Temp_OpenPlayer("sm15218544");
                    }
                );
            }
        }

        #endregion

        #region function
        #endregion
    }
}
