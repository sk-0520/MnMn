using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video
{
    /// <summary>
    /// VideoList.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoFinderControl: UserControl
    {
        public SmileVideoFinderControl()
        {
            InitializeComponent();
        }

        #region HeaderContentProperty

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(HeaderContentProperty)),
            typeof(object),
            typeof(SmileVideoFinderControl),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnHeaderContentChanged))
        );

        private static void OnHeaderContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderControl;
            if(control != null) {
                control.HeaderContent = e.NewValue;
            }
        }

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        #endregion

        #region FooterContentProperty

        public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FooterContentProperty)),
            typeof(object),
            typeof(SmileVideoFinderControl),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnFooterContentChanged))
        );

        private static void OnFooterContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderControl;
            if(control != null) {
                control.FooterContent = e.NewValue;
            }
        }

        public object FooterContent
        {
            get { return GetValue(FooterContentProperty); }
            set { SetValue(FooterContentProperty, value); }
        }

        #endregion

        #region CheckableProperty

        public static readonly DependencyProperty CheckableProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CheckableProperty)),
            typeof(bool),
            typeof(SmileVideoFinderControl),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnFCheckableChanged))
        );

        private static void OnFCheckableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderControl;
            if(control != null) {
                control.Checkable = (bool)e.NewValue;
            }
        }

        public bool Checkable
        {
            get { return (bool)GetValue(CheckableProperty); }
            set { SetValue(CheckableProperty, value); }
        }

        #endregion
    }
}
