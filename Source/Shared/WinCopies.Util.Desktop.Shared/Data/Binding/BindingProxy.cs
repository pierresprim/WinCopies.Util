using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using static WinCopies.Util.Desktop.UtilHelpers;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Represents a <see cref="Freezable"/> object that can be used as a <see cref="Binding"/> proxy. See the Remarks section.
    /// </summary>
    /// <typeparam name="T">The type of the data context bound to this proxy. This value is stored in the <see cref="BindingProxy{T}.DataContext"/> property.</typeparam>
    /// <remarks><para><b>Usage:</b> This class can be used in XAML when a <see cref="Binding"/> must be used to link two properties when a (relative) source must be set to a <see cref="Binding"/> object but when this (relative) source is not inherited via the visual tree, for example when binding a property of a class for which the instanciated object is assigned as the value of a property declared in a <see cref="ValidationRule"/> object. In that case, because the <see cref="ValidationRule"/> is not added to the visual tree, it does not inherit the data context. Then, a <see cref="BindingProxy{T}"/> can be added as a resource for the control that is concerned by the validation and thus can be used as the source of the <see cref="Binding"/>, that is why this class is called <see cref="BindingProxy{T}"/>. However, be careful the path you use for the <see cref="Binding"/> is like following: <see cref="DataContext"/>.&lt;the path starting from your data context&gt; because the real source is not your data context directly, but the <see cref="DataContext"/> property of this class.</para>
    /// <para><b>More info:</b> When you have to use a proxy for your <see cref="Binding"/> object, it is easier to use a non-generic type. A default one exists for this class : <see cref="BindingProxy"/>, where <typeparamref name="T"/> is sealed to <see cref="object"/>, so that this class overload can handle every .Net object.</para>
    /// </remarks>
    /// <seealso cref="BindingProxy"/>
    public class BindingProxy<T> : Freezable
    {
        /// <summary>
        /// Identifies the <see cref="DataContext"/> property.
        /// </summary>
        public static readonly DependencyProperty DataContextProperty = Register<T, BindingProxy<T>>(nameof(DataContext));

        /// <summary>
        /// Gets or sets the data context of this binding proxy.
        /// </summary>
        public T DataContext { get => (T)GetValue(DataContextProperty); set => SetValue(DataContextProperty, value); }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingProxy{T}"/> class.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected override
#if CS10
            BindingProxy<T>
#else
            Freezable
#endif
            CreateInstanceCore() => new
#if !CS10
            BindingProxy
#endif
            ();

        /// <inheritdoc/>
        protected override void CloneCore(Freezable sourceFreezable)
        {
            ((BindingProxy<T>)sourceFreezable).DataContext = DataContext;

            base.CloneCore(sourceFreezable);
        }
    }

    /// <summary>
    /// Non-generic class overload of <see cref="BindingProxy{T}"/>. See the doc for this type for more information.
    /// </summary>
    /// <seealso cref="BindingProxy{T}"/>
    public class BindingProxy : BindingProxy<object>
    {
        // Left empty.
    }

    public class DependencyObjectBindingProxy : BindingProxy<DependencyObject>
    {
        // Left empty.
    }
}
