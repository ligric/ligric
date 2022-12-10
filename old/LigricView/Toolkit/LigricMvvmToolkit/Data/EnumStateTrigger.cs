using System;
using System.Linq;
using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Data
{
    public class EnumStateTrigger : StateTriggerBase
    {
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object),
            typeof(EnumStateTrigger),
            new PropertyMetadata(null, ValuePropertyChanged));

        public string ActiveValues
        {
            get { return (string)GetValue(ActiveValuesProperty); }
            set { SetValue(ActiveValuesProperty, value); }
        }

        public static readonly DependencyProperty ActiveValuesProperty =
            DependencyProperty.Register("ActiveValues", typeof(string),
            typeof(EnumStateTrigger),
            new PropertyMetadata(null, ValuePropertyChanged));

        private static void ValuePropertyChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = (EnumStateTrigger)sender;
            obj.UpdateTrigger();
        }

        private void UpdateTrigger()
        {
            if (Value == null || ActiveValues == null)
            {
                SetActive(false);
            }
            else
            {
                var currentStates = Value.ToString().ToLower()
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList();

                var stateStrings = ActiveValues.ToString().ToLower()
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList();

                var isActive = currentStates.Intersect(stateStrings).Any();
                SetActive(isActive);
            }
        }
    }
}
