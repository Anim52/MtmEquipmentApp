using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel.Equipments
{
    public partial class EquipmentEditViewModel : ObservableObject
    {
        private readonly Window window;
        private readonly int? equipmentId;

        [ObservableProperty]
        private string windowTitle = "Добавление оборудования";

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string inventoryNumber = string.Empty;

        [ObservableProperty]
        private EquipmentStatus selectedStatus = EquipmentStatus.Normal;

        [ObservableProperty]
        private Department? selectedDepartment;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<EquipmentStatus> Statuses { get; } = new();
        public ObservableCollection<Department> Departments { get; } = new();

        public EquipmentEditViewModel(Window window, int? equipmentId = null)
        {
            this.window = window;
            this.equipmentId = equipmentId;

            LoadLookups();

            if (equipmentId.HasValue)
            {
                WindowTitle = "Редактирование оборудования";
                LoadEquipment(equipmentId.Value);
            }
        }

        private void LoadLookups()
        {
            foreach (var status in System.Enum.GetValues(typeof(EquipmentStatus)).Cast<EquipmentStatus>())
                Statuses.Add(status);

            using var db = new AppDbContext();

            var departments = db.Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToList();

            Departments.Clear();
            foreach (var item in departments)
                Departments.Add(item);

            SelectedDepartment ??= Departments.FirstOrDefault();
        }

        private void LoadEquipment(int id)
        {
            using var db = new AppDbContext();

            var entity = db.Equipment
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return;

            Name = entity.Name;
            InventoryNumber = entity.InventoryNumber;
            SelectedStatus = entity.Status;
            SelectedDepartment = Departments.FirstOrDefault(x => x.Id == entity.DepartmentId);
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Введите название оборудования.";
                return;
            }

            if (string.IsNullOrWhiteSpace(InventoryNumber))
            {
                ErrorMessage = "Введите инвентарный номер.";
                return;
            }

            if (SelectedDepartment == null)
            {
                ErrorMessage = "Выберите подразделение.";
                return;
            }

            using var db = new AppDbContext();

            var trimmedName = Name.Trim();
            var trimmedNumber = InventoryNumber.Trim();

            var exists = db.Equipment.Any(x =>
                x.InventoryNumber == trimmedNumber &&
                x.Id != (equipmentId ?? 0));

            if (exists)
            {
                ErrorMessage = "Оборудование с таким инвентарным номером уже существует.";
                return;
            }

            if (equipmentId.HasValue)
            {
                var entity = db.Equipment.FirstOrDefault(x => x.Id == equipmentId.Value);
                if (entity == null)
                {
                    ErrorMessage = "Оборудование не найдено.";
                    return;
                }

                entity.Name = trimmedName;
                entity.InventoryNumber = trimmedNumber;
                entity.Status = SelectedStatus;
                entity.DepartmentId = SelectedDepartment.Id;
            }
            else
            {
                db.Equipment.Add(new Models.Equipment
                {
                    Name = trimmedName,
                    InventoryNumber = trimmedNumber,
                    Status = SelectedStatus,
                    DepartmentId = SelectedDepartment.Id
                });
            }

            db.SaveChanges();
            window.DialogResult = true;
            window.Close();
        }
    }
}
