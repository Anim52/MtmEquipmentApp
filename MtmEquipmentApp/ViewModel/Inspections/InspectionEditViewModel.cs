using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MtmEquipmentApp.ViewModel.Inspections
{
    public partial class InspectionEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime inspectionDate;

        [ObservableProperty]
        private string remarks;

        [ObservableProperty]
        private Equipment selectedEquipment;

        [ObservableProperty]
        private User selectedInspector;

        public ObservableCollection<Equipment> EquipmentList { get; } = new ObservableCollection<Equipment>();
        public ObservableCollection<User> InspectorList { get; } = new ObservableCollection<User>();

        private Inspection _inspection;

        public InspectionEditViewModel(Inspection inspection)
        {
            if (inspection != null)
            {
                _inspection = inspection;
                InspectionDate = inspection.Date;
                Remarks = inspection.Remarks;
                SelectedEquipment = inspection.Equipment;
                SelectedInspector = inspection.Inspector;
            }
            else
            {
                _inspection = new Inspection();
                InspectionDate = DateTime.Now; // по умолчанию текущая дата
            }

            using var db = new AppDbContext();
            var equipmentList = db.Equipment.ToList();
            var inspectorList = db.Users.Where(u => u.Role == UserRole.Inspector).ToList();

            foreach (var equipment in equipmentList)
            {
                EquipmentList.Add(equipment);
            }

            foreach (var inspector in inspectorList)
            {
                InspectorList.Add(inspector);
            }
        }

        // Сохранение данных инспекции
        [RelayCommand]
        public void Save()
        {
            using var db = new AppDbContext();

            _inspection.Date = InspectionDate;
            _inspection.Remarks = Remarks;
            _inspection.Equipment = SelectedEquipment;
            _inspection.Inspector = SelectedInspector;

            if (_inspection.Id == 0) // новая инспекция
            {
                db.Inspections.Add(_inspection);
            }
            else // редактируем существующую
            {
                db.Inspections.Update(_inspection);
            }

            db.SaveChanges();
        }

        // Отмена
        [RelayCommand]
        public void Cancel()
        {
            // Логика отмены
        }
    }
}
