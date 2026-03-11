using MtmEquipmentApp.ViewModel.Equipments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MtmEquipmentApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для EquipmentEditWindow.xaml
    /// </summary>
    public partial class EquipmentEditWindow : Window
    {
        public EquipmentEditWindow(int? equipmentId = null)
        {
            InitializeComponent();
            DataContext = new EquipmentEditViewModel(this, equipmentId);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
