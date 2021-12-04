using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace WinRTMultibinding.Foundation.Data
{
    public class MultiBindingExpression
    {
        private readonly Action _updateSource;


        public object[] DataItems { get; }

        public MultiBinding ParentMultiBinding { get; }


        private MultiBindingExpression(object[] dataItems, MultiBinding parentMultiBinding, Action updateSource)
        {
            DataItems = dataItems;
            ParentMultiBinding = parentMultiBinding;
            _updateSource = updateSource;
        }


        public void UpdateSource()
        {
            if (ParentMultiBinding.Mode > BindingMode.OneWay)
            {
                _updateSource();
            }
        }


        internal static MultiBindingExpression CreateFrom(MultiBinding parentMultiBinding, Action updateSource)
        {
            var dataItems = parentMultiBinding.Bindings.Select(binding => binding.Source).ToArray();

            return new MultiBindingExpression(dataItems, parentMultiBinding, updateSource);
        }
    }
}