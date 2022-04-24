using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
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

        public ObservableCollection<string> NavigationMenuItems { get; } = new ObservableCollection<string>() { "News", "Profile", "Settings" };
        public ObservableCollection<string> BoardConeiners { get; } = new ObservableCollection<string>();


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
            Navigation.ActivePagesChanged += OnActivePagesChanged;
            Navigation.PageChanged += (pageKey) => SelectNavigationItemCommand.RaiseCanExecuteChanged();
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
