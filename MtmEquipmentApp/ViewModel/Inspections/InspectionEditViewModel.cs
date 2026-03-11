using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MtmEquipmentApp.ViewModel.Inspections
{
    public partial class InspectionEditViewModel : ObservableObject
    {
        private readonly Inspection inspection;
        private readonly Window window;

        [ObservableProperty]
        private DateTime date;

        [ObservableProperty]
        private string remarks = "";

        [ObservableProperty]
        private bool isDefective;

        [ObservableProperty]
        private Equipment? selectedEquipment;

        public ObservableCollection<Equipment> EquipmentList { get; set; } = new();

        public InspectionEditViewModel(Inspection? inspection, Window window)
        {
            this.window = window;
            this.inspection = inspection ?? new Inspection();

            LoadEquipment();

            if (inspection != null)
            {
                Date = inspection.Date;
                Remarks = inspection.Remarks;
                IsDefective = inspection.IsDefective;

                SelectedEquipment = EquipmentList
                    .FirstOrDefault(x => x.Id == inspection.EquipmentId);
            }
            else
            {
                Date = DateTime.Now;
            }
        }

        private void LoadEquipment()
        {
            using var db = new AppDbContext();

            var list = db.Equipment
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToList();

            EquipmentList.Clear();

            foreach (var item in list)
                EquipmentList.Add(item);
        }

        [RelayCommand]
        private void Save()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите оборудование.");
                return;
            }

            if (SessionService.CurrentUser == null)
            {
                MessageBox.Show("Не найден текущий пользователь.");
                return;
            }

            using var db = new AppDbContext();

            inspection.Date = Date;
            inspection.Remarks = Remarks;
            inspection.IsDefective = IsDefective;
            inspection.EquipmentId = SelectedEquipment.Id;
            inspection.UserId = SessionService.CurrentUser.Id;

            if (inspection.Id == 0)
                db.Inspections.Add(inspection);
            else
                db.Inspections.Update(inspection);

            db.SaveChanges();

            window.DialogResult = true;
            window.Close();
        }
    }
}
