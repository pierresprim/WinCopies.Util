/*
 Original source code: Fredrik Hedblad (https://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/)

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org> */

using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    public class PushBinding : FreezableBinding, System. IDisposable
    {
        #region Dependency Properties 

        public static DependencyProperty TargetPropertyMirrorProperty =
            DependencyProperty.Register(nameof(TargetPropertyMirror),
                                        typeof(object),
                                        typeof(PushBinding));

        public static DependencyProperty TargetPropertyListenerProperty =
            DependencyProperty.Register(nameof(TargetPropertyListener),
                                        typeof(object),
                                        typeof(PushBinding),
                                        new UIPropertyMetadata(null, OnTargetPropertyListenerChanged));

        private static void OnTargetPropertyListenerChanged(object sender, DependencyPropertyChangedEventArgs e) => (sender as PushBinding).TargetPropertyValueChanged();

        #endregion // Dependency Properties

        #region Constructor

        public PushBinding() => Mode = BindingMode.OneWayToSource;

        #endregion // Constructor

        #region Properties

        public object TargetPropertyMirror
        {
            get => GetValue(TargetPropertyMirrorProperty);
            set => SetValue(TargetPropertyMirrorProperty, value);
        }

        public object TargetPropertyListener
        {
            get => GetValue(dp: TargetPropertyListenerProperty);
            set => SetValue(TargetPropertyListenerProperty, value);
        }

        [DefaultValue(null)]
        public string TargetProperty { get; set; }

        [DefaultValue(null)]
        public DependencyProperty TargetDependencyProperty { get; set; }

        #endregion // Properties

        #region Public Methods

        public void SetupTargetBinding()
        {
            // Prevent the designer from reporting exceptions since
            // changes will be made of a Binding in use if it is set

            if (TargetObject == null|| DesignerProperties.GetIsInDesignMode(this))

                return;

            // Bind to the selected TargetProperty, e.g. ActualHeight and get
            // notified about changes in OnTargetPropertyListenerChanged
            var listenerBinding = new Binding
            {
                Source = TargetObject,
                Mode = BindingMode.OneWay
            };

            listenerBinding.Path = TargetDependencyProperty == null ? new PropertyPath(TargetProperty) : new PropertyPath(TargetDependencyProperty);

            _ = BindingOperations.SetBinding(this, TargetPropertyListenerProperty, listenerBinding);

            // Set up a OneWayToSource Binding with the Binding declared in Xaml from
            // the Mirror property of this class. The mirror property will be updated
            // everytime the Listener property gets updated
            _ = BindingOperations.SetBinding(this, TargetPropertyMirrorProperty, Binding);

            TargetPropertyValueChanged();

            if (TargetObject is FrameworkElement _targetObject)

                _targetObject.Loaded += DependencyObject_Loaded;

            else if (TargetObject is FrameworkContentElement __targetObject)

                __targetObject.Loaded += DependencyObject_Loaded;
        }

        private void DependencyObject_Loaded(object sender, RoutedEventArgs e) => TargetPropertyValueChanged();

        #endregion // Public Methods

        #region Private Methods

        private void TargetPropertyValueChanged() => SetValue(TargetPropertyMirrorProperty, GetValue(TargetPropertyListenerProperty));

        #endregion // Private Methods

        #region Freezable overrides

        protected override void CloneCore(Freezable sourceFreezable)
        {
            var pushBinding = sourceFreezable as PushBinding;
            TargetProperty = pushBinding.TargetProperty;
            TargetDependencyProperty = pushBinding.TargetDependencyProperty;
            base.CloneCore(sourceFreezable);
        }

        protected override Freezable CreateInstanceCore() => new PushBinding();

        #endregion // Freezable overrides

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, TargetPropertyListenerProperty);

            BindingOperations.ClearBinding(this, TargetPropertyMirrorProperty);

            if (TargetObject is FrameworkElement _targetObject)

                _targetObject.Loaded -= DependencyObject_Loaded;

            else if (TargetObject is FrameworkContentElement __targetObject)

                __targetObject.Loaded -= DependencyObject_Loaded;
        }
    }
}
