using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LigricViewModels.BaseMvvmTypes
{
    public abstract class OnNotifyPropertyChanged: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
 
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T propertyFiled, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(propertyFiled, newValue))
            {
                T oldValue = propertyFiled;
                propertyFiled = newValue;
                RaisePropertyChanged(propertyName);
 
                OnPropertyChanged(propertyName, oldValue, newValue);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) { }
    }
}
