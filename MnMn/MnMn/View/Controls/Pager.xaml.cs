using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// Pasing.xaml の相互作用ロジック
    /// </summary>
    public partial class Pager: UserControl
    {
        public Pager()
        {
            InitializeComponent();
        }

        #region PageItemsProperty

        public static readonly DependencyProperty PageItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PageItemsProperty)),
            typeof(IEnumerable),
            typeof(Pager),
            new FrameworkPropertyMetadata(default(IEnumerable), new PropertyChangedCallback(OnVideoLoadingForegroundChanged))
        );

        private static void OnVideoLoadingForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pager;
            if(control != null) {
                //control.PageItems = e.NewValue as IEnumerable;
                control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        /// <summary>
        /// <para>http://stackoverflow.com/questions/9460034/custom-itemssource-property-for-a-usercontrol?answertab=votes#tab-top</para>
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }

        }

        private void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public IEnumerable PageItems
        {
            get { return GetValue(PageItemsProperty) as IEnumerable; }
            set { SetValue(PageItemsProperty, value); }
        }

        #endregion

        #region Command

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandProperty)),
            typeof(ICommand),
            typeof(Pager),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandChanged))
        );

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pager;
            if(control != null) {
                control.Command = e.NewValue as ICommand;
            }
        }

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #endregion
    }
}
