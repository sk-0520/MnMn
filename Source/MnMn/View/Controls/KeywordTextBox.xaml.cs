using System;
using System.Collections;
using System.Collections.Generic;
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
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// KeywordTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class KeywordTextBox : UserControl
    {
        public KeywordTextBox()
        {
            InitializeComponent();
        }

        #region KeywordItemsSourceProperty

        public static readonly DependencyProperty KeywordItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(KeywordItemsSourceProperty)),
            typeof(IEnumerable),
            typeof(KeywordTextBox),
            new FrameworkPropertyMetadata(default(IEnumerable), new PropertyChangedCallback(OnKeywordItemsSourceChanged))
        );

        private static void OnKeywordItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as KeywordTextBox;
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
            if(e.Action == NotifyCollectionChangedAction.Add) {
            }
        }

        public IEnumerable KeywordItemsSource
        {
            get { return GetValue(KeywordItemsSourceProperty) as IEnumerable; }
            set { SetValue(KeywordItemsSourceProperty, value); }
        }

        #endregion

        #region TextProperty

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(TextProperty)),
            typeof(string),
            typeof(KeywordTextBox),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextChanged))
        );

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as KeywordTextBox;
            if(control != null) {
                control.Text = (string)e.NewValue;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region IsReadOnlyProperty

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsReadOnlyProperty)),
            typeof(bool),
            typeof(KeywordTextBox),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsReadOnlyChanged))
        );

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as KeywordTextBox;
            if(control != null) {
                control.IsReadOnly = (bool)e.NewValue;
            }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        #endregion

        #region function

        void SetKeyword(string value)
        {
            var text = this.inputValue.Text;
            var selectionStart = this.inputValue.SelectionStart;
            var selectionLength = this.inputValue.SelectionLength;
            if(selectionStart != -1) {
                var head = text.Substring(0, selectionStart);
                var tail = text.Substring(selectionStart + selectionLength, text.Length - (selectionStart + selectionLength));
                this.inputValue.Text = head + value + tail;
                this.inputValue.SelectionStart = head.Length;
                this.inputValue.SelectionLength = value.Length;
            }
        }

        #endregion

        private void AddKeywordButton_Click(object sender, RoutedEventArgs e)
        {
            var senderElement = (FrameworkElement)sender;
            var keywordTextItem = (IReadOnlyKeywordTextItem)senderElement.DataContext;
            SetKeyword(keywordTextItem.Keyword);
        }

        private void inputValue_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
