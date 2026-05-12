using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Views.Windows;
using System.Collections.ObjectModel;
using System.Windows;

namespace MtmEquipmentApp.ViewModel.Equipments
{
    public partial class ArchivePageViewModel : ObservableObject
    {
        private readonly ObservableCollection<Equipment> allEquipment = new();

        [ObservableProperty]
        private ObservableCollection<Equipment> equipmentItems = new();

        [ObservableProperty]
        private Equipment? selectedEquipment;

        [ObservableProperty]
        private string searchText = string.Empty;

        public ArchivePageViewModel()
        {
            LoadEquipment();
        }

        partial void OnSearchTextChanged(string value) => ApplyFilter();

        [RelayCommand]
        private void Refresh() => LoadEquipment();

        [RelayCommand]
        private void Restore()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите оборудование для восстановления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using var db = new AppDbContext();
            var entity = db.Equipment.FirstOrDefault(x => x.Id == SelectedEquipment.Id);
            if (entity == null)
                return;

            entity.Status = EquipmentStatus.Normal;
            db.SaveChanges();
            LoadEquipment();
        }

        [RelayCommand]
        private void OpenHistory()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите оборудование для просмотра истории.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var window = new EquipmentHistoryWindow(SelectedEquipment.Id);
            window.ShowDialog();
        }

        private void LoadEquipment()
        {
            using var db = new AppDbContext();
            var data = db.Equipment
                .Include(x => x.Department)
                .Where(x => x.Status == EquipmentStatus.Decommissioned)
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToList();

            allEquipment.Clear();
            foreach (var item in data)
                allEquipment.Add(item);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = allEquipment.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var text = SearchText.Trim();
                query = query.Where(x =>
                    x.Name.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                    x.InventoryNumber.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                    (x.Department?.Name?.Contains(text, System.StringComparison.OrdinalIgnoreCase) ?? false));
            }

            EquipmentItems.Clear();
            foreach (var item in query)
                EquipmentItems.Add(item);
        }
    }
}
