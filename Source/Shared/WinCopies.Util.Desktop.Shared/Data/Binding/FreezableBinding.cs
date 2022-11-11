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

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

namespace WinCopies.Util.Data
{
    public class FreezableBinding : Freezable
    {
        #region Properties
        private Binding
#if CS8
            ?
#endif
            _binding;

        protected Binding Binding => _binding
#if CS8
            ??=
#else
            ?? (_binding =
#endif
            new Binding()
#if !CS8
            )
#endif
            ;

        public DependencyObject TargetObject { get; internal set; }

        internal int Id { get; set; }

        [DefaultValue(null)]
        public object AsyncState { get => Binding.AsyncState; set => Binding.AsyncState = value; }

        [DefaultValue(false)]
        public bool BindsDirectlyToSource { get => Binding.BindsDirectlyToSource; set => Binding.BindsDirectlyToSource = value; }

        [DefaultValue(null)]
        public IValueConverter Converter { get => Binding.Converter; set => Binding.Converter = value; }

        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter)), DefaultValue(null)]
        public CultureInfo ConverterCulture { get => Binding.ConverterCulture; set => Binding.ConverterCulture = value; }

        [DefaultValue(null)]
        public object ConverterParameter { get => Binding.ConverterParameter; set => Binding.ConverterParameter = value; }

        [DefaultValue(null)]
        public string ElementName { get => Binding.ElementName; set => Binding.ElementName = value; }

        [DefaultValue(null)]
        public object FallbackValue { get => Binding.FallbackValue; set => Binding.FallbackValue = value; }

        [DefaultValue(false)]
        public bool IsAsync { get => Binding.IsAsync; set => Binding.IsAsync = value; }

        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode { get => Binding.Mode; set => Binding.Mode = value; }

        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated { get => Binding.NotifyOnSourceUpdated; set => Binding.NotifyOnSourceUpdated = value; }

        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated { get => Binding.NotifyOnTargetUpdated; set => Binding.NotifyOnTargetUpdated = value; }

        [DefaultValue(false)]
        public bool NotifyOnValidationError { get => Binding.NotifyOnValidationError; set => Binding.NotifyOnValidationError = value; }

        [DefaultValue(null)]
        public PropertyPath Path { get => Binding.Path; set => Binding.Path = value; }

        [DefaultValue(null)]
        public RelativeSource RelativeSource { get => Binding.RelativeSource; set => Binding.RelativeSource = value; }

        [DefaultValue(null)]
        public object Source { get => Binding.Source; set => Binding.Source = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get => Binding.UpdateSourceExceptionFilter; set => Binding.UpdateSourceExceptionFilter = value; }

        [DefaultValue(UpdateSourceTrigger.PropertyChanged)]
        public UpdateSourceTrigger UpdateSourceTrigger { get => Binding.UpdateSourceTrigger; set => Binding.UpdateSourceTrigger = value; }

        [DefaultValue(false)]
        public bool ValidatesOnDataErrors { get => Binding.ValidatesOnDataErrors; set => Binding.ValidatesOnDataErrors = value; }

        [DefaultValue(false)]
        public bool ValidatesOnExceptions { get => Binding.ValidatesOnExceptions; set => Binding.ValidatesOnExceptions = value; }

        [DefaultValue(null)]
        public string XPath { get => Binding.XPath; set => Binding.XPath = value; }

        [DefaultValue(null)]
        public Collection<ValidationRule> ValidationRules => Binding.ValidationRules;
        #endregion Properties

        #region Freezable overrides
        protected override void CloneCore(Freezable sourceFreezable)
        {
            var freezableBindingClone = (FreezableBinding)sourceFreezable;

            if (freezableBindingClone.ElementName != null)

                ElementName = freezableBindingClone.ElementName;

            else if (freezableBindingClone.RelativeSource != null)

                RelativeSource = freezableBindingClone.RelativeSource;

            else if (freezableBindingClone.Source != null)

                Source = freezableBindingClone.Source;

            AsyncState = freezableBindingClone.AsyncState;
            BindsDirectlyToSource = freezableBindingClone.BindsDirectlyToSource;
            Converter = freezableBindingClone.Converter;
            ConverterCulture = freezableBindingClone.ConverterCulture;
            ConverterParameter = freezableBindingClone.ConverterParameter;
            FallbackValue = freezableBindingClone.FallbackValue;
            IsAsync = freezableBindingClone.IsAsync;
            Mode = freezableBindingClone.Mode;
            NotifyOnSourceUpdated = freezableBindingClone.NotifyOnSourceUpdated;
            NotifyOnTargetUpdated = freezableBindingClone.NotifyOnTargetUpdated;
            NotifyOnValidationError = freezableBindingClone.NotifyOnValidationError;
            Path = freezableBindingClone.Path;
            UpdateSourceExceptionFilter = freezableBindingClone.UpdateSourceExceptionFilter;
            UpdateSourceTrigger = freezableBindingClone.UpdateSourceTrigger;
            ValidatesOnDataErrors = freezableBindingClone.ValidatesOnDataErrors;
            ValidatesOnExceptions = freezableBindingClone.ValidatesOnExceptions;
            XPath = freezableBindingClone.XPath;

            foreach (ValidationRule validationRule in freezableBindingClone.ValidationRules)

                ValidationRules.Add(validationRule);

            base.CloneCore(sourceFreezable);
        }

        protected override
#if CS10
            FreezableBinding
#else
            Freezable
#endif
            CreateInstanceCore() => new
#if !CS10
            FreezableBinding
#endif
            ();
        #endregion Freezable overrides
    }
}
