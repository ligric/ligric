using LigricMvvmToolkit.BaseMvvm;
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
        private string _selectedContentItem;

        public string SelectedContentItem { get => _selectedContentItem; set => SetProperty(ref _selectedContentItem, value); }

        public ObservableCollection<string> HeaderItems { get; } = new ObservableCollection<string>() { "News", "Profile", "Messages", "Settings" };
        public ObservableCollection<string> ContentItems { get; } = new ObservableCollection<string>() { "Board0", "Board1", "Board2", "Board4", "Board0", "Board1", "Board2", "Board4", "Board0", "Board1", "Board2", "Board4" };


        private RelayCommand<string> _selectNavigationItemCommand;
        public RelayCommand<string> SelectNavigationItemCommand => _selectNavigationItemCommand ?? (_selectNavigationItemCommand = new RelayCommand<string>(OnSelectedExecute, CanSelectedExecute));

        private void OnSelectedExecute(string parameter)
        {
            Navigation.GoTo(parameter + "Page");
        }
        private bool CanSelectedExecute(string parameter)
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

            if (propertyName == nameof(SelectedContentItem))
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
