using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages;
using LigricUno.Views.Pages.News;
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


        private RelayCommand _newsCommand, _profileCommand, _boardsCommand, _settingsCommand;
        public RelayCommand NewsCommand => _newsCommand ?? (_newsCommand = new RelayCommand(() => Navigation.GoTo(nameof(NewsPage)), () => !Navigation.GetCurrentPageKey().Contains(nameof(NewsPage))));
        public RelayCommand ProfileCommand => _profileCommand ?? (_profileCommand = new RelayCommand(() => Navigation.GoTo(nameof(NewsPage)), () => !Navigation.GetCurrentPageKey().Contains(nameof(NewsPage))));
        public RelayCommand BoardsCommand => _boardsCommand ?? (_boardsCommand = new RelayCommand(() => Navigation.GoTo(nameof(NewsPage)), () => !Navigation.GetCurrentPageKey().Contains(nameof(NewsPage))));
        public RelayCommand SettingsCommand => _settingsCommand ?? (_settingsCommand = new RelayCommand(() => Navigation.GoTo(nameof(NewsPage)), () => !Navigation.GetCurrentPageKey().Contains(nameof(NewsPage))));


        public NavigationMenuViewModel()
        {
            Navigation.ActivePagesChanged += OnActivePagesChanged;
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
