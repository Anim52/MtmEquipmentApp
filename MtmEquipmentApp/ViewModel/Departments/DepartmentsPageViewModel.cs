using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel.Departments
{
    public partial class DepartmentViewModel : ObservableObject
    {
        private readonly ObservableCollection<Department> allDepartments = new();

        [ObservableProperty]
        private ObservableCollection<Department> departments = new();

        [ObservableProperty]
        private Department? selectedDepartment;

        [ObservableProperty]
        private string searchText = string.Empty;

        public DepartmentViewModel()
        {
            LoadDepartments();
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilter();
        }

        [RelayCommand]
        private void Refresh()
        {
            LoadDepartments();
        }

        [RelayCommand]
        private void Add()
        {
            var window = new DepartmentEditWindow();
            if (window.ShowDialog() == true)
            {
                LoadDepartments();
            }
        }

        [RelayCommand]
        private void Edit()
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Выберите подразделение для редактирования.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var window = new DepartmentEditWindow(SelectedDepartment.Id);
            if (window.ShowDialog() == true)
            {
                LoadDepartments();
            }
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Выберите подразделение для удаления.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить подразделение \"{SelectedDepartment.Name}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var entity = db.Departments
                .Include(x => x.Equipments)
                .FirstOrDefault(x => x.Id == SelectedDepartment.Id);

            if (entity == null)
                return;

            if (entity.Equipments.Any())
            {
                MessageBox.Show("Нельзя удалить подразделение, пока к нему привязано оборудование.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            db.Departments.Remove(entity);
            db.SaveChanges();

            LoadDepartments();
        }

        public void LoadDepartments()
        {
            using var db = new AppDbContext();

            var data = db.Departments
                .Include(x => x.Equipments)
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToList();

            allDepartments.Clear();
            foreach (var item in data)
                allDepartments.Add(item);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? allDepartments
                : new ObservableCollection<Department>(
                    allDepartments.Where(x =>
                        x.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));

            Departments.Clear();
            foreach (var item in filtered)
                Departments.Add(item);
        }
    }
}
