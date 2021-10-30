﻿using LigricUno.Views.Pages;
using LigricViewModels.BaseMvvmTypes;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace LigricViewModels.Pages
{
    public class LoginViewModel : OnNotifyPropertyChanged
    {
        private string _email;

        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private RelayCommand _loginLate;
        public RelayCommand LoginLate => _loginLate ?? (_loginLate = new RelayCommand(LoginLaterMethod));

        private void LoginLaterMethod(object parameter)
        {
            Frame newFrame = new Frame();
            newFrame.Navigate(typeof(HomePage), null, new EntranceNavigationTransitionInfo());
        }
    }
}
