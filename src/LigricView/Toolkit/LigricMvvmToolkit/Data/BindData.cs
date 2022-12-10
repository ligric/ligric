using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace LigricMvvmToolkit.Data
{
    public class BindData : MarkupExtension
    {
        private readonly Binding binding = new Binding();

        /// <inheritdoc cref="Binding.Source"/>>
        public object Source
        {
            get => binding.Source;
            set => binding.Source = value;
        }

        /// <inheritdoc cref="Binding.RelativeSource"/>>
        public RelativeSource RelativeSource
        {
            get => binding.RelativeSource;
            set => binding.RelativeSource = value;
        }

        /// <inheritdoc cref="Binding.Path"/>>
        public PropertyPath Path
        {
            get => binding.Path;
            set => binding.Path = value;
        }

        /// <inheritdoc cref="Binding.Mode"/>>
        public BindingMode Mode
        {
            get => binding.Mode;
            set => binding.Mode = value;
        }

        /// <inheritdoc cref="Binding.ElementName"/>>
        public string ElementName
        {
            get => binding.ElementName?.ToString();
            set => binding.ElementName = value;
        }

        /// <inheritdoc cref="Binding.ConverterParameter"/>>
        public object ConverterParameter
        {
            get => binding.ConverterParameter;
            set => binding.ConverterParameter = value;
        }

        /// <inheritdoc cref="Binding.ConverterLanguage"/>>
        public string ConverterLanguage
        {
            get => binding.ConverterLanguage;
            set => binding.ConverterLanguage = value;
        }

        /// <inheritdoc cref="Binding.Converter"/>>
        public IValueConverter Converter
        {
            get => binding.Converter;
            set => binding.Converter = value;
        }

        /// <inheritdoc cref="Binding.UpdateSourceTrigger"/>>
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => binding.UpdateSourceTrigger;
            set => binding.UpdateSourceTrigger = value;
        }

        /// <inheritdoc cref="Binding.TargetNullValue"/>>
        public object TargetNullValue
        {
            get => binding.TargetNullValue;
            set => binding.TargetNullValue = value;
        }

        /// <inheritdoc cref="Binding.FallbackValue"/>>
        public object FallbackValue
        {
            get => binding.FallbackValue;
            set => binding.FallbackValue = value;
        }

        protected override object ProvideValue()
        {
            return binding;
        }

        public BindData() { }
        public BindData(string path) => Path = new PropertyPath(path);
    }
}