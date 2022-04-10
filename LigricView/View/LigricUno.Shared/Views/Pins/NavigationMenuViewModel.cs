using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricUno.Shared.Views.Pins
{
    public class NavigationMenuViewModel : OnNotifyPropertyChanged
    {
        private string _currentContainer;

        public string CurrentContainer { get => _currentContainer; set => SetProperty(ref _currentContainer, value); }

        public ObservableCollection<string> BoardConeiners { get; } = new ObservableCollection<string>();

        public NavigationMenuViewModel()
        {
            Navigation.ActivePagesChanged += OnActivePagesChanged;
            //var test = Navigation.GetPages();

            //foreach (var item in test)
            //{
            //    BoardConeiners.Add(item.Key);
            //}
        }

        private void OnActivePagesChanged(string pageKey)
        {
            BoardConeiners.Add(pageKey);
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(CurrentContainer))
            {
                var newValueString = newValue as string;
                if (newValueString != null)
                {
                    Navigation.GoTo(newValueString);
                }
            }
        }
    }
}
