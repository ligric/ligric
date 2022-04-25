﻿using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Boards;
using LigricUno.Views.Pages.News;
using LigricUno.Views.Pages.Profile;
using System;
using System.Collections.ObjectModel;

namespace LigricUno.Views.Pins
{
    public class NavigationMenuViewModel : DispatchedBindableBase
    {
        private string _currentContainer;

        public string CurrentContainer { get => _currentContainer; set => SetProperty(ref _currentContainer, value); }

        public ObservableCollection<string> HeaderItems { get; } = new ObservableCollection<string>() { "News", "Profile", "Messages", "Settings" };
        public ObservableCollection<string> ContentItems { get; } = new ObservableCollection<string>();


        private RelayCommand<string> _selectNavigationItemCommand;
        public RelayCommand<string> SelectNavigationItemCommand => _selectNavigationItemCommand ?? (_selectNavigationItemCommand = new RelayCommand<string>(OnSelectExecute, CanSelectExecute));

        private void OnSelectExecute(string parameter)
        {
            Navigation.GoTo(parameter + "Page");
        }
        private bool CanSelectExecute(string parameter)
        {
            return !Navigation.GetCurrentPageKey().Contains(parameter + "Page");
        }

        public NavigationMenuViewModel()
        {
            Navigation.PageChanged += (pageKey) => SelectNavigationItemCommand.RaiseCanExecuteChanged();
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(CurrentContainer))
            {
                var newValueString = newValue as string;
                if (newValueString != null)
                {
                    if (newValueString.Contains(nameof(BoardsPage)))
                    {
                        Navigation.GoTo(nameof(BoardsPage));
                    }
                }
            }
        }
    }
}
