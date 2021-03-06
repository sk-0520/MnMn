﻿using System;
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

        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewMouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            this.AssociatedObject.DragOver += AssociatedObject_DragOver;
            this.AssociatedObject.DragLeave += AssociatedObject_DragLeave;
            this.AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewMouseDown -= AssociatedObject_MouseDown;
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
            DragAndDrop.MouseDown((UIElement)sender, e);
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DragAndDrop.MouseMove((UIElement)sender, e);
        }

        void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            DragAndDrop.DragEnter((UIElement)sender, e);
        }

        void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            DragAndDrop.DragOver((UIElement)sender, e);
        }

        void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            DragAndDrop.DragLeave((UIElement)sender, e);
        }

        void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            DragAndDrop.Drop((UIElement)sender, e);
        }
    }
}
