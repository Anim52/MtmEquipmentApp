using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;

namespace MtmEquipmentApp.ViewModel.Users
{
    public partial class UserEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private UserRole role;

        private User _user;

        // Список ролей
        public ObservableCollection<UserRole> Roles { get; } = new ObservableCollection<UserRole>
        {
            UserRole.Admin,
            UserRole.Inspector,
            UserRole.Viewer
        };

        public UserEditViewModel(User user)
        {
            if (user != null)
            {
                _user = user;
                FullName = user.FullName;
                Login = user.Login;
                Role = user.Role;
            }
            else
            {
                _user = new User();
                Role = UserRole.Viewer; // Роль по умолчанию
            }
        }
        public UserEditViewModel()
        {
            
        }

        // Сохранение данных пользователя
        [RelayCommand]
        public void Save()
        {
            using var db = new AppDbContext();

            _user.FullName = FullName;
            _user.Login = Login;
            _user.Role = Role;

            if (_user.Id == 0) // Новый пользователь
            {
                db.Users.Add(_user);
            }
            else // Редактируем существующего пользователя
            {
                db.Users.Update(_user);
            }

            db.SaveChanges();
        }

        // Закрытие окна
        [RelayCommand]
        public void Cancel()
        {
            // Логика для отмены редактирования
        }
    }
}
