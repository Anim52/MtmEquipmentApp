using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MtmEquipmentApp.ViewModel.Departments
{
    public partial class DepartmentPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Department> departments;

        [ObservableProperty]
        private Department selectedDepartment;

        public DepartmentPageViewModel()
        {
            Departments = new ObservableCollection<Department>();
            LoadDepartments();
        }

        // Загрузка данных из базы
        private void LoadDepartments()
        {
            using var db = new AppDbContext();
            var departmentsList = db.Departments.ToList();
            Departments.Clear();
            foreach (var department in departmentsList)
            {
                Departments.Add(department);
            }
        }

        // Команда для добавления нового подразделения
        [RelayCommand]
        public void AddDepartment()
        {
            var departmentEditWindow = new DepartmentEditWindow(null);
            departmentEditWindow.ShowDialog();
            LoadDepartments();  // Перезагружаем список после добавления
        }

        // Команда для редактирования выбранного подразделения
        [RelayCommand]
        public void EditDepartment()
        {
            if (SelectedDepartment == null) return;

            var departmentEditWindow = new DepartmentEditWindow(SelectedDepartment);
            departmentEditWindow.ShowDialog();
            LoadDepartments();  // Перезагружаем список после редактирования
        }

        // Команда для удаления выбранного подразделения
        [RelayCommand]
        public void DeleteDepartment()
        {
            if (SelectedDepartment == null) return;

            using var db = new AppDbContext();
            var departmentToDelete = db.Departments.Find(SelectedDepartment.Id);
            if (departmentToDelete != null)
            {
                db.Departments.Remove(departmentToDelete);
                db.SaveChanges();
            }
            LoadDepartments();  // Перезагружаем список после удаления
        }
    }
}
