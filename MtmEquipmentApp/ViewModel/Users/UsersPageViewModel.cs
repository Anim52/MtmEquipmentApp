using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel.Users
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly ObservableCollection<User> allUsers = new();

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private User? selectedUser;

        [ObservableProperty]
        private ObservableCollection<string> roles = new();

        [ObservableProperty]
        private string selectedRoleFilter = "Все роли";

        [ObservableProperty]
        private string searchText = string.Empty;

        public UserViewModel()
        {
            LoadRoles();
            LoadUsers();
        }

        partial void OnSearchTextChanged(string value) => ApplyFilter();
        partial void OnSelectedRoleFilterChanged(string value) => ApplyFilter();

        [RelayCommand]
        private void Refresh()
        {
            LoadRoles();
            LoadUsers();
        }

        [RelayCommand]
        private void Add()
        {
            var window = new UserEditWindow();
            if (window.ShowDialog() == true)
                LoadUsers();
        }

        [RelayCommand]
        private void Edit()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var window = new UserEditWindow(SelectedUser.Id);
            if (window.ShowDialog() == true)
                LoadUsers();
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (SelectedUser.Login == "admin")
            {
                MessageBox.Show("Удалять администратора admin нельзя.",
                    "Ограничение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить пользователя \"{SelectedUser.FullName}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var entity = db.Users
                .Include(x => x.Inspections)
                .FirstOrDefault(x => x.Id == SelectedUser.Id);

            if (entity == null)
                return;

            if (entity.Inspections.Any())
            {
                MessageBox.Show("Нельзя удалить пользователя, у которого есть записи инспекций.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            db.Users.Remove(entity);
            db.SaveChanges();

            LoadUsers();
        }

        private void LoadRoles()
        {
            Roles.Clear();
            Roles.Add("Все роли");

            foreach (var role in System.Enum.GetNames(typeof(UserRole)))
                Roles.Add(role);

            SelectedRoleFilter ??= "Все роли";
        }

        public void LoadUsers()
        {
            using var db = new AppDbContext();

            var data = db.Users
                .AsNoTracking()
                .OrderBy(x => x.FullName)
                .ToList();

            allUsers.Clear();
            foreach (var item in data)
                allUsers.Add(item);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = allUsers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var text = SearchText.Trim();
                query = query.Where(x =>
                    x.FullName.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                    x.Login.Contains(text, System.StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SelectedRoleFilter) && SelectedRoleFilter != "Все роли")
            {
                query = query.Where(x => x.Role.ToString() == SelectedRoleFilter);
            }

            Users.Clear();
            foreach (var item in query)
                Users.Add(item);
        }
    }
}
