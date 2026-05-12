using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using System.Collections.ObjectModel;

namespace MtmEquipmentApp.ViewModel.Equipments
{
    public partial class EquipmentHistoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private string equipmentName = "-";

        [ObservableProperty]
        private string inventoryNumber = "-";

        [ObservableProperty]
        private string departmentName = "-";

        [ObservableProperty]
        private string status = "-";

        [ObservableProperty]
        private string purchaseDate = "Не указана";

        [ObservableProperty]
        private int inspectionsCount;

        [ObservableProperty]
        private int repairsCount;

        [ObservableProperty]
        private string defectReasons = "Нет данных о поломках.";

        [ObservableProperty]
        private ObservableCollection<Inspection> inspections = new();

        public EquipmentHistoryViewModel(int equipmentId)
        {
            Load(equipmentId);
        }

        private void Load(int equipmentId)
        {
            using var db = new AppDbContext();

            var equipment = db.Equipment
                .Include(x => x.Department)
                .Include(x => x.Inspections)
                    .ThenInclude(x => x.Inspector)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == equipmentId);

            if (equipment == null)
                return;

            EquipmentName = equipment.Name;
            InventoryNumber = equipment.InventoryNumber;
            DepartmentName = equipment.Department?.Name ?? "-";
            Status = equipment.Status.ToString();

            var orderedInspections = equipment.Inspections
                .OrderByDescending(x => x.Date)
                .ToList();

            InspectionsCount = orderedInspections.Count;
            RepairsCount = orderedInspections.Count(x => x.IsDefective);

            var reasons = orderedInspections
                .Where(x => x.IsDefective && !string.IsNullOrWhiteSpace(x.Remarks))
                .Select(x => $"{x.Date:dd.MM.yyyy}: {x.Remarks}")
                .ToList();

            if (reasons.Any())
                DefectReasons = string.Join("\n", reasons);

            Inspections.Clear();
            foreach (var inspection in orderedInspections)
                Inspections.Add(inspection);
        }
    }
}
