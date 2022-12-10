using Grpc.Net.Client;
using Ligric.Business;
using Ligric.Common.Types.Api;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Board;
using LigricUno.Views.Pages.Login;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace LigricUno.Views.Pins
{
    public class NavigationMenuViewModel
    {
        //private readonly IBoardsRepository _boardsService;

        private RelayCommand<string> _selectHeaderNavigationItemCommand, _selectHeaderOptionItemCommand;
        private RelayCommand<ApiDto?> _selectContentNavigationItemCommand;
        private RelayCommand _addNewBoardCommand;
        //private RelayCommand<BoardEntityType> _addNewBoardEntityCommand;

        public ObservableCollection<string> HeaderItems { get; } = new ObservableCollection<string>() { };
        public ObservableCollection<string> HeaderOptionItems { get; } = new ObservableCollection<string>();
        public ObservableCollection<ApiDto> ContentItems { get; } = new ObservableCollection<ApiDto>();


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
        public RelayCommand<ApiDto> SelectContentNavigationItemCommand => _selectContentNavigationItemCommand ?? (
            _selectContentNavigationItemCommand =
                new RelayCommand<ApiDto>(OnSelectedContentExecute, CanSelectedContentExecute));

        private void OnSelectedContentExecute(ApiDto parameter)
        {
        }

        private bool CanSelectedContentExecute(ApiDto? parameter)
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

        //#region AddNewBoardEntityCommand
        //public RelayCommand<BoardEntityType> AddNewBoardEntityCommand => _addNewBoardEntityCommand ?? (
        //    _addNewBoardEntityCommand =
        //        new RelayCommand<BoardEntityType>(OnAddNewBoardEntityExecute));

        //private void OnAddNewBoardEntityExecute(BoardEntityType parameter)
        //{
        //    _boardsService.CurrentBoard.AddEntity(parameter);
        //}
        //#endregion

        #endregion


        public NavigationMenuViewModel()
        {
            //_boardsService = boardsService;
            //_boardsService.BoardsChanged += OnBoardsChanged;

            Navigation.PageChanged += OnPageChanged;
        }

        private async void OnApiesChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<long, ApiDto> e)
        {
            switch(e.Action) 
            {
                case Common.EventArgs.NotifyDictionaryChangedAction.Added:
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ContentItems.Add(e.NewValue);
                    });
                    break;
            }
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

        //private void OnBoardsChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<long, IBoardRepository> e)
        //{
        //    switch (e.Action)
        //    {
        //        case Common.EventArgs.NotifyDictionaryChangedAction.Added:
        //            //ContentItems.Add(e.NewValue.Id);
        //            break;
        //        case Common.EventArgs.NotifyDictionaryChangedAction.Removed:
        //            break;
        //        case Common.EventArgs.NotifyDictionaryChangedAction.Changed:
        //            break;
        //        case Common.EventArgs.NotifyDictionaryChangedAction.Cleared:
        //            break;
        //        case Common.EventArgs.NotifyDictionaryChangedAction.Initialized:
        //            break;
        //        default:
        //            break;
        //    }

        //    SelectContentNavigationItemCommand.RaiseCanExecuteChanged();
        //}
    }
}
