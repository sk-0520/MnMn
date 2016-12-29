using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Attachment
{
    public class HtmlDescriptionBehavior: Behavior<FlowDocumentScrollViewer>
    {
        #region HtmlSourceProperty

        public static readonly DependencyProperty HtmlSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(HtmlSourceProperty)),
            typeof(string),
            typeof(HtmlDescriptionBehavior),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnHtmlSourceChanged))
        );

        private static void OnHtmlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlDescriptionBehavior;
            if(control != null) {
                control.HtmlSource = e.NewValue as string ?? string.Empty;
                control.ChangedProperty();
            }
        }

        public string HtmlSource
        {
            get { return GetValue(HtmlSourceProperty) as string; }
            set { SetValue(HtmlSourceProperty, value); }
        }

        #endregion

        //#region TargetProperty

        //public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
        //    DependencyPropertyUtility.GetName(nameof(TargetProperty)),
        //    typeof(IDescription),
        //    typeof(HtmlDescriptionBehavior),
        //    new FrameworkPropertyMetadata(default(IDescription), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTargetChanged))
        //);

        //private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as HtmlDescriptionBehavior;
        //    if(control != null) {
        //        control.Target = e.NewValue as IDescription;
        //        control.ChangedProperty();
        //    }
        //}

        //public IDescription Target
        //{
        //    get { return GetValue(TargetProperty) as IDescription; }
        //    set { SetValue(TargetProperty, value); }
        //}

        //#endregion

        #region DescriptionProcessorProperty

        public static readonly DependencyProperty DescriptionProcessorProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(DescriptionProcessorProperty)),
            typeof(DescriptionBase),
            typeof(HtmlDescriptionBehavior),
            new FrameworkPropertyMetadata(default(DescriptionBase), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnDescriptionProcessorChanged))
        );

        private static void OnDescriptionProcessorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlDescriptionBehavior;
            if(control != null) {
                control.DescriptionProcessor = e.NewValue as DescriptionBase;
                control.ChangedProperty();
            }
        }

        public DescriptionBase DescriptionProcessor
        {
            get { return GetValue(DescriptionProcessorProperty) as DescriptionBase; }
            set { SetValue(DescriptionProcessorProperty, value); }
        }

        #endregion

        public HtmlDescriptionBehavior()
        { }

        #region function

        void ChangedProperty()
        {
            //if(Target == null) {
            //    return;
            //}

            if(DescriptionProcessor == null) {
                return;
            }

            if(AssociatedObject == null) {
                return;
            }

            MakeDescription();
        }

        void MakeDescription()
        {
            var flowDocumentSource = DescriptionProcessor.ConvertFlowDocumentFromHtml(HtmlSource);
            
            DescriptionUtility.SetDescriptionDocument(this.AssociatedObject, flowDocumentSource, new Logger());
        }


        #endregion

        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;

            base.OnDetaching();
        }

        #endregion

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            ChangedProperty();
        }

    }
}
