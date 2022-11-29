using BoardsCommon.Enums;
using BoardsCore.Abstractions.BoardsAbstractions.Interfaces;
using BoardsCore.Board;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Board;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Uno.Extensions;

namespace LigricUno.Views.Pins
{
    public class NavigationMenuViewModel
    {
        private readonly IBoardsService _boardsService;

        private RelayCommand<string> _selectHeaderNavigationItemCommand, _selectHeaderOptionItemCommand;
        private RelayCommand<byte?> _selectContentNavigationItemCommand;
        private RelayCommand _addNewBoardCommand;
        private RelayCommand<BoardEntityType> _addNewBoardEntityCommand;

        public ApiKeyViewModel AddingApi { get; } = new ApiKeyViewModel();
        public ObservableCollection<string> HeaderItems { get; } = new ObservableCollection<string>() { };
        public ObservableCollection<string> HeaderOptionItems { get; } = new ObservableCollection<string>();
        public ObservableCollection<byte> ContentItems { get; } = new ObservableCollection<byte>();


        #region Commands

        #region SelectHeaderNavigationItemCommand
        public RelayCommand<string> SelectHeaderNavigationItemCommand => _selectHeaderNavigationItemCommand ?? (
            _selectHeaderNavigationItemCommand =
                new RelayCommand<string>(OnSelectedHeaderExecute, CanSelectedHeaderExecute));

        private void OnSelectedHeaderExecute(string parameter)
        { 
            Navigation.GoTo(parameter + "Page");
        }

        private bool CanSelectedHeaderExecute(string parameter)
        {
            return !Navigation.GetCurrentPageKey().Contains(parameter + "Page");
        }
        #endregion

        #region SelectContentNavigationItemCommand
        public RelayCommand<byte?> SelectContentNavigationItemCommand => _selectContentNavigationItemCommand ?? (
            _selectContentNavigationItemCommand =
                new RelayCommand<byte?>(OnSelectedContentExecute, CanSelectedContentExecute));

        private void OnSelectedContentExecute(byte? parameter)
        {
            //_boardsService.SetNewCurrentBoard((byte)parameter);

            if (!Navigation.GetCurrentPageKey().Contains(nameof(BoardPage)))
            {
                Navigation.GoTo(nameof(BoardPage));
            }
        }

        private bool CanSelectedContentExecute(byte? parameter)
        {
            if (parameter is null)
                return false;

            return true;
        }
        #endregion

        #region SelectHeaderOptionItemCommand
        public RelayCommand<string> SelectHeaderOptionItemCommand => _selectHeaderOptionItemCommand ?? (
            _selectHeaderOptionItemCommand =
                new RelayCommand<string>(OnSelectedHeaderOptionalItemExecute));

        private void OnSelectedHeaderOptionalItemExecute(string parameter)
        {
        }
        #endregion

        #region AddNewBoardCommand
        public RelayCommand AddNewBoardCommand => _addNewBoardCommand ?? (
            _addNewBoardCommand =
                new RelayCommand(OnAddNewElementExecute, CanAddNewElementExecute));

        private void OnAddNewElementExecute(object parameter)
        {
            _boardsService.AddBoard();
            AddingApi.Name = "";
            AddingApi.PublicKey = "";
            AddingApi.PrivateKey = "";
        }      
        
        private bool CanAddNewElementExecute(object parameter)
        {
            if (string.IsNullOrEmpty(AddingApi?.PublicKey)
                || string.IsNullOrEmpty(AddingApi?.Name)
                || string.IsNullOrEmpty(AddingApi?.PrivateKey))
                return false;

            return true;
        }
        #endregion

        #region AddNewBoardEntityCommand
        public RelayCommand<BoardEntityType> AddNewBoardEntityCommand => _addNewBoardEntityCommand ?? (
            _addNewBoardEntityCommand =
                new RelayCommand<BoardEntityType>(OnAddNewBoardEntityExecute));

        private void OnAddNewBoardEntityExecute(BoardEntityType parameter)
        {
            _boardsService.CurrentBoard.AddEntity(parameter);
        }
        #endregion

        #endregion


        public NavigationMenuViewModel(IBoardsService boardsService)
        {
            _boardsService = boardsService;
            _boardsService.BoardsChanged += OnBoardsChanged;
            _boardsService.AddBoard();

            Navigation.PageChanged += OnPageChanged;

            AddingApi.PropertyChanged += (s, e) =>
            {
                AddNewBoardCommand.RaiseCanExecuteChanged();
            };
        }

        private void OnPageChanged(string obj)
        {
            if (string.Equals(obj, nameof(BoardPage)))
            {
                HeaderOptionItems.Clear();
                HeaderOptionItems.Add("BoardSettings");
            }
            else
            {
                HeaderOptionItems.Clear();
            }
            SelectHeaderNavigationItemCommand.RaiseCanExecuteChanged();
        }

        private void OnBoardsChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<byte, BoardService> e)
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

            SelectContentNavigationItemCommand.RaiseCanExecuteChanged();
        }
    }
}
