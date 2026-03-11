using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.ViewModel.Departments
{
    public partial class DepartmentEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        private Department _department;

        public DepartmentEditViewModel(Department department)
        {
            if (department != null)
            {
                _department = department;
                Name = department.Name;
            }
            else
            {
                _department = new Department();
            }
        }
        public DepartmentEditViewModel()
        {
            
        }
        // Сохранение данных
        [RelayCommand]
        public void Save()
        {
            using var db = new AppDbContext();

            if (_department.Id == 0) // Это новое подразделение
            {
                _department.Name = Name;
                db.Departments.Add(_department);
            }
            else // Это редактируемое подразделение
            {
                _department.Name = Name;
                db.Departments.Update(_department);
            }

            db.SaveChanges();
        }

        // Закрытие окна
        [RelayCommand]
        public void Cancel()
        {
            // Здесь можно добавить логику для отмены изменений
        }
    }
}
