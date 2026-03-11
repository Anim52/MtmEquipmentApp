using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel.Departments
{
    public partial class DepartmentEditViewModel : ObservableObject
    {
        private readonly Window window;
        private readonly int? departmentId;

        [ObservableProperty]
        private string windowTitle = "Добавление подразделения";

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public DepartmentEditViewModel(Window window, int? departmentId = null)
        {
            this.window = window;
            this.departmentId = departmentId;

            if (departmentId.HasValue)
            {
                WindowTitle = "Редактирование подразделения";
                LoadDepartment(departmentId.Value);
            }
        }

        private void LoadDepartment(int id)
        {
            using var db = new AppDbContext();

            var entity = db.Departments
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return;

            Name = entity.Name;
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Введите название подразделения.";
                return;
            }

            using var db = new AppDbContext();

            var trimmedName = Name.Trim();

            var exists = db.Departments
                .Any(x => x.Name == trimmedName && x.Id != (departmentId ?? 0));

            if (exists)
            {
                ErrorMessage = "Подразделение с таким названием уже существует.";
                return;
            }

            if (departmentId.HasValue)
            {
                var entity = db.Departments.FirstOrDefault(x => x.Id == departmentId.Value);
                if (entity == null)
                {
                    ErrorMessage = "Подразделение не найдено.";
                    return;
                }

                entity.Name = trimmedName;
            }
            else
            {
                db.Departments.Add(new Department
                {
                    Name = trimmedName
                });
            }

            db.SaveChanges();
            window.DialogResult = true;
            window.Close();
        }
    }
}
