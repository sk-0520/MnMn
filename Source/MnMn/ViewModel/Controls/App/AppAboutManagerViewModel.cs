using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppAboutManagerViewModel: ManagerViewModelBase
    {
        public AppAboutManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region command

        public ICommand ExecuteCommand
        {
            get
            {
                return CreateCommand(o => {
                    var s = (string)o;
                    try {
                        if(s.Any(c => c == '@')) {
                            var mail = "mailto:" + s;
                            Process.Start(mail);
                        } else {
                            Process.Start(s);
                        }
                    } catch(Exception ex) {
                        Mediation.Logger.Warning(ex);
                    }
                });
            }
        }

        #endregion

        #region ManagerViewModelBase

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
        }

        public override void UninitializeView(MainWindow view)
        {
        }

        #endregion
    }
}
