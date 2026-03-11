using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MtmEquipmentApp.ViewModel.Users
{
    public partial class UserPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> users;

        [ObservableProperty]
        private User selectedUser;

        public UserPageViewModel()
        {
            Users = new ObservableCollection<User>();
            LoadUsers();
        }

        // Загрузка данных пользователей из базы
        private void LoadUsers()
        {
            using var db = new AppDbContext();
            var usersList = db.Users.ToList();
            Users.Clear();
            foreach (var user in usersList)
            {
                Users.Add(user);
            }
        }

        // Команда для добавления нового пользователя
        [RelayCommand]
        public void AddUser()
        {
            var userEditWindow = new UserEditWindow(null);
            userEditWindow.ShowDialog();
            LoadUsers();  // Перезагружаем список после добавления
        }

        // Команда для редактирования выбранного пользователя
        [RelayCommand]
        public void EditUser()
        {
            if (SelectedUser == null) return;

            var userEditWindow = new UserEditWindow(SelectedUser);
            userEditWindow.ShowDialog();
            LoadUsers();  // Перезагружаем список после редактирования
        }

        // Команда для удаления выбранного пользователя
        [RelayCommand]
        public void DeleteUser()
        {
            if (SelectedUser == null) return;

            using var db = new AppDbContext();
            var userToDelete = db.Users.Find(SelectedUser.Id);
            if (userToDelete != null)
            {
                db.Users.Remove(userToDelete);
                db.SaveChanges();
            }
            LoadUsers();  // Перезагружаем список после удаления
        }
    }
}
