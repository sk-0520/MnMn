﻿using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// ScaleSlider.xaml の相互作用ロジック
    /// </summary>
    public partial class ScaleSlider : UserControl
    {
        public ScaleSlider()
        {
            InitializeComponent();
        }

        #region DependencyProperty

        #region MinimumProperty

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MinimumProperty)),
            typeof(double),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleRange.Head, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnMinimumChanged))
        );

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleSlider;
            if (control != null)
            {
                control.Minimum = (double)e.NewValue;
            }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        #endregion

        #region MaximumProperty

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MaximumProperty)),
            typeof(double),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleRange.Tail, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnMaximumChanged))
        );

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleSlider;
            if (control != null)
            {
                control.Maximum = (double)e.NewValue;
            }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        #endregion

        #region ResetValueProperty

        public static readonly DependencyProperty ResetValueProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ResetValueProperty)),
            typeof(double),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleReset, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnResetValueChanged))
        );

        private static void OnResetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleSlider;
            if (control != null)
            {
                control.ResetValue = (double)e.NewValue;
            }
        }

        public double ResetValue
        {
            get { return (double)GetValue(ResetValueProperty); }
            set { SetValue(ResetValueProperty, value); }
        }

        #endregion

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ValueProperty)),
            typeof(double),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleReset, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnValueChanged))
        );

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleSlider;
            if (control != null)
            {
                control.Value = (double)e.NewValue;
            }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region SmallChangeProperty

        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SmallChangeProperty)),
            typeof(double),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleChangeRange.Head, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSmallChangeChanged))
        );

        private static void OnSmallChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleSlider;
            if (control != null) {
                control.SmallChange = (double)e.NewValue;
            }
        }

        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        #endregion

        #region LargeChangeProperty

        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(LargeChangeProperty)),
            typeof(double),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleChangeRange.Tail, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnLargeChangeChanged))
        );

        private static void OnLargeChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleSlider;
            if (control != null) {
                control.LargeChange = (double)e.NewValue;
            }
        }

        public double LargeChange
        {
            get { return (double)GetValue(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }

        #endregion

        #region ItemsSourceProperty

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ItemsSourceProperty)),
            typeof(IEnumerable),
            typeof(ScaleSlider),
            new FrameworkPropertyMetadata(Constants.ViewScaleList, new PropertyChangedCallback(OnItemsSourceChanged))
        );

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<ScaleSlider>(d, control => {
                control.ItemsSource = e.NewValue as IEnumerable<double>;
            });
        }

        public IEnumerable<double> ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as ObservableCollection<double>; }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #endregion

        #region property
        #endregion

        #region command

        public ICommand ScaleChangeCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    var scale = (double)o;
                    Value = scale;
                    this.commandScale.IsChecked = false;
                });
            }
        }

        public ICommand ScaleDownCommand
        {
            get
            {
                return new DelegateCommand(
                    o => Value = GetApproximateRange().Head,
                    o => Minimum < Value
                );
            }
        }

        public ICommand ScaleUpCommand
        {
            get
            {
                return new DelegateCommand(
                    o => Value = GetApproximateRange().Tail,
                    o => Value < Maximum
                );
            }
        }

        public ICommand ScaleResetCommand
        {
            get
            {
                return new DelegateCommand(
                    o => {
                        Value = ResetValue;
                        this.commandScale.IsChecked = false;
                    },
                    o => Value != ResetValue
                );
            }
        }

        #endregion

        #region function

        IReadOnlyRange<double> GetApproximateRange()
        {
            var width = (int)(Maximum * 100) - (int)(Minimum * 100);
            var nums = width / (int)(SmallChange * 100);
            var blocks = Enumerable.Range(1, nums)
                .Select(n => SmallChange + (n * SmallChange))
                .ToList()
            ;
            var small = blocks.FindLast(n => n < Value);
            if(small < Minimum) {
                small = Minimum;
            }
            var big = blocks.Find(n => Value < n);
            if (big < Minimum || Maximum < big) {
                big = Maximum;
            }

            return RangeModel.Create(small, big);
        }

        #endregion

    }
}
