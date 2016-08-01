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

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// FontSelectControl.xaml の相互作用ロジック
    /// </summary>
    public partial class FontSelectControl: UserControl
    {
        public FontSelectControl()
        {
            InitializeComponent();
        }

        #region FamilyNameProperty

        public static readonly DependencyProperty FamilyNameProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FamilyNameProperty)),
            typeof(FontFamily),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontFamily,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnFamilyNameChanged)
            )
        );

        public FontFamily FamilyName
        {
            get { return (FontFamily)GetValue(FamilyNameProperty); }
            set { SetValue(FamilyNameProperty, value); }
        }

        static void OnFamilyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.FamilyName = (FontFamily)e.NewValue;
            }
        }

        #endregion

        #region IsBoldProperty

        public static readonly DependencyProperty IsBoldProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsBoldProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsBoldChanged)
            )
        );

        public bool IsBold
        {
            get { return (bool)GetValue(IsBoldProperty); }
            set { SetValue(IsBoldProperty, value); }
        }

        static void OnIsBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsBold = (bool)e.NewValue;
            }
        }

        #endregion

        #region IsItalicProperty

        public static readonly DependencyProperty IsItalicProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsItalicProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsItalicChanged)
            )
        );

        public bool IsItalic
        {
            get { return (bool)GetValue(IsItalicProperty); }
            set { SetValue(IsItalicProperty, value); }
        }

        static void OnIsItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsItalic = (bool)e.NewValue;
            }
        }

        #endregion

        #region SizeProperty

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SizeProperty)),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontSize,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSizeChanged)
            )
        );

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.Size = (double)e.NewValue;
            }
        }

        #endregion

        #region SizeMinimumProperty

        public static readonly DependencyProperty SizeMinimumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SizeMinimumProperty)),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnSizeMinimumChanged)
            )
        );

        public double SizeMinimum
        {
            get { return (double)GetValue(SizeMinimumProperty); }
            set { SetValue(SizeMinimumProperty, value); }
        }

        static void OnSizeMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SizeMinimum = (double)e.NewValue;
            }
        }

        #endregion

        #region SizeMaximumProperty

        public static readonly DependencyProperty SizeMaximumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SizeMaximumProperty)),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                100.0,
                new PropertyChangedCallback(OnSizeMaximumChanged)
            )
        );

        public double SizeMaximum
        {
            get { return (double)GetValue(SizeMaximumProperty); }
            set { SetValue(SizeMaximumProperty, value); }
        }

        static void OnSizeMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SizeMaximum = (double)e.NewValue;
            }
        }

        #endregion

        #region IsEnabledBoldProperty

        public static readonly DependencyProperty IsEnabledBoldProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledBoldProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledBoldChanged)
            )
        );

        public bool IsEnabledBold
        {
            get { return (bool)GetValue(IsEnabledBoldProperty); }
            set { SetValue(IsEnabledBoldProperty, value); }
        }

        static void OnIsEnabledBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsEnabledBold = (bool)e.NewValue;
            }
        }

        #endregion

        #region IsEnabledItalicProperty

        public static readonly DependencyProperty IsEnabledItalicProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledItalicProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledItalicChanged)
            )
        );

        public bool IsEnabledItalic
        {
            get { return (bool)GetValue(IsEnabledItalicProperty); }
            set { SetValue(IsEnabledItalicProperty, value); }
        }

        static void OnIsEnabledItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsEnabledItalic = (bool)e.NewValue;
            }
        }

        #endregion

        #region AdditionalContentProperty

        public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(AdditionalContentProperty)),
            typeof(object),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnAdditionalContentPropertyChanged)
            )
        );

        public object AdditionalContent
        {
            get { return GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        static void OnAdditionalContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.AdditionalContent = e.NewValue;
            }
        }

        #endregion
    }
}
