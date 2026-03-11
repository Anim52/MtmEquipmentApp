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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MtmEquipmentApp.ViewModel.Inspections
{
    public partial class InspectionViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Inspection> inspections = new();

        [ObservableProperty]
        private Inspection? selectedInspection;

        [ObservableProperty]
        private DateTime? dateFrom;

        [ObservableProperty]
        private DateTime? dateTo;

        public InspectionViewModel()
        {
            LoadInspections();
        }

        [RelayCommand]
        private void Refresh()
        {
            LoadInspections();
        }

        [RelayCommand]
        private void Add()
        {
            var window = new InspectionEditWindow();

            if (window.ShowDialog() == true)
                LoadInspections();
        }

        [RelayCommand]
        private void Edit()
        {
            if (SelectedInspection == null)
            {
                MessageBox.Show("Выберите инспекцию.");
                return;
            }

            var window = new InspectionEditWindow(SelectedInspection);

            if (window.ShowDialog() == true)
                LoadInspections();
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedInspection == null)
            {
                MessageBox.Show("Выберите инспекцию.");
                return;
            }

            var result = MessageBox.Show(
                "Удалить запись проверки?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var entity = db.Inspections.FirstOrDefault(x => x.Id == SelectedInspection.Id);

            if (entity == null)
                return;

            db.Inspections.Remove(entity);
            db.SaveChanges();

            LoadInspections();
        }

        private void LoadInspections()
        {
            using var db = new AppDbContext();

            var query = db.Inspections
                .Include(x => x.Equipment)
                .Include(x => x.Inspector)
                .AsNoTracking()
                .AsQueryable();

            if (DateFrom.HasValue)
                query = query.Where(x => x.Date >= DateFrom.Value);

            if (DateTo.HasValue)
                query = query.Where(x => x.Date <= DateTo.Value);

            var data = query
                .OrderByDescending(x => x.Date)
                .ToList();

            Inspections.Clear();

            foreach (var item in data)
                Inspections.Add(item);
        }
    }
}
