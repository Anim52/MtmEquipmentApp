using MtmEquipmentApp.ViewModel.Equipments;
using System.Windows;

namespace MtmEquipmentApp.Views.Windows
{
    public partial class EquipmentHistoryWindow : Window
    {
        public EquipmentHistoryWindow(int equipmentId)
        {
            InitializeComponent();
            DataContext = new EquipmentHistoryViewModel(equipmentId);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
