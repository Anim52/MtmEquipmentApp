using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel.Users
{
    public partial class UserEditViewModel : ObservableObject
    {
        private readonly Window window;
        private readonly int? userId;

        [ObservableProperty]
        private string windowTitle = "Добавление пользователя";

        [ObservableProperty]
        private string fullName = string.Empty;

        [ObservableProperty]
        private string login = string.Empty;

        [ObservableProperty]
        private UserRole selectedRole = UserRole.Viewer;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<UserRole> Roles { get; } = new();

        public UserEditViewModel(Window window, int? userId = null)
        {
            this.window = window;
            this.userId = userId;

            foreach (var role in System.Enum.GetValues(typeof(UserRole)).Cast<UserRole>())
                Roles.Add(role);

            if (userId.HasValue)
            {
                WindowTitle = "Редактирование пользователя";
                LoadUser(userId.Value);
            }
        }

        private void LoadUser(int id)
        {
            using var db = new AppDbContext();

            var entity = db.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return;

            FullName = entity.FullName;
            Login = entity.Login;
            SelectedRole = entity.Role;
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(FullName))
            {
                ErrorMessage = "Введите ФИО пользователя.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorMessage = "Введите логин.";
                return;
            }

            using var db = new AppDbContext();

            var trimmedFullName = FullName.Trim();
            var trimmedLogin = Login.Trim();

            var loginExists = db.Users.Any(x =>
                x.Login == trimmedLogin &&
                x.Id != (userId ?? 0));

            if (loginExists)
            {
                ErrorMessage = "Пользователь с таким логином уже существует.";
                return;
            }

            if (userId.HasValue)
            {
                var entity = db.Users.FirstOrDefault(x => x.Id == userId.Value);
                if (entity == null)
                {
                    ErrorMessage = "Пользователь не найден.";
                    return;
                }

                entity.FullName = trimmedFullName;
                entity.Login = trimmedLogin;
                entity.Role = SelectedRole;

                if (!string.IsNullOrWhiteSpace(Password))
                    entity.PasswordHash = PasswordHasher.HashPassword(Password);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Введите пароль для нового пользователя.";
                    return;
                }

                db.Users.Add(new User
                {
                    FullName = trimmedFullName,
                    Login = trimmedLogin,
                    Role = SelectedRole,
                    PasswordHash = PasswordHasher.HashPassword(Password)
                });
            }

            db.SaveChanges();
            window.DialogResult = true;
            window.Close();
        }
    }
}
