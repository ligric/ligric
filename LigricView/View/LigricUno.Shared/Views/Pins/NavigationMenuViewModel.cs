using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LigricUno.Shared.Views.Pins
{
    public class NavigationMenuViewModel : OnNotifyPropertyChanged
    {
        private string _currentContainer;

        public string CurrentContainer { get => _currentContainer; private set => SetProperty(ref _currentContainer, value); }

        public ObservableCollection<string> BoardConeiners { get; } = new ObservableCollection<string>();


        private RelayCommand<string> _selectedBoardCommand;
        public RelayCommand<string> SelectedBoardCommand => _selectedBoardCommand ?? (_selectedBoardCommand = new RelayCommand<string>(SelectedBoardExecute));

        private void SelectedBoardExecute(string parameter)
        {
            Navigation.GoTo(parameter);
        }

        public NavigationMenuViewModel()
        {
            var test = Navigation.GetPages();

            foreach (var item in test)
            {
                BoardConeiners.Add(item.Key);
            }
        }
    }
}
