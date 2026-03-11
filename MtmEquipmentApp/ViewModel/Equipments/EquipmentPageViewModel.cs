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

namespace MtmEquipmentApp.ViewModel.Equipments
{
    public partial class EquipmentViewModel : ObservableObject
    {
        private readonly ObservableCollection<Models.Equipment> allEquipment = new();

        [ObservableProperty]
        private ObservableCollection<Models.Equipment> equipmentItems = new();

        [ObservableProperty]
        private Models.Equipment? selectedEquipment;

        [ObservableProperty]
        private ObservableCollection<Department> departments = new();

        [ObservableProperty]
        private Department? selectedDepartmentFilter;

        [ObservableProperty]
        private string searchText = string.Empty;

        public EquipmentViewModel()
        {
            LoadDepartments();
            LoadEquipment();
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilter();
        }

        partial void OnSelectedDepartmentFilterChanged(Department? value)
        {
            ApplyFilter();
        }

        [RelayCommand]
        private void Refresh()
        {
            LoadDepartments();
            LoadEquipment();
        }

        [RelayCommand]
        private void Add()
        {
            var window = new EquipmentEditWindow();
            if (window.ShowDialog() == true)
            {
                LoadEquipment();
            }
        }

        [RelayCommand]
        private void Edit()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите оборудование для редактирования.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var window = new EquipmentEditWindow(SelectedEquipment.Id);
            if (window.ShowDialog() == true)
            {
                LoadEquipment();
            }
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите оборудование для удаления.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить оборудование \"{SelectedEquipment.Name}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var entity = db.Equipment
                .FirstOrDefault(x => x.Id == SelectedEquipment.Id);

            if (entity == null)
                return;

            db.Equipment.Remove(entity);
            db.SaveChanges();

            LoadEquipment();
        }

        private void LoadDepartments()
        {
            using var db = new AppDbContext();

            var data = db.Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToList();

            Departments.Clear();
            Departments.Add(new Department { Id = 0, Name = "Все подразделения" });

            foreach (var item in data)
                Departments.Add(item);

            if (SelectedDepartmentFilter == null)
                SelectedDepartmentFilter = Departments.FirstOrDefault();
        }

        private void LoadEquipment()
        {
            using var db = new AppDbContext();

            var data = db.Equipment
                .Include(x => x.Department)
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToList();

            allEquipment.Clear();
            foreach (var item in data)
                allEquipment.Add(item);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = allEquipment.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var text = SearchText.Trim();
                query = query.Where(x =>
                    x.Name.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                    x.InventoryNumber.Contains(text, System.StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedDepartmentFilter != null && SelectedDepartmentFilter.Id != 0)
            {
                query = query.Where(x => x.DepartmentId == SelectedDepartmentFilter.Id);
            }

            EquipmentItems.Clear();
            foreach (var item in query)
                EquipmentItems.Add(item);
        }
    }
}
