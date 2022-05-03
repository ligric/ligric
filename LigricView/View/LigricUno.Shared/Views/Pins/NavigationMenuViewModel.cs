﻿using BoardsCore.Abstractions.BoardsAbstractions.Interfaces;
using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Board;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LigricUno.Views.Pins
{
    public class NavigationMenuViewModel : DispatchedBindableBase
    {
        private RelayCommand<string> _selectHeaderNavigationItemCommand, _selectHeaderOptionItemCommand;
        private RelayCommand<byte?> _selectContentNavigationItemCommand;

        public ObservableCollection<string> HeaderItems { get; } = new ObservableCollection<string>() { "News", "Profile", "Messages" };
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

        #endregion


        public NavigationMenuViewModel()
        {
            Navigation.PageChanged += OnPageChanged; ;

            var boardsService = IocService.ServiceProvider.GetService<IBoardsService>();
            boardsService.BoardsChanged += OnBoardsChanged;
            boardsService.AddBoard();
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

        private void OnBoardsChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<byte, BoardsCore.CommonTypes.Entities.Board.BoardDto> e)
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
