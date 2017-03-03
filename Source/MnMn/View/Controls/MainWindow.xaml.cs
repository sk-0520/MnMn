﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.View.Window;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using MahApps.Metro.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow: MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            scale.Value = 1;
            selectScale.IsChecked = false;
        }
    }
}
