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

namespace MtmEquipmentApp.ViewModel.Inspections
{
    public partial class InspectionPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Inspection> inspections;

        [ObservableProperty]
        private Inspection selectedInspection;

        public InspectionPageViewModel()
        {
            Inspections = new ObservableCollection<Inspection>();
            LoadInspections();
        }

        // Загрузка данных инспекций из базы
        private void LoadInspections()
        {
            using var db = new AppDbContext();
            var inspectionsList = db.Inspections.Include(i => i.Equipment).Include(i => i.Inspector).ToList();
            Inspections.Clear();
            foreach (var inspection in inspectionsList)
            {
                Inspections.Add(inspection);
            }
        }

        // Команда для добавления новой инспекции
        [RelayCommand]
        public void AddInspection()
        {
            var inspectionEditWindow = new InspectionEditWindow(null);
            inspectionEditWindow.ShowDialog();
            LoadInspections();  // Перезагружаем список после добавления
        }

        // Команда для редактирования выбранной инспекции
        [RelayCommand]
        public void EditInspection()
        {
            if (SelectedInspection == null) return;

            var inspectionEditWindow = new InspectionEditWindow(SelectedInspection);
            inspectionEditWindow.ShowDialog();
            LoadInspections();  // Перезагружаем список после редактирования
        }

        // Команда для удаления выбранной инспекции
        [RelayCommand]
        public void DeleteInspection()
        {
            if (SelectedInspection == null) return;

            using var db = new AppDbContext();
            var inspectionToDelete = db.Inspections.Find(SelectedInspection.Id);
            if (inspectionToDelete != null)
            {
                db.Inspections.Remove(inspectionToDelete);
                db.SaveChanges();
            }
            LoadInspections();  // Перезагружаем список после удаления
        }
    }
}
