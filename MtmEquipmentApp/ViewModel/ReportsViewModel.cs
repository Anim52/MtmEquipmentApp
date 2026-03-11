using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MtmEquipmentApp.ViewModel
{
    public partial class ReportsViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime? dateFrom;

        [ObservableProperty]
        private DateTime? dateTo;

        [ObservableProperty]
        private ObservableCollection<Department> departments = new();

        [ObservableProperty]
        private Department? selectedDepartment;

        [ObservableProperty]
        private int totalEquipment;

        [ObservableProperty]
        private int totalInspections;

        [ObservableProperty]
        private int totalDefects;

        [ObservableProperty]
        private ObservableCollection<ReportRow> reportRows = new();

        public ReportsViewModel()
        {
            LoadDepartments();
            Generate();
        }

        [RelayCommand]
        private void Reset()
        {
            DateFrom = null;
            DateTo = null;
            SelectedDepartment = Departments.FirstOrDefault();
            Generate();
        }

        [RelayCommand]
        private void Generate()
        {
            using var db = new AppDbContext();

            var equipmentQuery = db.Equipment
                .Include(x => x.Department)
                .Include(x => x.Inspections)
                .AsNoTracking()
                .AsQueryable();

            if (SelectedDepartment != null && SelectedDepartment.Id != 0)
                equipmentQuery = equipmentQuery.Where(x => x.DepartmentId == SelectedDepartment.Id);

            var equipmentList = equipmentQuery.ToList();

            if (DateFrom.HasValue || DateTo.HasValue)
            {
                foreach (var item in equipmentList)
                {
                    item.Inspections = item.Inspections
                        .Where(i =>
                            (!DateFrom.HasValue || i.Date.Date >= DateFrom.Value.Date) &&
                            (!DateTo.HasValue || i.Date.Date <= DateTo.Value.Date))
                        .ToList();
                }
            }

            TotalEquipment = equipmentList.Count;
            TotalInspections = equipmentList.Sum(x => x.Inspections.Count);
            TotalDefects = equipmentList.Sum(x => x.Inspections.Count(i => i.IsDefective));

            ReportRows.Clear();
            foreach (var item in equipmentList.OrderBy(x => x.Name))
            {
                ReportRows.Add(new ReportRow
                {
                    EquipmentName = item.Name,
                    InventoryNumber = item.InventoryNumber,
                    DepartmentName = item.Department?.Name ?? "-",
                    Status = item.Status.ToString(),
                    InspectionCount = item.Inspections.Count,
                    HasDefects = item.Inspections.Any(i => i.IsDefective)
                });
            }
        }

        private void LoadDepartments()
        {
            using var db = new AppDbContext();

            Departments.Clear();
            Departments.Add(new Department { Id = 0, Name = "Все подразделения" });

            foreach (var item in db.Departments.AsNoTracking().OrderBy(x => x.Name))
                Departments.Add(item);

            SelectedDepartment = Departments.FirstOrDefault();
        }
    }
}
