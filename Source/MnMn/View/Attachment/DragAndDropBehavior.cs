using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.View.Attachment
{
    public class DragAndDropBehavior: Behavior<UIElement>
    {
        #region DragAndDropProperty

        public static readonly DependencyProperty DragAndDropProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(DragAndDropProperty)),
            typeof(IDragAndDrop),
            typeof(DragAndDropBehavior),
            new FrameworkPropertyMetadata(default(IDragAndDrop), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnDragAndDropChanged))
        );

        private static void OnDragAndDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DragAndDropBehavior;
            if(control != null) {
                control.DragAndDrop = e.NewValue as IDragAndDrop;
            }
        }

        public IDragAndDrop DragAndDrop
        {
            get { return GetValue(DragAndDropProperty) as IDragAndDrop; }
            set { SetValue(DragAndDropProperty, value); }
        }

        #endregion

        #region AllowDropProperty

        public static readonly DependencyProperty AllowDropProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(AllowDropProperty)),
            typeof(bool),
            typeof(DragAndDropBehavior),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnAllowDropChanged))
        );

        private static void OnAllowDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DragAndDropBehavior;
            if(control != null) {
                control.AllowDrop = (bool)e.NewValue;
            }
        }

        public bool AllowDrop
        {
            get { return (bool)GetValue(AllowDropProperty); }
            set { SetValue(AllowDropProperty, value); }
        }

        #endregion

        #region IsEnabledDragProperty

        public static readonly DependencyProperty IsEnabledDragProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledDragProperty)),
            typeof(bool),
            typeof(DragAndDropBehavior),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsEnabledDragChanged))
        );

        private static void OnIsEnabledDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DragAndDropBehavior;
            if(control != null) {
                control.IsEnabledDrag = (bool)e.NewValue;
            }
        }

        public bool IsEnabledDrag
        {
            get { return (bool)GetValue(IsEnabledDragProperty); }
            set { SetValue(IsEnabledDragProperty, value); }
        }

        #endregion

        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();

            AllowDrop = this.AssociatedObject.AllowDrop;

            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            this.AssociatedObject.DragOver += AssociatedObject_DragOver;
            this.AssociatedObject.DragLeave += AssociatedObject_DragLeave;
            this.AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            this.AssociatedObject.DragOver -= AssociatedObject_DragOver;
            this.AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
            this.AssociatedObject.Drop -= AssociatedObject_Drop;

            base.OnDetaching();
        }

        #endregion

        void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(IsEnabledDrag) {
                DragAndDrop.MouseDown?.Invoke((UIElement)sender, e);
            }
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(IsEnabledDrag) {
                DragAndDrop.MouseMove?.Invoke((UIElement)sender, e);
            }
        }

        void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if(IsEnabledDrag) {
                DragAndDrop.DragEnter?.Invoke((UIElement)sender, e);
            }
        }

        void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            if(IsEnabledDrag) {
                DragAndDrop.DragOver?.Invoke((UIElement)sender, e);
            }
        }

        void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if(IsEnabledDrag) {
                DragAndDrop.DragLeave?.Invoke((UIElement)sender, e);
            }
        }

        void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if(AllowDrop) {
                DragAndDrop.Drop?.Invoke((UIElement)sender, e);
            }
        }
    }
}
