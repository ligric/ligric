using BoardsShared.Abstractions.BoardsAbstractions.Interfaces;
using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Board;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;

namespace LigricUno.Views.Pins
{
    public class NavigationMenuViewModel : DispatchedBindableBase
    {
        private string _selectedContentItem;
        private RelayCommand<string> _selectHeaderNavigationItemCommand;
        private RelayCommand<byte> _selectContentNavigationItemCommand;


        public string SelectedContentItem { get => _selectedContentItem; set => SetProperty(ref _selectedContentItem, value); }

        public ObservableCollection<string> HeaderItems { get; } = new ObservableCollection<string>() { "News", "Profile", "Messages", "Settings" };
        public ObservableCollection<byte> ContentItems { get; } = new ObservableCollection<byte>();


        public RelayCommand<string> SelectHeaderNavigationItemCommand => _selectHeaderNavigationItemCommand ?? 
            (_selectHeaderNavigationItemCommand = 
                new RelayCommand<string>(OnSelectedHeaderExecute, CanSelectedHeaderExecute));

        private void OnSelectedHeaderExecute(string parameter)
        {
            Navigation.GoTo(parameter + "Page");           
        }

        private bool CanSelectedHeaderExecute(string parameter)
        {
            return !Navigation.GetCurrentPageKey().Contains(parameter + "Page");
        }

        public RelayCommand<byte> SelectContentNavigationItemCommand => _selectContentNavigationItemCommand ??
            (_selectContentNavigationItemCommand =
                new RelayCommand<byte>(OnSelectedContentExecute, CanSelectedContentExecute));


        private void OnSelectedContentExecute(byte parameter)
        {
            if (!Navigation.GetCurrentPageKey().Contains(nameof(BoardPage)))
            {
                Navigation.GoTo(nameof(BoardPage));
            }
        }
        private bool CanSelectedContentExecute(byte parameter)
        {
            return true;
        }




        public NavigationMenuViewModel()
        {
            Navigation.PageChanged += (pageKey) => SelectHeaderNavigationItemCommand.RaiseCanExecuteChanged();

            var boardsService = IocService.ServiceProvider.GetService<IBoardsService>();
            boardsService.BoardsChanged += OnBoardsChanged;
            boardsService.AddBoard();
        }

        private void OnBoardsChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<byte, BoardsShared.CommonTypes.Entities.Board.BoardDto> e)
        {
            switch (e.Action)
            {
                case Common.EventArgs.NotifyDictionaryChangedAction.Added:
                    ContentItems.Add(e.NewValue.Id);
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Removed:
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Changed:
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Cleared:
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Initialized:
                    break;
                default:
                    break;
            }
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(SelectedContentItem))
            {
                var newValueString = newValue as string;
                if (newValueString != null)
                {
                    if (newValueString.Contains(nameof(BoardPage)))
                    {
                        Navigation.GoTo(nameof(BoardPage));
                    }
                }
            }
        }
    }
}
