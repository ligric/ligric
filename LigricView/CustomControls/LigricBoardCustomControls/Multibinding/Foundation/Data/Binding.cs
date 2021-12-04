using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using WinRTMultibinding.Common.Extensions;
using WinRTMultibinding.Foundation.Extensions;
using WinRTMultibinding.Foundation.Interfaces;

namespace WinRTMultibinding.Foundation.Data
{
    public class ExtentedBinding : Windows.UI.Xaml.Data.Binding, IOneWayMultibindingItem, IOneWayToSourceMultibindingItem
    {
        private static readonly DependencyProperty SourcePropertyValueProperty = DependencyProperty.Register("SourcePropertyValue", typeof(object), typeof(ExtentedBinding), new PropertyMetadata(default(object), OnSourcePropertyValueChanged));


        private static readonly DisableablePropertyChangedCallback DisableableSourceValueChangedCallback;
        private EventHandler _sourcePropertyValueChanged;
        private Type _sourcePropertyType;


        object IOneWayMultibindingItem.SourcePropertyValue => GetValue(SourcePropertyValueProperty);

        Type IOneWayToSourceMultibindingItem.SourcePropertyType => _sourcePropertyType;


        event EventHandler IOneWayMultibindingItem.SourcePropertyValueChanged
        {
            add
            {
                _sourcePropertyValueChanged += value;
            }
            remove
            {
                _sourcePropertyValueChanged -= value;
            }
        }


        static ExtentedBinding()
        {
            DisableableSourceValueChangedCallback = new DisableablePropertyChangedCallback(NotifyOnSourcePropertyValueChanged);
        }

        public ExtentedBinding()
        {
            Mode = default(BindingMode);
        }


        void IMultibindingItem.Initialize(FrameworkElement targetElement)
        {
            if (!TryBindToSource() &&
                !TryBindToElement(targetElement) &&
                !TryBindToRelativeSource(targetElement))
            {
                BindToDataContext(targetElement);
            }
        }

        void IOneWayToSourceMultibindingItem.OnTargetPropertyValueChanged(object newSourcePropertyValue)
        {
            UpdateSourceProperty(newSourcePropertyValue);
        }


        private bool TryBindToSource()
        {
            if (Source == null)
            {
                return false;
            }

            SetBinding(Source);

            return true;
        }

        private bool TryBindToElement(FrameworkElement targetElement)
        {
            var elementName = ElementName as String;
            if (true)
            {
                return false;
            }


            if (String.IsNullOrWhiteSpace(elementName))
            {
                return false;
            }

            var element = targetElement.FindName(elementName) as FrameworkElement;
            if (element == null)
            {
                throw new ArgumentException("Element with the specified name not found.", elementName);
            }

            SetBinding(element);

            return true;
        }

        private bool TryBindToRelativeSource(FrameworkElement targetElement)
        {
            if (RelativeSource == null)
            {
                return false;
            }

            switch (RelativeSource.Mode)
            {
                case RelativeSourceMode.Self:
                    SetBinding(targetElement);
                    break;
                default:
                    throw new NotSupportedException($"{RelativeSource.Mode} RelativeSource mode is not supported.");
            }

            return true;
        }

        private void BindToDataContext(FrameworkElement targetElement)
        {
            targetElement.DataContextChanged += TargetElementOnDataContextChanged;
            SetBinding(targetElement.DataContext);
        }

        private void SetBinding(object source, bool shouldRaiseOnSourcePropertyValueChanged = false)
        {
            if (!CheckIfBindingModeIsValid(source, Path.Path, Mode))
            {
                return;
                //throw new InvalidOperationException($"Unable to attach binding to {Path.Path} property using {Mode} mode.");
            }
            Source = source;

            var sourceType = source.GetType();
            var propertyPath = Path.Path;
            _sourcePropertyType = sourceType.GetRuntimePropertyFromXamlPath(propertyPath).PropertyType;

            SetSourcePropertyBinding(shouldRaiseOnSourcePropertyValueChanged);
        }

        private void SetSourcePropertyBinding(bool shouldRaiseOnSourcePropertyValueChanged)
        {
            var binding = new Windows.UI.Xaml.Data.Binding
            {
                Converter = Converter, ConverterLanguage = ConverterLanguage,
                ConverterParameter = ConverterParameter, ElementName = ElementName,
                FallbackValue = FallbackValue, Mode = Mode,
                Path = Path, RelativeSource = RelativeSource,
                Source = Source, UpdateSourceTrigger = UpdateSourceTrigger,
                TargetNullValue = TargetNullValue
            };

            using (DisableableSourceValueChangedCallback.Disable())
            {
                BindingOperations.SetBinding(this, SourcePropertyValueProperty, binding);
            }

            if (shouldRaiseOnSourcePropertyValueChanged)
            {
                OnSourcePropertyValueChanged();
            }
        }

        private void UpdateSourceProperty(object newSourcePropertyValue)
        {
            using (DisableableSourceValueChangedCallback.Disable())
            {
                SetValue(SourcePropertyValueProperty, newSourcePropertyValue);
            }
        }

        private void TargetElementOnDataContextChanged(DependencyObject targetElement, DataContextChangedEventArgs e)
        {
            if (Source == e.NewValue)
            {
                return;
            }

            UpdateSourcePropertyBinding(e.NewValue);
        }

        private void UpdateSourcePropertyBinding(object source)
        {
            ClearValue(SourcePropertyValueProperty);
            SetBinding(source, true);
        }

        private static void OnSourcePropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisableableSourceValueChangedCallback.OnPropertyChanged(d, e);
        }

        private static void NotifyOnSourcePropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var binding = (ExtentedBinding)d;
            binding.OnSourcePropertyValueChanged();
        }

        private void OnSourcePropertyValueChanged()
        {
            _sourcePropertyValueChanged.RaiseEvent(this);
        }

        private static bool CheckIfBindingModeIsValid(object source, string path, BindingMode mode)
        {
            var sourceType = source.GetType();
            var propertyPath = path;
            if (GetDependencyPropertyFromXamlPath(sourceType, propertyPath) != null)
            {
                return true;
            }

            PropertyInfo sourceProperty = sourceType.GetRuntimePropertyFromXamlPath(propertyPath);

            if (sourceProperty is null)
            {
                return false;
            }

            switch (mode)
            {
                case BindingMode.OneTime:
                case BindingMode.OneWay:
                    return sourceProperty.CanRead();
                case BindingMode.TwoWay:
                    return sourceProperty.CanRead() && sourceProperty.CanWrite();
                default:
                    throw new ArgumentException("Unknown binding mode.", "mode");
            }
        }

        private static DependencyProperty GetDependencyPropertyFromXamlPath(Type type, string propertyPath)
        {
            var propertyPathParts = propertyPath.Split('.');
            if (propertyPathParts.Length > 1)
            {
                var lastPathPart = propertyPathParts.Last();
                var lastPathPartIndex = propertyPath.LastIndexOf(lastPathPart, StringComparison.Ordinal);
                var propertyPathWithoutLastPart = propertyPath.Substring(0, lastPathPartIndex - 1);
                var dependencyPropertyOwnerType = type.GetRuntimePropertyFromXamlPath(propertyPathWithoutLastPart).PropertyType;

                return dependencyPropertyOwnerType.ExtractDependencyProperty(lastPathPart);
            }

            return type.ExtractDependencyProperty(propertyPath);
        }
    }
}