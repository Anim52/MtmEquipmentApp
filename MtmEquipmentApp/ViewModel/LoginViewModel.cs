using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Services;
using MtmEquipmentApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string login = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [RelayCommand]
        private void LoginUser()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorMessage = "Введите логин.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите пароль.";
                return;
            }

            using var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(x => x.Login == Login);

            if (user is null)
            {
                ErrorMessage = "Пользователь не найден.";
                return;
            }

            if (!PasswordHasher.VerifyPassword(Password, user.PasswordHash))
            {
                ErrorMessage = "Неверный пароль.";
                return;
            }

            SessionService.CurrentUser = user;
            ErrorMessage = string.Empty;

            var mainWindow = new MainWindow();
            mainWindow.Show();

            foreach (var window in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
